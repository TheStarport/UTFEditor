using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTFEditor
{
    class CmpFixData
    {
        public class Part
        {
            public string ParentName;
            public string ChildName;
            public float OriginX;
            public float OriginY;
            public float OriginZ;
            public float RotMatXX;
            public float RotMatXY;
            public float RotMatXZ;
            public float RotMatYX;
            public float RotMatYY;
            public float RotMatYZ;
            public float RotMatZX;
            public float RotMatZY;
            public float RotMatZZ;
        };

        /// <summary>
        /// The list of parts in the fix data.
        /// </summary>
        public List<Part> Parts = new List<Part>();

        public CmpFixData()
        {

        }

        /// <summary>
        /// Decode a fix node. Throw an exception if this fails.
        /// </summary>
        public CmpFixData(byte[] data)
        {
            int pos = 0;
            int num_parts = data.Length / 0xb0;
            for (int count = 0; count < num_parts; count++)
            {
                Part part = new Part();

                part.ParentName = Utilities.GetString(data, ref pos, 0x40);
                part.ChildName  = Utilities.GetString(data, ref pos, 0x40);

                part.OriginX    = Utilities.GetFloat(data, ref pos);
                part.OriginY    = Utilities.GetFloat(data, ref pos);
                part.OriginZ    = Utilities.GetFloat(data, ref pos);

                part.RotMatXX   = Utilities.GetFloat(data, ref pos);
                part.RotMatXY   = Utilities.GetFloat(data, ref pos);
                part.RotMatXZ   = Utilities.GetFloat(data, ref pos);

                part.RotMatYX   = Utilities.GetFloat(data, ref pos);
                part.RotMatYY   = Utilities.GetFloat(data, ref pos);
                part.RotMatYZ   = Utilities.GetFloat(data, ref pos);

                part.RotMatZX   = Utilities.GetFloat(data, ref pos);
                part.RotMatZY   = Utilities.GetFloat(data, ref pos);
                part.RotMatZZ   = Utilities.GetFloat(data, ref pos);

                Parts.Add(part);
            }
        }

        /// <summary>
        /// Return a byte array containing the fix data.
        /// </summary>
        public byte[] GetBytes()
        {
            List<byte> data = new List<byte>();
            foreach (Part part in Parts)
            {
                if (part.ParentName.Length > 0x3F)
                    part.ParentName = part.ParentName.Substring(0, 0x3F);
                data.AddRange(ASCIIEncoding.ASCII.GetBytes(part.ParentName));
                for (int i = 0; i < 0x40 - part.ParentName.Length; i++)
                    data.Add(0);

                if (part.ChildName.Length > 0x3F)
                    part.ChildName = part.ChildName.Substring(0, 0x3F);
                        data.AddRange(ASCIIEncoding.ASCII.GetBytes(part.ChildName));
                for (int i = 0; i < 0x40 - part.ChildName.Length; i++)
                            data.Add(0);
                
                data.AddRange(BitConverter.GetBytes(part.OriginX));
                data.AddRange(BitConverter.GetBytes(part.OriginY));
                data.AddRange(BitConverter.GetBytes(part.OriginZ));

                data.AddRange(BitConverter.GetBytes(part.RotMatXX));
                data.AddRange(BitConverter.GetBytes(part.RotMatXY));
                data.AddRange(BitConverter.GetBytes(part.RotMatXZ));

                data.AddRange(BitConverter.GetBytes(part.RotMatYX));
                data.AddRange(BitConverter.GetBytes(part.RotMatYY));
                data.AddRange(BitConverter.GetBytes(part.RotMatYZ));

                data.AddRange(BitConverter.GetBytes(part.RotMatZX));
                data.AddRange(BitConverter.GetBytes(part.RotMatZY));
                data.AddRange(BitConverter.GetBytes(part.RotMatZZ));
            }

            return data.ToArray();
        }
    }
}
