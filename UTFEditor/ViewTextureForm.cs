using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpDX.Direct3D9;

namespace UTFEditor
{
    public partial class ViewTextureForm : Form
    {
        TreeNode node;
        Bitmap[] tex;
        byte[][,] alphamap;
        int level;
        Device device = null;

        public ViewTextureForm(TreeNode node, string title)
        {
            this.node = node;
            InitializeComponent();
            this.Text = title;
        }

        private void ViewTextureForm_Load(object sender, EventArgs e)
        {
            if (node.Nodes.Count != 0)
            {
                TreeNode child = node.Nodes["MIPS"];
                if (child == null)
                {
                    child = node.Nodes["MIP0"];
                    if (child == null)
                        child = node.Nodes[0];
                }
                node = child;
            }
            if (Utilities.StrIEq(node.Name, "MIPS"))
            {
                tex = ReadDDS(node.Tag as byte[]);
                if (tex == null)
                {
                    tex = new Bitmap[1];
                    tex[0] = LoadTexture(node.Tag as byte[]);
                }
                level = 0;
                this.Text += ".dds";
            }
            else
            {
                this.Text += ".tga";
                int lev;
                int.TryParse(node.Name.Substring(3), out lev);
                node = node.Parent;
                tex = new Bitmap[node.Nodes.Count];
                alphamap = new byte[node.Nodes.Count][,];
                foreach (TreeNode n in node.Nodes)
                {
                    if (int.TryParse(n.Name.Substring(3), out level))
                    {
                        tex[level] = ReadTGA(n.Tag as byte[]);
                        if (tex[level] == null)
                            tex[level] = LoadTexture(n.Tag as byte[]);
                    }
                }
                level = lev;
            }
            checkBoxFlip.Checked = true;
            spinnerLevel.Maximum = tex.Length - 1;
            spinnerLevel.Value = level;
            if (tex.Length == 1)
            {
                label1.Visible = false;
                spinnerLevel.Visible = false;
            }

            if (tex[0].Width > 256 || tex[0].Height > 256)
                ClientSize = new Size(ClientSize.Width + 256, ClientSize.Height + 256);
            if (tex[level].Width > 512 || tex[level].Height > 512)
                checkBoxStretch.Checked = true;
        }

        private void checkBoxStretch_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = (checkBoxStretch.Checked) ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.CenterImage;
        }

