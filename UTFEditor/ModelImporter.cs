using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ArcManagedFBX.IO;
using ArcManagedFBX;
using System.Windows.Forms;

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

        TreeView treeCMP = null, treeMAT = null;
        TreeNode rootCMP, cmpnd, vmeshlib;

        UTFEditorMain parent = null;

        public ModelImporter(UTFEditorMain parent, string path)
        {
            this.parent = parent;

            var importForm = new ModelImporterOptions();
            if (importForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            Wireframe = importForm.Wireframe;
            VertexType = importForm.VertexType;

            InitCMP(path);
            InitMAT(path);

            if (Path.GetExtension(path).ToLower() == ".fbx")
                ImportFBX(path);

            MAT.Show();
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

        private void CreateCmpnd(string group_name, string mesh_name, int i)
        {
            var part = CreateNode("Part_" + group_name);

            var filename = CreateNode("File name", Encoding.ASCII.GetBytes(mesh_name));
            var index = CreateNode("Index", BitConverter.GetBytes(i));
            var objectname = CreateNode("Object name", Encoding.ASCII.GetBytes(group_name));

            part.Nodes.Add(filename);
            part.Nodes.Add(index);
            part.Nodes.Add(objectname);

            cmpnd.Nodes.Add(part);
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
                ParseNode(child);
            }
        }

        private void ParseNode(FBXNode n)
        {
            var attr = n.GetNodeAttribute();
            if(attr != null)
            {
                var type = attr.GetAttributeType();
                switch(type)
                {
                    case ArcManagedFBX.Types.EAttributeType.eMesh:
                        ParseMesh(n);
                        break;
                }
            }

            for (int i = 0; i < n.GetChildCount(); i++)
            {
                var child = n.GetChild(i);
                ParseNode(child);
            }
        }

        private void ParseMesh(FBXNode n)
        {
            FBXMesh m = n.GetNodeAttribute() as FBXMesh;

            var vertices = m.GetControlPoints();

            for (int i = 0; i < m.GetPolygonCount(); i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    var vid = m.GetPolygonVertex(i, j);
                    var v = vertices[vid];
                    System.Diagnostics.Debug.WriteLine($"vertex {v.x}, {v.y}, {v.z}");
                }
            }
        }
    }
}
