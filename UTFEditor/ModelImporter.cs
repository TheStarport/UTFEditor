using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ArcManagedFBX.IO;
using ArcManagedFBX;
using System.Windows.Forms;
using SharpDX;
using System.Text.RegularExpressions;

namespace UTFEditor
{
    public enum ModelImportVertexType
    {
        Normals = 0,
        VertexColors = 1,
        VertexColorsNormals = 2,
        ExtraUVs = 3,
        TangentsBinormals = 4,
        ExtraUVsTangentsBinormals = 5
    }

    public class ModelImporter
    {

        public UTFForm CMP { get; private set; }
        public UTFForm MAT { get; private set; }

        public bool Wireframe { get; private set; }
        public ModelImportVertexType VertexType { get; private set; }
        public bool Relocate { get; private set; }
        public string UniqueName { get; private set; }

        TreeView treeCMP = null, treeMAT = null;
        TreeNode rootCMP, cmpnd, vmeshlib;

        CmpRevData rev = new CmpRevData();
        CmpRevData pris = new CmpRevData();
        CmpFixData fix = new CmpFixData();

        UTFEditorMain parent = null;

        public ModelImporter(UTFEditorMain parent, string path)
        {
            this.parent = parent;

            var importForm = new ModelImporterOptions(Path.GetFileNameWithoutExtension(path));
            if (importForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            Wireframe = importForm.Wireframe;
            VertexType = importForm.VertexType;
            Relocate = importForm.Relocate;
            UniqueName = importForm.UniqueName;

            InitCMP(path);
            //InitMAT(path);

            if (Path.GetExtension(path).ToLower() == ".fbx")
                ImportFBX(path);

            CMP.Modified();
            //MAT.Modified();

            //MAT.Show();
            CMP.Show();
        }

        private void InitMAT(string path)
        {
            MAT = new UTFForm(parent, Path.ChangeExtension(path, ".mat"));
            treeMAT = MAT.Tree;
            MAT.MdiParent = parent;
        }

        private void InitCMP(string path)
        {
            CMP = new UTFForm(parent, Path.ChangeExtension(path, ".cmp"));
            treeCMP = CMP.Tree;
            CMP.MdiParent = parent;

            rootCMP = CreateNode("\\");
            treeCMP.Nodes.Add(rootCMP);

            vmeshlib = CreateNode("VMeshLibrary");
            rootCMP.Nodes.Add(vmeshlib);

            cmpnd = CreateNode("Cmpnd");
            rootCMP.Nodes.Add(cmpnd);
        }

        private TreeNode CreateNode(string name, byte[] data = null)
        {
            TreeNode node = new TreeNode(name);
            node.Name = name;
            node.Tag = data != null ? data : new byte[0];

            return node;
        }

        private void ImportFBX(string path)
        {
            FBXManager man = FBXManager.Create();
            FBXIOSettings io = FBXIOSettings.Create(man, "IOSRoot");
            man.SetIOSettings(io);

            FBXImporter fbx = FBXImporter.Create(man, "");
            fbx.Initialize(path, -1, man.GetIOSettings());

            FBXScene scene = FBXScene.Create(man, "importScene");
            fbx.Import(scene);
            fbx.Destroy();

            var root = scene.GetRootNode();
            for(int i = 0; i < root.GetChildCount(); i++)
            {
                var child = root.GetChild(i);
                
                if(child.GetName() == "Root")
                {
                    ParseNode(child, true);
                    break;
                }
            }

            List<VMeshDataFile> lst_vmesh_data = new List<VMeshDataFile>();
            VMeshDataFile cur_meshdata_file = null;

            for(int i = 0; i < cmpndData.Count; i++)
            {
                var cmpnd = cmpndData[i];

                for(uint lod = 0; lod < cmpnd.object_data.lods; lod++)
                {
                    if(cur_meshdata_file == null || cmpnd.object_data.data[lod].vmeshref.NumIndex + cur_meshdata_file.ref_vertices > 0xFFFF)
                    {
                        cur_meshdata_file = new VMeshDataFile();
                        cur_meshdata_file.filename = UniqueName + lst_vmesh_data.Count + ".vms";
                        cur_meshdata_file.ref_vertices = 0;
                        cur_meshdata_file.vertices = 0;
                        cur_meshdata_file.meshes = 0;
                        lst_vmesh_data.Add(cur_meshdata_file);
                    }

                    cur_meshdata_file.ref_vertices += cmpnd.object_data.data[lod].vmeshref.NumIndex;
                    cur_meshdata_file.vertices += cmpnd.object_data.data[lod].vmeshref.NumVert;
                    cur_meshdata_file.meshes += cmpnd.object_data.data[lod].vmeshref.NumMeshes;

                    cmpnd.object_data.data[lod].vmeshref.VMeshLibId = Utilities.FLModelCRC(cur_meshdata_file.filename);
                    cmpnd.object_data.data[lod].vmeshdata = cur_meshdata_file;
                }

                cmpnd.object_data.file_name = UniqueName + "." + cmpnd.object_name + ".3db";

                cmpndData.RemoveAt(i);
                cmpndData.Insert(i, cmpnd);
            }

            foreach(var vmesh in lst_vmesh_data)
            {
                /*VMeshData vmeshdata = new VMeshData();

                switch (VertexType)
                {
                    case ModelImportVertexType.Normals:
                        vmeshdata.FlexibleVertexFormat = (ushort)(VMeshData.D3DFVF_XYZ | VMeshData.D3DFVF_NORMAL | VMeshData.D3DFVF_TEX1);
                        break;
                    case ModelImportVertexType.VertexColors:
                        vmeshdata.FlexibleVertexFormat = (ushort)(VMeshData.D3DFVF_XYZ | VMeshData.D3DFVF_DIFFUSE | VMeshData.D3DFVF_TEX1);
                        break;
                    case ModelImportVertexType.VertexColorsNormals:
                        vmeshdata.FlexibleVertexFormat = (ushort)(VMeshData.D3DFVF_XYZ | VMeshData.D3DFVF_DIFFUSE | VMeshData.D3DFVF_NORMAL | VMeshData.D3DFVF_DIFFUSE);
                        break;
                    case ModelImportVertexType.ExtraUVs:
                        vmeshdata.FlexibleVertexFormat = (ushort)(VMeshData.D3DFVF_XYZ | VMeshData.D3DFVF_NORMAL | VMeshData.D3DFVF_TEX2);
                        break;
                    case ModelImportVertexType.TangentsBinormals:
                        vmeshdata.FlexibleVertexFormat = (ushort)(VMeshData.D3DFVF_XYZ | VMeshData.D3DFVF_NORMAL | VMeshData.D3DFVF_TEX4);
                        break;
                    case ModelImportVertexType.ExtraUVsTangentsBinormals:
                        vmeshdata.FlexibleVertexFormat = (ushort)(VMeshData.D3DFVF_XYZ | VMeshData.D3DFVF_NORMAL | VMeshData.D3DFVF_TEX5);
                        break;
                }*/

                const uint HEADER_SIZE = 2 * 4 + 4 * 2;
                const uint MESH_HEADER_SIZE = 4 + 3 * 2 + 2;
                const uint INDEX_SIZE = 2;
                uint VERTEX_SIZE = 0;
                switch (VertexType)
                {
                    case ModelImportVertexType.Normals:
                        VERTEX_SIZE = 3 * 4 + 3 * 4 + 2 * 4;
                        break;
                    case ModelImportVertexType.VertexColors:
                        VERTEX_SIZE = 3 * 4 + 1 * 4 + 2 * 4;
                        break;
                    case ModelImportVertexType.VertexColorsNormals:
                        VERTEX_SIZE = 3 * 4 + 3 * 4 + 1 * 4 + 2 * 4;
                        break;
                    case ModelImportVertexType.ExtraUVs:
                        VERTEX_SIZE = 3 * 4 + 3 * 4 + 2 * 4 + 2 * 4;
                        break;
                    case ModelImportVertexType.TangentsBinormals:
                        VERTEX_SIZE = 3 * 4 + 3 * 4 + 2 * 4 + 3 * 4 + 3 * 4;
                        break;
                    case ModelImportVertexType.ExtraUVsTangentsBinormals:
                        VERTEX_SIZE = 3 * 4 + 3 * 4 + 2 * 4 + 2 * 4 + 3 * 4 + 3 * 4;
                        break;
                }

                int meshCount = 0;
                int indicesCount = 0;
                int verticesCount = 0;
                foreach (var cmpnd in cmpndData)
                {
                    for (int lod = 0; lod < cmpnd.object_data.lods; lod++)
                    {
                        if (cmpnd.object_data.data[lod].vmeshdata == vmesh)
                        {
                            meshCount += cmpnd.object_data.data[lod].meshes.Count;
                            foreach (var m in cmpnd.object_data.data[lod].meshes)
                            {
                                indicesCount += m.t.Length * 3;
                                verticesCount += m.v.Length;
                            }
                        }
                    }
                }

                byte[] data = new byte[HEADER_SIZE + MESH_HEADER_SIZE * meshCount + INDEX_SIZE * indicesCount + VERTEX_SIZE * verticesCount];
                int pos = 0;

                // write header
                Utilities.WriteInt(data, 1, ref pos);
                Utilities.WriteInt(data, 4, ref pos);
                Utilities.WriteWord(data, (ushort)vmesh.meshes, ref pos);
                Utilities.WriteWord(data, (ushort)vmesh.ref_vertices, ref pos);

                ushort fvf = 0;
                switch(VertexType)
                {
                    case ModelImportVertexType.Normals:
                        fvf = 0x112;
                        break;
                    case ModelImportVertexType.VertexColors:
                        fvf = 0x142;
                        break;
                    case ModelImportVertexType.VertexColorsNormals:
                        fvf = 0x152;
                        break;
                    case ModelImportVertexType.ExtraUVs:
                        fvf = 0x212;
                        break;
                    case ModelImportVertexType.TangentsBinormals:
                        fvf = 0x412;
                        break;
                    case ModelImportVertexType.ExtraUVsTangentsBinormals:
                        fvf = 0x512;
                        break;
                }
                Utilities.WriteWord(data, fvf, ref pos);
                Utilities.WriteWord(data, (ushort)vmesh.vertices, ref pos);

                uint iMesh = 0;
                uint iGlobalStartVertex = 0;
                uint iGlobalStartIndex = 0;

                // save mesh header data
                foreach (var cmpnd in cmpndData)
                {
                    for(int lod = 0; lod < cmpnd.object_data.lods; lod++)
                    {
                        if(cmpnd.object_data.data[lod].vmeshdata == vmesh)
                        {
                            cmpnd.object_data.data[lod].vmeshref.StartMesh = (ushort)iMesh;
                            cmpnd.object_data.data[lod].vmeshref.StartVert = (ushort)iGlobalStartVertex;
                            cmpnd.object_data.data[lod].vmeshref.StartIndex = (ushort)iGlobalStartIndex;

                            uint iStartVert = 0;

                            foreach(var m in cmpnd.object_data.data[lod].meshes)
                            {
                                Utilities.WriteDWord(data, Utilities.FLModelCRC(m.material_name), ref pos);
                                Utilities.WriteWord(data, (ushort)iStartVert, ref pos);
                                Utilities.WriteWord(data, (ushort)(iStartVert + m.nVerts - 1), ref pos);
                                Utilities.WriteWord(data, (ushort)(m.nTris * 3), ref pos);
                                Utilities.WriteWord(data, 0xCC, ref pos);

                                iStartVert += (uint)m.nVerts;
                                iGlobalStartIndex += (uint)m.nTris * 3;
                                iGlobalStartVertex += (uint)m.nVerts;
                                iMesh++;
                            }
                        }
                    }
                }

                // save indices
                foreach (var cmpnd in cmpndData)
                {
                    for (int lod = 0; lod < cmpnd.object_data.lods; lod++)
                    {
                        if (cmpnd.object_data.data[lod].vmeshdata == vmesh)
                        {
                            foreach (var m in cmpnd.object_data.data[lod].meshes)
                            {
                                foreach (var t in m.t)
                                {
                                    Utilities.WriteWord(data, t.vertices[0], ref pos);
                                    Utilities.WriteWord(data, t.vertices[1], ref pos);
                                    Utilities.WriteWord(data, t.vertices[2], ref pos);
                                }
                            }
                        }
                    }
                }

                // save vertices
                foreach (var cmpnd in cmpndData)
                {
                    for (int lod = 0; lod < cmpnd.object_data.lods; lod++)
                    {
                        if (cmpnd.object_data.data[lod].vmeshdata == vmesh)
                        {
                            foreach (var m in cmpnd.object_data.data[lod].meshes)
                            {
                                foreach (var v in m.v)
                                {
                                    Utilities.WriteFloat(data, v.vert.X, ref pos);
                                    Utilities.WriteFloat(data, v.vert.Y, ref pos);
                                    Utilities.WriteFloat(data, v.vert.Z, ref pos);

                                    if (VertexType != ModelImportVertexType.VertexColors)
                                    {
                                        Utilities.WriteFloat(data, v.normal.X, ref pos);
                                        Utilities.WriteFloat(data, v.normal.Y, ref pos);
                                        Utilities.WriteFloat(data, v.normal.Z, ref pos);
                                    }

                                    if (VertexType == ModelImportVertexType.VertexColors || VertexType == ModelImportVertexType.VertexColorsNormals)
                                        Utilities.WriteDWord(data, v.diffuse, ref pos);

                                    Utilities.WriteFloat(data, v.uv.X, ref pos);
                                    Utilities.WriteFloat(data, v.uv.Y, ref pos);

                                    if(VertexType == ModelImportVertexType.ExtraUVs || VertexType == ModelImportVertexType.ExtraUVsTangentsBinormals)
                                    {
                                        Utilities.WriteFloat(data, v.uv2.X, ref pos);
                                        Utilities.WriteFloat(data, v.uv2.Y, ref pos);
                                    }

                                    if(VertexType == ModelImportVertexType.ExtraUVsTangentsBinormals || VertexType == ModelImportVertexType.TangentsBinormals)
                                    {
                                        Utilities.WriteFloat(data, v.tangent.X, ref pos);
                                        Utilities.WriteFloat(data, v.tangent.Y, ref pos);
                                        Utilities.WriteFloat(data, v.tangent.Z, ref pos);

                                        Utilities.WriteFloat(data, v.binormal.X, ref pos);
                                        Utilities.WriteFloat(data, v.binormal.Y, ref pos);
                                        Utilities.WriteFloat(data, v.binormal.Z, ref pos);
                                    }
                                }
                            }
                        }
                    }
                }
                
                TreeNode vmeshnode = CreateNode(vmesh.filename);
                vmeshlib.Nodes.Add(vmeshnode);

                TreeNode vmeshdatanode = CreateNode("VMeshData", data);
                vmeshnode.Nodes.Add(vmeshdatanode);
            }

            uint iTotalVWireIndices = 0;
            if (Wireframe)
            {

            }

            TreeNode consnode = CreateNode("Cons");
            cmpnd.Nodes.Add(consnode);

            if (rev.Parts.Count > 0)
            {
                TreeNode revnode = CreateNode("Rev", rev.GetBytes());
                consnode.Nodes.Add(revnode);
            }

            if (pris.Parts.Count > 0)
            {
                TreeNode prisnode = CreateNode("Pris", pris.GetBytes());
                consnode.Nodes.Add(prisnode);
            }

            if (fix.Parts.Count > 0)
            {
                TreeNode fixnode = CreateNode("Fix", fix.GetBytes());
                consnode.Nodes.Add(fixnode);
            }

            foreach(var cmpnd in cmpndData)
            {
                // Cmpnd child node
                {
                    TreeNode cmpndnode = CreateNode(cmpnd.name);
                    this.cmpnd.Nodes.Add(cmpndnode);

                    TreeNode file_name = CreateNode("File name", Encoding.ASCII.GetBytes(cmpnd.object_data.file_name + "\u0000"));
                    TreeNode index = CreateNode("Index", BitConverter.GetBytes(cmpnd.index));
                    TreeNode object_name = CreateNode("Object name", Encoding.ASCII.GetBytes(cmpnd.object_name + "\u0000"));

                    cmpndnode.Nodes.Add(file_name);
                    cmpndnode.Nodes.Add(index);
                    cmpndnode.Nodes.Add(object_name);
                }

                // Root child node
                {
                    TreeNode threedbnode = CreateNode(cmpnd.object_data.file_name);
                    rootCMP.Nodes.Add(threedbnode);

                    if(Wireframe)
                    {

                    }

                    TreeNode multilevel = CreateNode("MultiLevel");
                    threedbnode.Nodes.Add(multilevel);

                    if(cmpnd.object_data.lods > 1)
                    {
                        byte[] data = new byte[4 * cmpnd.object_data.lods];
                        int pos = 0;
                        Utilities.WriteFloat(data, 0.0f, ref pos);

                        for (int lod = 1; lod < cmpnd.object_data.lods; lod++)
                            Utilities.WriteFloat(data, (float)Math.Pow(10.0f, (float)(lod + 1)), ref pos);
                        Utilities.WriteFloat(data, 1000000.0f, ref pos);

                        TreeNode switch2 = CreateNode("Switch2", data);
                        multilevel.Nodes.Add(switch2);
                    }

                    for(int lod = 0; lod < cmpnd.object_data.lods; lod++)
                    {
                        TreeNode levelnode = CreateNode("Level" + lod);
                        multilevel.Nodes.Add(levelnode);

                        TreeNode vmeshpart = CreateNode("VMeshPart");
                        levelnode.Nodes.Add(vmeshpart);

                        TreeNode vmeshref = CreateNode("VMeshRef", cmpnd.object_data.data[lod].vmeshref.GetBytes());
                        vmeshpart.Nodes.Add(vmeshref);
                    }
                }
            }

        }

        private void ParseNode(FBXNode n, bool traverse)
        {
            CreateCMPData(n);

            if (!traverse)
                return;

            for (int i = 0; i < n.GetChildCount(); i++)
            {
                var child = n.GetChild(i);
                ParseNode(child, false);
            }
        }

        List<CmpndData> cmpndData = new List<CmpndData>();

        struct CmpndData
        {
            public string name;
            public string object_name;
            public ThreeDBData object_data;
            public int index;
        }

        struct ThreeDBData
        {
            public string file_name;
            public uint lods;
            public uint wireframe_lod;
            public LodData[] data;
        }

        struct LodData
        {
            public VMeshDataFile vmeshdata;
            public VMeshRef vmeshref;
            public List<SMesh> meshes;
            public bool is_wireframe;
        }

        class VMeshDataFile
        {
            public string filename;
            public uint ref_vertices;
            public uint vertices;
            public uint meshes;
        }

        struct SMesh
        {
            public VMSVert[] v;
            public VMSTri[] t;
            public int nVerts;
            public int nTris;

            public string material_name;
            public string name;
        }

        struct VMSVert
        {
            public Vector3 vert;
            public Vector3 normal;
            public uint diffuse;
            public Vector2 uv;
            public Vector2 uv2;
            public Vector3 tangent;
            public Vector3 binormal;
        }

        struct VMSTri
        {
            public ushort[] vertices;
        }

        private Vector3 T(FBXVector v)
        {
            return new Vector3((float)v.x, (float)v.y, (float)v.z);
        }

        private Vector2 T2(FBXVector v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        private Vector4 T4(FBXVector v)
        {
            return new Vector4((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        }

        private Matrix T(FBXMatrix m)
        {
            Matrix m2 = new Matrix(
                (float)m.mData[0].x, (float)m.mData[0].y, (float)m.mData[0].z, (float)m.mData[0].w,
                (float)m.mData[1].x, (float)m.mData[1].y, (float)m.mData[1].z, (float)m.mData[1].w,
                (float)m.mData[2].x, (float)m.mData[2].y, (float)m.mData[2].z, (float)m.mData[2].w,
                (float)m.mData[3].x, (float)m.mData[3].y, (float)m.mData[3].z, (float)m.mData[3].w
                );
            return m2;
        }

        private void CreateCMPData(FBXNode n)
        {
            Vector3 vOffset = Vector3.Zero;

            CmpndData cmpnd = new CmpndData();
            cmpnd.index = 0;
            cmpnd.object_name = n.GetName();
            if (cmpnd.object_name == "Root")
                cmpnd.name = "Root";
            else
            {
                cmpnd.name = "Part_" + cmpnd.object_name;

                if(Relocate)
                {
                    vOffset = T(n.EvaluateLocalTranslation(FBXTime.Infinite(), ArcManagedFBX.Types.EPivotSet.eSourcePivot, false, false));
                }
            }

            cmpnd.object_data = new ThreeDBData();
            cmpnd.object_data.data = new LodData[8];

            cmpnd.object_data.wireframe_lod = 0;

            int lods = 0;

            for(int i = 0; i < n.GetChildCount(); i++)
            {
                var cur_lodnode = n.GetChild(i);

                var match = Regex.Match(cur_lodnode.GetName(), @"^.+_lod(?<lod>[0-9])(?<wireframe>_vwd)?");
                if (match.Success)
                {
                    uint lod = uint.Parse(match.Groups["lod"].Value);
                    if (match.Groups["wireframe"].Success)
                        cmpnd.object_data.wireframe_lod = lod;

                    cmpnd.object_data.data[lod].meshes = new List<SMesh>();

                    lods++;

                    int numVerts = 0;
                    int numFaces = 0;

                    Vector3 bbmin = new Vector3(float.MaxValue);
                    Vector3 bbmax = new Vector3(float.MinValue);

                    for (int j = 0; j < cur_lodnode.GetChildCount(); j++)
                    {
                        var cur_node = cur_lodnode.GetChild(j);

                        var attr = cur_node.GetNodeAttribute();
                        if (attr == null)
                            continue;

                        if (attr.GetAttributeType() != ArcManagedFBX.Types.EAttributeType.eMesh)
                            continue;

                        var pmesh = attr as FBXMesh;

                        var cur_node_trans = T(cur_node.EvaluateGlobalTransform(FBXTime.Infinite(), ArcManagedFBX.Types.EPivotSet.eSourcePivot, false, false));
                        var cur_node_geo = T(cur_node.GetGeometricMatrix(ArcManagedFBX.Types.EPivotSet.eSourcePivot));
                        cur_node_trans = cur_node_geo * cur_node_trans;

                        SMesh mesh = new SMesh();

                        int nTris = pmesh.GetPolygonCount();
                        if (nTris <= 0)
                            continue;

                        if (cur_node.GetMaterialCount() <= 0)
                            mesh.material_name = "default";
                        else
                        {
                            var material = cur_node.GetMaterial(0);
                            mesh.material_name = material.GetName();
                        }

                        mesh.name = cur_node.GetName();

                        mesh.t = new VMSTri[nTris];
                        int nVerts = nTris * 3;
                        mesh.v = new VMSVert[nVerts];

                        var vertices = pmesh.GetControlPoints();
                        int tangentLayerCount = pmesh.GetElementTangentCount();
                        var tangentLayer = tangentLayerCount > 0 ? pmesh.GetElementTangent().GetDirectArray() : null;
                        int binormalLayerCount = pmesh.GetElementBinormalCount();
                        var binormalLayer = binormalLayerCount > 0 ? pmesh.GetElementBinormal().GetDirectArray() : null;

                        for (int nTri = 0; nTri < nTris; nTri++)
                        {
                            mesh.t[nTri].vertices = new ushort[3];

                            for(int k = 0; k < 3; k++)
                            {
                                int nVert = (nTri * 3 + k);

                                Vector3 vertice, normal;
                                Vector2 uv;
                                bool unmapped = false;
                                FBXVector fbxuv = new FBXVector();
                                FBXVector fbxnormal = new FBXVector();

                                vertice = Vector3.TransformCoordinate(T(vertices[pmesh.GetPolygonVertex(nTri, k)]), cur_node_trans);
                                
                                // offset vertice
                                vertice -= vOffset;
                                pmesh.GetPolygonVertexUV(nTri, k, "map1", ref fbxuv, ref unmapped);
                                uv = T2(fbxuv);
                                pmesh.GetPolygonVertexNormal(nTri, k, ref fbxnormal);
                                normal = Vector3.TransformNormal(T(fbxnormal), cur_node_trans);

                                mesh.t[nTri].vertices[k] = (ushort)nVert;

                                mesh.v[nVert].vert = vertice;
                                mesh.v[nVert].normal = normal;
                                mesh.v[nVert].uv = uv;

                                mesh.v[nVert].tangent = tangentLayer != null ? T(tangentLayer.GetAt(nVert)) : Vector3.Zero;
                                mesh.v[nVert].binormal = binormalLayer != null ? T(binormalLayer.GetAt(nVert)) : Vector3.Zero;
                                /*
                               
                                float alpha = 1.0f;
                                int iVCindex = mesh->pMesh->GetFaceVertex(pTriangle->meshFaceIndex, i);
                                if (iVCindex != -1)
                                {
                                    alpha = mesh->pMesh->GetAlphaVertex(pTriangle->alpha[i]);
                                    color = mesh->pMesh->GetColorVertex(pTriangle->color[i]);
                                    mesh->v[nVert].diffuse = (DWORD)(alpha * 255) << 24 | (DWORD)(color.x * 255) << 16 | (DWORD)(color.y * 255) << 8 | (DWORD)(color.z * 255);
                                    mesh->v[nVert].uv = uv;
                                }
                                else
                                {
                                    mesh->v[nVert].tangent = Point3(0, 0, 0);
                                    mesh->v[nVert].binormal = Point3(0, 0, 0);
                                }*/

                                bbmin = Vector3.Min(bbmin, vertice);
                                bbmax = Vector3.Max(bbmin, vertice);
                            }
                        }

                        mesh.nVerts = nVerts;
                        numVerts += mesh.nVerts;

                        mesh.nTris = nTris;
                        numFaces += mesh.nTris;

                        cmpnd.object_data.data[lod].meshes.Add(mesh);
                    }

                    cmpnd.object_data.data[lod].vmeshref = new VMeshRef();
                    cmpnd.object_data.data[lod].vmeshref.BoundingBoxMaxX = bbmax.X;
                    cmpnd.object_data.data[lod].vmeshref.BoundingBoxMaxY = bbmax.Y;
                    cmpnd.object_data.data[lod].vmeshref.BoundingBoxMaxZ = bbmax.Z;
                    cmpnd.object_data.data[lod].vmeshref.BoundingBoxMinX = bbmin.X;
                    cmpnd.object_data.data[lod].vmeshref.BoundingBoxMinY = bbmin.Y;
                    cmpnd.object_data.data[lod].vmeshref.BoundingBoxMinZ = bbmin.Z;
                    var vCenter = (bbmax + bbmin) / 2;
                    cmpnd.object_data.data[lod].vmeshref.CenterX = vCenter.X;
                    cmpnd.object_data.data[lod].vmeshref.CenterY = vCenter.Y;
                    cmpnd.object_data.data[lod].vmeshref.CenterZ = vCenter.Z;
                    cmpnd.object_data.data[lod].vmeshref.Radius = (bbmax - vCenter).Length() * 1.25f;

                    cmpnd.object_data.data[lod].vmeshref.HeaderSize = 60;
                    cmpnd.object_data.data[lod].vmeshref.NumMeshes = (ushort)cmpnd.object_data.data[lod].meshes.Count;
                    cmpnd.object_data.data[lod].vmeshref.NumVert = (ushort)numVerts;
                    cmpnd.object_data.data[lod].vmeshref.NumIndex = (ushort)(numFaces * 3);

                    if (numFaces * 3 > 0xFFFF)
                        throw new ArgumentException($"{cmpnd.object_name} references more than 65535 vertices. Split the group into smaller groups!");
                }
            }

            cmpnd.object_data.lods = (uint)lods;
            cmpnd.index = cmpndData.Count;
            cmpndData.Add(cmpnd);

            if(cmpnd.object_name != "Root")
            {
                if(cmpnd.object_name.EndsWith("_rev"))
                {
                    CmpRevData.Part revdata = new CmpRevData.Part();
                    revdata.ParentName = "Root";
                    revdata.ChildName = cmpnd.object_name;

                    revdata.OriginX = vOffset.X;
                    revdata.OriginY = vOffset.Y;
                    revdata.OriginZ = vOffset.Z;

                    var mat = n.EvaluateLocalTransform(FBXTime.Infinite(), ArcManagedFBX.Types.EPivotSet.eSourcePivot, false, false);
                    revdata.RotMatXX = (float)mat.mData[0].x;
                    revdata.RotMatXY = (float)mat.mData[1].x;
                    revdata.RotMatXZ = (float)mat.mData[2].x;
                    revdata.RotMatYX = (float)mat.mData[0].y;
                    revdata.RotMatYY = (float)mat.mData[1].y;
                    revdata.RotMatYZ = (float)mat.mData[2].y;
                    revdata.RotMatZX = (float)mat.mData[0].z;
                    revdata.RotMatZY = (float)mat.mData[1].z;
                    revdata.RotMatZZ = (float)mat.mData[2].z;

                    revdata.AxisRotX = (float)mat.mData[0].x;
                    revdata.AxisRotY = (float)mat.mData[1].x;
                    revdata.AxisRotZ = (float)mat.mData[2].x;
                    revdata.Max = 1;

                    rev.Parts.Add(revdata);
                }
                else if(cmpnd.object_name.EndsWith("_pris"))
                {
                    CmpRevData.Part revdata = new CmpRevData.Part();
                    revdata.ParentName = "Root";
                    revdata.ChildName = cmpnd.object_name;

                    revdata.OriginX = vOffset.X;
                    revdata.OriginY = vOffset.Y;
                    revdata.OriginZ = vOffset.Z;

                    revdata.RotMatXX = revdata.RotMatYY = revdata.RotMatZZ = 1;

                    revdata.AxisRotZ = 1;
                    revdata.Max = 360;

                    pris.Parts.Add(revdata);
                }
                else
                {
                    CmpFixData.Part fixdata = new CmpFixData.Part();
                    fixdata.ParentName = "Root";
                    fixdata.ChildName = cmpnd.object_name;

                    fixdata.OriginX = vOffset.X;
                    fixdata.OriginY = vOffset.Y;
                    fixdata.OriginZ = vOffset.Z;

                    fixdata.RotMatXX = fixdata.RotMatYY = fixdata.RotMatZZ = 1;

                    fix.Parts.Add(fixdata);
                }
            }
        }
    }
}