        private void spinnerLevel_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = tex[(int)spinnerLevel.Value];
            labelSize.Text = String.Format("{0}x{1}", pictureBox1.Image.Width, pictureBox1.Image.Height);
        }

        private void checkBoxTransparent_Click(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle();
            for (int i = 0; i < tex.Length; ++i)
            {
                rect.Width = tex[i].Width;
                rect.Height = tex[i].Height;
                BitmapData bmdata = tex[i].LockBits(rect, ImageLockMode.WriteOnly, tex[i].PixelFormat);
                int bytes = bmdata.Stride * tex[i].Height;
                byte[] data = new byte[bytes];
                Marshal.Copy(bmdata.Scan0, data, 0, bytes);
                if (checkBoxTransparent.Checked)
                {
                    if (checkBoxFlip.Checked)
                    {
                        for (int p = 3, y = rect.Height; --y >= 0; )
                            for (int x = 0; x < rect.Width; p += 4, ++x)
                                data[p] = alphamap[i][y, x];
                    }
                    else
                    {
                        for (int p = 3, y = 0; y < rect.Height; ++y)
                            for (int x = 0; x < rect.Width; p += 4, ++x)
                                data[p] = alphamap[i][y, x];
                    }
                }
                else
                {
                    for (int p = 3; p < bytes; p += 4)
                        data[p] = 255;
                }
                Marshal.Copy(data, 0, bmdata.Scan0, bytes);
                tex[i].UnlockBits(bmdata);
            }
            pictureBox1.Invalidate();
        }

        private void checkBoxFlip_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Bitmap bm in tex)
            {
                bm.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            pictureBox1.Invalidate();
        }

        // Information gleaned from Ignacio Castano (NVIDIA Texture Tools)
        // and Volker Gärtner and Sherman Wilcox (FreeImage 3).
        private Bitmap[] ReadDDS(byte[] data)
        {
            int pos;
            DDSHeader dds = new DDSHeader();
            if (!dds.Read(data, out pos))
                return null;
            // Require height, width and pixel format
            if ((dds.flags & 0x1006) != 0x1006)
                return null;
            // FourCC present or RGB, with alpha. 
            if (dds.pflags != 4 && dds.pflags != 0x40 && dds.pflags != 0x41)
                return null;
            bool DXT1 = (dds.FourCC == "DXT1");
            bool DXT3 = (dds.FourCC == "DXT3");
            bool DXT5 = (dds.FourCC == "DXT5");
            if (dds.pflags == 4 && !(DXT1 || DXT3 || DXT5))
                return null;
            if (dds.pflags == 4)
            {
                // DXT5 textures look better without transparency (atmosphere being an exception).
                if (DXT5)
                    checkBoxTransparent.Checked = false;
                // Assume DXT1 has no transparency, turned back on if it does.
                else if (DXT1)
                    checkBoxTransparent.Visible = false;
            }
            else if (dds.bpp == 16)
            {
                if (dds.pflags == 0x41)
                {
                    if (dds.rmask != 0x7C00 && dds.gmask != 0x03E0 && dds.bmask != 0x001F && dds.amask != 0x8000)
                        return null;
                    // These look better without transparency.
                    checkBoxTransparent.Checked = false;
                }
                else
                {
                    if (dds.rmask != 0xF800 && dds.gmask != 0x07E0 && dds.bmask != 0x001F)
                        return null;
                    checkBoxTransparent.Visible = false;
                }
            }
            else if (dds.bpp == 24)
            {
                if (dds.rmask != 0xFF0000 && dds.gmask != 0x00FF00 && dds.bmask != 0x0000FF)
                    return null;
                checkBoxTransparent.Visible = false;
            }
            else
            {
                return null;
            }
            
            Bitmap[] tex = new Bitmap[dds.mipmapcount];
            alphamap = new byte[dds.mipmapcount][,];
            for (int level = 0; level < dds.mipmapcount; ++level)
            {
                tex[level] = new Bitmap(dds.width, dds.height);
                alphamap[level] = new byte[dds.height, dds.width];
                byte[] alpha = null;
                byte[] index = null;
                Rectangle rect = new Rectangle(0, 0, dds.width, dds.height);
                BitmapData bmdata = tex[level].LockBits(rect, ImageLockMode.WriteOnly, tex[level].PixelFormat);
                int bytes = bmdata.Stride * dds.height;
                byte[] pdata = new byte[bytes];
                if (dds.pflags == 4)
                {
                    for (int y = 0; y < dds.height; y += 4)
                    {
                        for (int x = 0; x < dds.width; x += 4)
                        {
                            if (DXT3)
                            {
                                alpha = GetDXT3Alpha(data, ref pos);
                            }
                            else if (DXT5)
                            {
                                alpha = GetDXT5Alpha(data, ref pos, out index);
                            }
                            Color[] col = GetBlockColors(data, ref pos);
                            for (int y1 = 0; y1 < 4; ++y1)
                            {
                                int  p = (y + y1) * bmdata.Stride + x * 4;
                                byte val = data[pos++];
                                for (int x1 = 0; x1 < 4; ++x1)
                                {
                                    Color c = col[val & 3];
                                    byte  a;
                                    if (DXT3)
                                    {
                                        a = alphamap[level][y + y1, x + x1] = alpha[y1 * 4 + x1];
                                    }
                                    else if (DXT5)
                                    {
                                        alphamap[level][y + y1, x + x1] = alpha[index[y1 * 4 + x1]];
                                        a = 0xFF;
                                    }
                                    else // (DXT1)
                                    {
                                        a = alphamap[level][y + y1, x + x1] = c.A;
                                        if (a != 0xFF)
                                            checkBoxTransparent.Visible = true;
                                    }
                                    pdata[p++] = c.B;
                                    pdata[p++] = c.G;
                                    pdata[p++] = c.R;
                                    pdata[p++] = a;
                                    val >>= 2;
                                }
                            }
                        }
                    }
                }
                else if (dds.bpp == 24) // dds.pflags == 0x40
                {
                    for (int p = 0; p < bytes; )
                    {
                        pdata[p++] = data[pos++];
                        pdata[p++] = data[pos++];
                        pdata[p++] = data[pos++];
                        pdata[p++] = 0xFF;
                    }
                }
                else if (dds.pflags == 0x40) // dds.bpp == 16
                {
                    for (int p = 0; p < bytes; )
                    {
                        int val = Utilities.GetWord(data, ref pos);
                        int r = (val & dds.rmask) >> 11;
                        int g = (val & dds.gmask) >> 5;
                        int b = (val & dds.bmask);
                        pdata[p++] = (byte)((b << 3) | (b >> 2));
                        pdata[p++] = (byte)((g << 2) | (g >> 4));
                        pdata[p++] = (byte)((r << 3) | (r >> 2));
                        pdata[p++] = 0xFF;
                    }
                }
                else // (dds.pflags == 0x41 && dds.bpp == 16)
                {
                    int p = 0;
                    for (int y = 0; y < dds.height; ++y)
                    {
                        for (int x = 0; x < dds.width; ++x)
                        {
                            int val = Utilities.GetWord(data, ref pos);
                            int r = (val & dds.rmask) >> 10;
                            int g = (val & dds.gmask) >> 5;
                            int b = (val & dds.bmask);
                            pdata[p++] = (byte)((b << 3) | (b >> 2));
                            pdata[p++] = (byte)((g << 3) | (g >> 2));
                            pdata[p++] = (byte)((r << 3) | (r >> 2));
                            pdata[p++] = 0xFF;
                            alphamap[level][y, x] = (byte)((short)val >> 15);
                        }
                    }
                }
                Marshal.Copy(pdata, 0, bmdata.Scan0, bytes);
                tex[level].UnlockBits(bmdata);
                dds.width >>= 1;
                dds.height >>= 1;
                if (dds.pflags == 4 && (dds.width < 4 || dds.height < 4))
                {
                    Array.Resize(ref tex, level + 1);
                    break;
                }
            }

            return tex;
        }

        private Color[] GetBlockColors(byte[] data, ref int pos)
        {
            int col1 = Utilities.GetWord(data, ref pos);
            int col2 = Utilities.GetWord(data, ref pos);
            int r1 = (col1 >> 11);
            int g1 = (col1 >> 5) & 0x3f;
            int b1 = (col1 & 0x1f);
            int r2 = (col2 >> 11);
            int g2 = (col2 >> 5) & 0x3f;
            int b2 = (col2 & 0x1f);
            
            Color[] col = new Color[4];
            col[0] = Color.FromArgb((r1 << 3) | (r1 >> 2),
                                    (g1 << 2) | (g1 >> 4),
                                    (b1 << 3) | (b1 >> 2));
            col[1] = Color.FromArgb((r2 << 3) | (r2 >> 2),
                                    (g2 << 2) | (g2 >> 4),
                                    (b2 << 3) | (b2 >> 2));
            if (col1 > col2)
            {
                col[2] = Color.FromArgb((2 * col[0].R + col[1].R) / 3,
                                        (2 * col[0].G + col[1].G) / 3,
                                        (2 * col[0].B + col[1].B) / 3);
                col[3] = Color.FromArgb((2 * col[1].R + col[0].R) / 3,
                                        (2 * col[1].G + col[0].G) / 3,
                                        (2 * col[1].B + col[0].B) / 3);
            }
            else
            {
                col[2] = Color.FromArgb((col[0].R + col[1].R) / 2,
                                        (col[0].G + col[1].G) / 2,
                                        (col[0].B + col[1].B) / 2);
                col[3] = Color.FromArgb(0);
            }
            
            return col;
        }

        private byte[] GetDXT3Alpha(byte[] data, ref int pos)
        {
            byte[] alpha = new byte[16];
            for (int i = 0; i < 16; i += 2)
            {
                byte val = data[pos++];
                alpha[i] = (byte)(((val & 0x0F) << 4) | (val & 0x0F));
                alpha[i + 1] = (byte)((val & 0xF0) | (val >> 4));
            }
            return alpha;
        }

        private byte[] GetDXT5Alpha(byte[] data, ref int pos, out byte[] index)
        {
            byte[] alpha = new byte[8];
            index = new byte[16];
            ulong val = BitConverter.ToUInt64(data, pos);
            pos += 8;
            alpha[0] = (byte)(val & 0xFF);
            alpha[1] = (byte)((val >> 8) & 0xFF);
            val >>= 16;
            for (int i = 0; i < 16; ++i)
            {
                index[i] = (byte)(val & 7);
                val >>= 3;
            }
            if (alpha[0] > alpha[1])
            {
                alpha[2] = (byte)((6 * alpha[0] + 1 * alpha[1]) / 7);
                alpha[3] = (byte)((5 * alpha[0] + 2 * alpha[1]) / 7);
                alpha[4] = (byte)((4 * alpha[0] + 3 * alpha[1]) / 7);
                alpha[5] = (byte)((3 * alpha[0] + 4 * alpha[1]) / 7);
                alpha[6] = (byte)((2 * alpha[0] + 5 * alpha[1]) / 7);
                alpha[7] = (byte)((1 * alpha[0] + 6 * alpha[1]) / 7);
            }
            else
            {
                alpha[2] = (byte)((4 * alpha[0] + 1 * alpha[1]) / 5);
                alpha[3] = (byte)((3 * alpha[0] + 2 * alpha[1]) / 5);
                alpha[4] = (byte)((2 * alpha[0] + 3 * alpha[1]) / 5);
                alpha[5] = (byte)((1 * alpha[0] + 4 * alpha[1]) / 5);
                alpha[6] = 0x00;
                alpha[7] = 0xFF;
            }
            return alpha;
        }

        private Bitmap ReadTGA(byte[] data)
        {
            int pos;
            TgaHeader tga = new TgaHeader();
            if (!tga.Read(data, out pos))
                return null;
            // Only handle uncompressed mapped and RGB images.
            if (tga.Image_Type == 1)
            {
                if (tga.First_Entry_Index != 0 ||
                    tga.Color_Map_Entry_Size != 24 ||
                    tga.Pixel_Depth != 8)
                    return null;
                checkBoxTransparent.Visible = false;
            }
            else if (tga.Image_Type == 2)
            {
                if (tga.Pixel_Depth == 16 || tga.Pixel_Depth == 24)
                    checkBoxTransparent.Visible = false;
                else if (tga.Pixel_Depth != 32)
                    return null;
            }
            else
            {
                return null;
            }
            
            Bitmap bm = new Bitmap(tga.Image_Width, tga.Image_Height);
            Rectangle rect = new Rectangle(0, 0, tga.Image_Width, tga.Image_Height);
            BitmapData bmdata = bm.LockBits(rect, ImageLockMode.WriteOnly, bm.PixelFormat);
            int bytes = bmdata.Stride * tga.Image_Height;
            byte[] pdata = new byte[bytes];
            // Process the image data depending on its type.
            if (tga.Image_Type == 1)
            {
                int pal = pos;
                pos += 3 * tga.Color_Map_Length;
                for (int y = tga.Image_Height; --y >= 0; )
                {
                    int p = y * bmdata.Stride;
                    for (int x = 0; x < tga.Image_Width; ++x)
                    {
                        int c = pal + 3 * data[pos++];
                        pdata[p++] = data[c++];
                        pdata[p++] = data[c++];
                        pdata[p++] = data[c++];
                        pdata[p++] = 0xFF;
                    }
                }
            }
            else if (tga.Pixel_Depth == 16)
            {
                for (int y = tga.Image_Height; --y >= 0; )
                {
                    int p = y * bmdata.Stride;
                    for (int x = 0; x < tga.Image_Width; ++x)
                    {
                        int val = Utilities.GetWord(data, ref pos);
                        int r = (val & 0x7C00) >> 10;
                        int g = (val & 0x03E0) >> 5;
                        int b = (val & 0x001F);
                        pdata[p++] = (byte)((b << 3) | (b >> 2));
                        pdata[p++] = (byte)((g << 3) | (g >> 2));
                        pdata[p++] = (byte)((r << 3) | (r >> 2));
                        pdata[p++] = 0xFF;
                    }
                }
            }
            else
            {
                bool alpha = false;
                if (tga.Pixel_Depth == 32)
                {
                    alpha = true;
                    alphamap[level] = new byte[tga.Image_Height, tga.Image_Width];
                }
                for (int y = tga.Image_Height; --y >= 0; )
                {
                    int p = y * bmdata.Stride;
                    for (int x = 0; x < tga.Image_Width; ++x)
                    {
                        pdata[p++] = data[pos++];
                        pdata[p++] = data[pos++];
                        pdata[p++] = data[pos++];
                        if (alpha)
                            alphamap[level][y, x] = pdata[p++] = data[pos++];
                        else
                            pdata[p++] = 0xFF;
                    }
                }
            }
            Marshal.Copy(pdata, 0, bmdata.Scan0, bytes);
            bm.UnlockBits(bmdata);
            return bm;
        }

        private Bitmap LoadTexture(byte[] data)
        {
            if (device == null)
            {
                PresentParameters presentParams = new PresentParameters();
                presentParams.Windowed = true;
                presentParams.SwapEffect = SwapEffect.Discard;
                presentParams.BackBufferFormat = Format.Unknown;
                presentParams.EnableAutoDepthStencil = true;

                Format[] formats = { Format.D32, Format.D24X8, Format.D16 };
                foreach (var format in formats)
                {
                    try
                    {
                        presentParams.AutoDepthStencilFormat = format;
                        device = new Device(new Direct3D(), 0, DeviceType.Hardware, pictureBox1.Handle, CreateFlags.SoftwareVertexProcessing, presentParams);
                        break;
                    }
                    catch { }
                }
                if (device == null)
                    throw new Exception("Unable to initialize DirectX.");

                checkBoxTransparent.Visible = false;
            }

            Texture tex;

            using (MemoryStream ms = new MemoryStream(data))
            {
                tex = Texture.FromStream(device, ms);
            }
            return (Bitmap)Bitmap.FromStream(Texture.ToStream(tex, ImageFileFormat.Png));
        }
    }

    public class DDSHeader
    {
        public int flags;
        public int height;
        public int width;
        public int mipmapcount;
        public int pflags;
        public string FourCC;
        public int bpp;
        public int rmask;
        public int gmask;
        public int bmask;
        public int amask;

        public DDSHeader() { }

        public bool Read(byte[] data, out int pos)
        {
            pos = 0;
            try
            {
                if (Utilities.GetString(data, ref pos, 4) != "DDS ")
                    return false;
                // sizeof(DDSURFACEDESC2)
                if (Utilities.GetInt(data, ref pos) != 124)
                    return false;
                flags = Utilities.GetInt(data, ref pos);
                height = Utilities.GetInt(data, ref pos);
                width = Utilities.GetInt(data, ref pos);
                pos += 4; // skip linear size
                pos += 4; // skip depth
                mipmapcount = Utilities.GetInt(data, ref pos);
                if (mipmapcount == 0)
                    mipmapcount = 1;
                pos += 11 * 4; // skip reserved
                // sizeof(DDPIXELFORMAT)
                if (Utilities.GetInt(data, ref pos) != 32)
                    return false;
                pflags = Utilities.GetInt(data, ref pos);
                FourCC = Utilities.GetString(data, ref pos, 4);
                bpp = Utilities.GetInt(data, ref pos);
                rmask = Utilities.GetInt(data, ref pos);
                gmask = Utilities.GetInt(data, ref pos);
                bmask = Utilities.GetInt(data, ref pos);
                amask = Utilities.GetInt(data, ref pos);
                pos += 4 * 4;   // ignore DDSCAPS2
                pos += 4;       // ignore second reserved
                return true;
            }
            catch { }
            return false;
        }
    }

    class TgaHeader
    {
        public byte Color_Map_Type;
        public byte Image_Type;
        public int  First_Entry_Index;
        public int  Color_Map_Length;
        public byte Color_Map_Entry_Size;
        public int  Image_Width;
        public int  Image_Height;
        public byte Pixel_Depth;

        public TgaHeader() { }

        public bool Read(byte[] data, out int pos)
        {
            pos = 0;
            try
            {
                int ID_Length = data[pos++];
                Color_Map_Type = data[pos++];
                // Should only be 0 (no color-map) or 1 (color-map is included).
                if (Color_Map_Type != 0 && Color_Map_Type != 1)
                    return false;
                Image_Type = data[pos++];
                First_Entry_Index = Utilities.GetWord(data, ref pos);
                Color_Map_Length = Utilities.GetWord(data, ref pos);
                Color_Map_Entry_Size = data[pos++];
                pos += 2; // ignore the X Origin
                pos += 2; // ignore the Y Origin
                Image_Width = Utilities.GetWord(data, ref pos);
                if (Image_Width == 0)
                    return false;
                Image_Height = Utilities.GetWord(data, ref pos);
                if (Image_Height == 0)
                    return false;
                Pixel_Depth = data[pos++];
                pos++; // ignore the Image Descriptor

                // Skip the Image ID.
                while (ID_Length-- != 0)
                    ++pos;

                // Verify there's enough data.
                int len = data.Length - pos;
                len -= Color_Map_Length * ((Color_Map_Entry_Size + 7) / 8);
                if (len <= 0)
                    return false;
                return (Image_Width * Image_Height * ((Pixel_Depth + 7) / 8) <= len);
            }
            catch { }
            return false;
        }
    }
}
