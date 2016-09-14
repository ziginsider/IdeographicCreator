using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IdeographicCreator
{
    public partial class FormSetLabels : Form
    {
        public Font TreeFont { get; set; }
        public Color TreeColor { get; set; }
        public string selectNodeName { get; set; }


        private List<string> listCheckedTopics;
        private List<string> listTopicLabels = new List<string>();
        public FormSetLabels()
        {
            InitializeComponent();
        }

        private void btnSetLabels_Click(object sender, EventArgs e)
        {
            Ostarbeiter ost = new Ostarbeiter();
            listCheckedTopics.Clear();
            CallRecursive(treeViewSetLabels);
            ost.SetTopicLabels(Properties.Settings.Default.PathFile, selectNodeName, listCheckedTopics);
            this.Close();
        }

        private void FormSetLabels_Load(object sender, EventArgs e)
        {
            listCheckedTopics = new List<string>();
            DrawAllTree();
        }

        private void DrawAllTree()
        {
            TreeNode root;
            

            Ostarbeiter ost = new Ostarbeiter();

            if (treeViewSetLabels.GetNodeCount(true) == 0)
            {
                root = ost.GetTreeNodeTopicsAll(Properties.Settings.Default.PathFile);
                treeViewSetLabels.Nodes.Add(root);
                //treeViewSetLabels.ExpandAll();
                treeViewSetLabels.Nodes[0].Expand();

                // SelectNode = root;
            }

            treeViewSetLabels.CheckBoxes = true;
            treeViewSetLabels.Font = TreeFont;
            treeViewSetLabels.ForeColor = TreeColor;

            listTopicLabels = ost.GetTopicLabels(Properties.Settings.Default.PathFile, selectNodeName);

            //отмечаем чекбоксы с уже существующими ссылками у текущей темы
            CallRecursive2(treeViewSetLabels);

        }


        private void TreeChekedShowRecursive(TreeNode treeNode)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                TreeChekedShowRecursive(tn);

                // то, что отмечено флажком, но не корневой и не текущий узел
                if (tn.Checked == true && tn.Name != "0" && tn.Name != selectNodeName)
                {
                    //MessageBox.Show(tn.Text);
                    listCheckedTopics.Add(tn.Name);
                }
            }
        }

        private void CallRecursive(TreeView treeView)
        {
            // Print each node recursively.
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                TreeChekedShowRecursive(n);
            }
        }


        private void TreeChekedShowRecursive2(TreeNode treeNode)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                TreeChekedShowRecursive2(tn);

                foreach (string ll in listTopicLabels)
                {
                    if (tn.Name == ll)
                    {
                        tn.Checked = true;
                    }
                }
            }
        }

        private void CallRecursive2(TreeView treeView)
        {
            // Print each node recursively.
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                TreeChekedShowRecursive2(n);
            }
        }
    }
}
