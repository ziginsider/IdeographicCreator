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
    public partial class FormSetTopic : Form
    {
        //выбранный по двойному щелчку узел
        public TreeNode SelectNode { get; set; }

        public TreeView Tree { get; set; }


        public FormSetTopic()
        {
            InitializeComponent();
        }

        /// <summary>
        /// добавление нового узла (подтемы)
        /// </summary>
        /// <param name="nodeName">ключ узла</param>
        /// <param name="nodeValue">текст узла</param>
        private void AddNode(string nodeName, string nodeValue)
        {
            SelectNode.Nodes.Add(nodeName, nodeValue);
            SelectNode.Expand();
            
        }

        private void buttonSetTopic_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxSetTopic.Text))
            {
                int id = 0;
                string nodeValue = textBoxSetTopic.Text;
                Ostarbeiter ost = new Ostarbeiter();
                
                //добавляем тему
                id = ost.SetSingleTopic(Properties.Settings.Default.PathFile, textBoxSetTopic.Text, SelectNode.Name);

                //рисуем узел
                AddNode(id.ToString(), nodeValue);

                this.Close();
            }
            else
            {
                MessageBox.Show("Введите название подтемы!", "Название подтемы не введено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void FormSetTopic_Load(object sender, EventArgs e)
        {

            this.Text = SelectNode.Text;
            textBoxChangeTopic.Text = SelectNode.Text;
            
        }

        private void buttonChangeTopic_Click(object sender, EventArgs e)
        {
            string newText = textBoxChangeTopic.Text;
            Ostarbeiter ost = new Ostarbeiter();
            //меняем название темы в базе данных
            ost.UpdateSingleTopicWithId(Properties.Settings.Default.PathFile, SelectNode.Name, newText);
            //меняем название темы в TreeView
            SelectNode.Text = newText;
            this.Close();
        }

        private void buttonDeleteTopic_Click(object sender, EventArgs e)
        {

            DialogResult r = MessageBox.Show("Удалить тему?\n" + SelectNode.Text, "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {

                Ostarbeiter ost = new Ostarbeiter();
                //удаляем рекурсивно дочерние узлы
                //DeleteTopicRecursive(SelectNode);

                foreach (TreeNode tn in SelectNode.Nodes)
                {
                    ost.UpdatePerentIdTopicWithId(Properties.Settings.Default.PathFile, tn.Name, SelectNode.Parent.Name);
                }

                //RemoveSelectedNodeRecursive(SelectNode);


                ost.UpdateParentIdListExpressionsWithParentId(Properties.Settings.Default.PathFile, SelectNode.Name, SelectNode.Parent.Name);
                ost.DeleteSingleTopic(Properties.Settings.Default.PathFile, SelectNode.Name);

                

                SelectNode.Remove();

                

            }
            this.Close();
                       
        }

        private void DeleteTopicRecursive(TreeNode treeNode)
        {

            foreach (TreeNode tn in treeNode.Nodes)
            {
                DeleteTopicRecursive(tn);
                Ostarbeiter ost = new Ostarbeiter();
                ost.DeleteSingleTopic(Properties.Settings.Default.PathFile, tn.Name);
            }
        }
    }
}
