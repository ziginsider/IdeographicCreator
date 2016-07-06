using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace IdeographicCreator
{
    public partial class FormMainCreator : Form
    {
        public TreeNode SelectNode { get; set; }
        public TreeNode TempSelectNode { get; set; }

        //public float FontCount { get; set; }

        public TreeNode root { get; set; }

        public Point point = new Point();

        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private Rectangle dragBoxFromMouseDown;

        private int flagTopic { get; set; }

        public List<string> listCheckedTopics;

        //содержит пару ключ-значение. Ключ: идентификатор темы, значение: индекс текущего выражения в теме
        Dictionary<string, int> dictCurrentExp = new Dictionary<string, int>();

        //private Point screenOffset;
        public FormMainCreator()
        {
            InitializeComponent();
        }


        private void toolStripButtonTreeView_Click(object sender, EventArgs e)
        {
            //DrawAllTree();
            treeViewCreator.ExpandAll();
        }

        private void toolStripButtonTreeViewFalse_Click(object sender, EventArgs e)
        {
            treeViewCreator.CollapseAll();
        }

        private void FormMainCreator_Load(object sender, EventArgs e)
        {
            

         
            if (String.IsNullOrEmpty(Properties.Settings.Default.PathFileOpen) || !System.IO.File.Exists(Properties.Settings.Default.PathFileOpen))
            {
                //имя открытого файла в статус-строке
                toolStripStatusLabelFileOpen.Text = Properties.Settings.Default.PathFile;
                toolStripStatusLabelFileOpen.ForeColor = Color.DarkCyan;

                this.Text = "Ideographic Creator " + Application.ProductVersion + " File open: " + Properties.Settings.Default.PathFile;
            }
            else
            {
                string dbName = Properties.Settings.Default.PathFile;
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialogMain.OpenFile();
                //MessageBox.Show(dbPath, "имя источника");
                //MessageBox.Show(name, "имя файла назначения");
                File.Delete(dbPath);
                File.Copy(Properties.Settings.Default.PathFileOpen, dbPath);

                //имя открытого файла в статус-строке
                string a = System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.PathFileOpen);
                toolStripStatusLabelFileOpen.Text = a;
                toolStripStatusLabelFileOpen.ForeColor = Color.DarkCyan;

                this.Text = "Ideographic Creator v1.5.8  File open: " + Properties.Settings.Default.PathFileOpen;
            }

            //FontCount = 7.8F;
            DrawAllTree();
            root.Expand();

            SelectNode = root;
            toolStripStatusLabel2.Text = SelectNode.Text;
            toolStripStatusLabel2.ForeColor = Color.DarkGreen;
            Ostarbeiter ost = new Ostarbeiter();
            DataTable dt = ost.GetDataTableFromDB(Properties.Settings.Default.PathFile);
            dataGridViewExpressionsWork.DataSource = dt;

            //виден ли заголовок
            //

            dataGridViewExpressionsWork.Columns[0].Visible = false;
            dataGridViewExpressionsWork.Columns[2].Visible =false;
            dataGridViewExpressionsWork.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridViewExpressionsWork.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //чётные
            dataGridViewExpressionsWork.RowsDefaultCellStyle.BackColor = Color.White;
            //нечётные
            dataGridViewExpressionsWork.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridViewExpressionsWork.CurrentCell = dataGridViewExpressionsWork[1, 0];

            //устанавливаем шрифты
            treeViewCreator.Font = Properties.Settings.Default.FontTree;
            dataGridViewExpressionsWork.Font = Properties.Settings.Default.FontExpressions;
            treeViewCreator.ForeColor = Properties.Settings.Default.ColorTree;
            dataGridViewExpressionsWork.ForeColor = Properties.Settings.Default.ColorExpressions;

            //выводим количество выражений
            labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();

            flagTopic = 0;
            listCheckedTopics = new List<string>();

            //заполняем tag дерева тем, там будем хранить индекс строки списка выражений, чтобы начинать с поледнего всегда
            dictCurrentExp.Add(treeViewCreator.Nodes[0].Name, 0);
            CallRecursiveSetTag(treeViewCreator, 0);


            //MessageBox.Show(treeViewCreator.Nodes[0].Text, treeViewCreator.Nodes[0].Tag.ToString());
            //MessageBox.Show(dictCurrentExp[treeViewCreator.Nodes[0].Name].ToString());



        }

        private void TreeSetTag(TreeNode treeNode, int index)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                TreeSetTag(tn, index);
                //tn.Tag = index;
                dictCurrentExp.Add(tn.Name, index);

            }
        }

        private void CallRecursiveSetTag(TreeView treeView, int index)
        {
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                TreeSetTag(n, index);
            }
        }

        private void treeViewCreator_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int index = SelectNode.Parent.Index;
            string idNodeParent = SelectNode.Parent.Name;

            using (FormSetTopic formSetTopic = new FormSetTopic())
            {
                formSetTopic.SelectNode = e.Node;
                //formSetTopic.Tree = treeViewCreator;
                formSetTopic.ShowDialog();
            }

            treeViewCreator.Nodes.Clear();
            DrawAllTree();
            root.Expand();

            //родительский узел делаем текущим
            SelectedNodeRecursive(treeViewCreator.Nodes[0], idNodeParent);

            //ShowAllExp();

            toolStripStatusLabel2.Text = SelectNode.Text;
            toolStripStatusLabel2.ForeColor = Color.DarkGreen;


        }

        /// <summary>
        /// Рисует дерево тем полностью
        /// </summary>
        private void DrawAllTree()
        {

            Ostarbeiter ost = new Ostarbeiter();

            if (treeViewCreator.GetNodeCount(true) == 0)
            {
                root = ost.GetTreeNodeTopicsAll(Properties.Settings.Default.PathFile);
                treeViewCreator.Nodes.Add(root);
            }

            if (checkBoxSortTree.Checked == true)
            {
                treeViewCreator.Sort();                             
            }

            //treeViewCreator.TreeViewNodeSorter =
        }

        private void treeViewCreator_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //сохраняем положение текущего выражения  в списке выражений для предыдущего узла
            dictCurrentExp[SelectNode.Name] = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;


            //предыдущую выбранную тему перерисовываем системным цветом
            SelectNode.BackColor = Color.White;
            SelectNode.ForeColor = treeViewCreator.ForeColor;

            TempSelectNode = SelectNode;
            SelectNode = e.Node;

            //подсвечиваем тему жёлтеньким
            SelectNode.BackColor = Color.Yellow;
            SelectNode.ForeColor = Color.Black;

    

            if (SelectNode.Name != "0") //если это не корневой узел
            {
                Ostarbeiter ost = new Ostarbeiter();
                DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                dataGridViewExpressionsWork.DataSource = dt;

                DataTable dtSearch = SearchExp();
                dataGridViewExpressionsWork.DataSource = dtSearch;


                labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();

                SetCurrentRow();

            }
            else // выводим все выражения
            {
                ShowAllExp();
            }
            //;

            toolStripStatusLabel2.Text = SelectNode.Text;
            toolStripStatusLabel2.ForeColor = Color.DarkGreen;

            //MessageBox.Show(e.Node.Tag.ToString());
            
            
            
            
        }

        private void treeViewCreator_KeyPress(object sender, KeyPressEventArgs e)
        {
            

            if (Convert.ToInt32(e.KeyChar) == 13 || Convert.ToInt32(e.KeyChar) == 32) //если нажали ввод или пробел
            {
                //сохраняем положение текущего выражения  в списке выражений для предыдущего узла
                dictCurrentExp[SelectNode.Name] = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;

                //предыдущую выбранную тему перерисовываем системным цветом
                SelectNode.BackColor = Color.White;
                SelectNode.ForeColor = treeViewCreator.ForeColor;
                
                TempSelectNode = SelectNode;
                SelectNode = treeViewCreator.SelectedNode;

                //подсвечиваем тему жёлтеньким
                SelectNode.BackColor = Color.Yellow;
                SelectNode.ForeColor = Color.Black;

                Ostarbeiter ost = new Ostarbeiter();
                DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                dataGridViewExpressionsWork.DataSource = dt;

                DataTable dtSearch = SearchExp();
                dataGridViewExpressionsWork.DataSource = dtSearch;

                labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();
                //;

                SetCurrentRow();

                toolStripStatusLabel2.Text = SelectNode.Text;
                toolStripStatusLabel2.ForeColor = Color.DarkGreen;
            }

        }



        /// <summary>
        /// После того как что-то редактировали в ячейке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewExpressionsWork_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Ostarbeiter ost = new Ostarbeiter();
            string ExpNewText = dataGridViewExpressionsWork.Rows[e.RowIndex].Cells[1].Value.ToString();
            string idText = dataGridViewExpressionsWork.Rows[e.RowIndex].Cells[0].Value.ToString();
            //string str3 = dataGridViewExpressionsWork.Rows[e.RowIndex].Cells[2].Value.ToString();
            // MessageBox.Show(str1, "Text");
            // MessageBox.Show(str2, "Id");
            //MessageBox.Show(str3, "Idparent");
            if (!String.IsNullOrEmpty(idText) && !String.IsNullOrEmpty(ExpNewText))
            {
                ost.UpdateSingleExpressionWithId(Properties.Settings.Default.PathFile, idText, ExpNewText);
            }
        }

        /// <summary>
        /// Два раза кликнули по содержимому ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewExpressionsWork_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            string ExpText = dataGridViewExpressionsWork.Rows[e.RowIndex].Cells[1].Value.ToString();
            string idText = dataGridViewExpressionsWork.Rows[e.RowIndex].Cells[0].Value.ToString();
            string idParentText = dataGridViewExpressionsWork.Rows[e.RowIndex].Cells[2].Value.ToString();
            //MessageBox.Show(ExpNewText, "Text");
            if (!String.IsNullOrEmpty(idText) && !String.IsNullOrEmpty(ExpText))
            {

                int indexDisplayed = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;
                int index = e.RowIndex;
                //MessageBox.Show(index.ToString());

                using (FormOptionsExpressions formOptionExp = new FormOptionsExpressions())
                {
                    formOptionExp.SelectExpText = ExpText;
                    formOptionExp.SelectExpId = idText;
                    formOptionExp.SelectExpParentId = idParentText;
                    formOptionExp.ManyExpFlag = false; //редактируем только одно выражение
                    formOptionExp.TreeFont = treeViewCreator.Font;
                    formOptionExp.TreeColor = treeViewCreator.ForeColor;
                    formOptionExp.TextFont = dataGridViewExpressionsWork.Font;
                    formOptionExp.TextColor = dataGridViewExpressionsWork.ForeColor;

                    //formSetTopic.SelectNode = e.Node;
                    formOptionExp.ShowDialog();

                }

                

                if (SelectNode.Name != "0")
                {
                    //заново рисуем таблицу выражений т.к. содержание её могло измениться
                    Ostarbeiter ost = new Ostarbeiter();
                    DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                    dataGridViewExpressionsWork.DataSource = dt;
                    //поиск
                    DataTable dtSearch = SearchExp();
                    dataGridViewExpressionsWork.DataSource = dtSearch;

                    labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();

                }
                else
                {
                    ShowAllExp();

                }
                //выделяем и показываем редактируемый узел как и было до редактирования:
                dataGridViewExpressionsWork.CurrentCell = dataGridViewExpressionsWork.Rows[index].Cells[1];
                dataGridViewExpressionsWork.Rows[index].Cells[1].Selected = true;
                dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = indexDisplayed;
            }
        }

        private void dataGridViewExpressionsWork_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            point.X = e.ColumnIndex;
            point.Y = e.RowIndex;
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedExp();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            //MessageBox.Show(ExpNewText, "Text");
            try
            {
                int index = dataGridViewExpressionsWork.CurrentCell.RowIndex;
                int indexDisplayed = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;
                string ExpText = dataGridViewExpressionsWork.Rows[point.Y].Cells[1].Value.ToString();
                string idText = dataGridViewExpressionsWork.Rows[point.Y].Cells[0].Value.ToString();
                string idParentText = dataGridViewExpressionsWork.Rows[point.Y].Cells[2].Value.ToString();
                if (!String.IsNullOrEmpty(idText) && !String.IsNullOrEmpty(ExpText))
                {
                    using (FormOptionsExpressions formOptionExp = new FormOptionsExpressions())
                    {
                        formOptionExp.SelectExpText = ExpText;
                        formOptionExp.SelectExpId = idText;
                        formOptionExp.SelectExpParentId = idParentText;
                        formOptionExp.ManyExpFlag = false; //редактируем только одно выражение
                        formOptionExp.TreeFont = treeViewCreator.Font;
                        formOptionExp.TreeColor = treeViewCreator.ForeColor;
                        formOptionExp.TextFont = dataGridViewExpressionsWork.Font;
                        formOptionExp.TextColor = dataGridViewExpressionsWork.ForeColor;

                        //formSetTopic.SelectNode = e.Node;
                        formOptionExp.ShowDialog();

                    }


                    //заново рисуем таблицу выражений т.к. содержание её могло измениться
                    if (SelectNode.Name != "0")
                    {
                        Ostarbeiter ost = new Ostarbeiter();
                        DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                        dataGridViewExpressionsWork.DataSource = dt;
                        DataTable dtSearch = SearchExp();
                        dataGridViewExpressionsWork.DataSource = dtSearch;

                        labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();
                    }
                    else
                    {
                        ShowAllExp();

                    }
                    //выделяем и показываем редактируемый узел как и было до редактирования:
                    dataGridViewExpressionsWork.CurrentCell = dataGridViewExpressionsWork.Rows[index].Cells[1];
                    dataGridViewExpressionsWork.Rows[index].Cells[1].Selected = true;
                    dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = indexDisplayed;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Микола, курсор должен указывать на существующее выражение!\n System info:\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButtonEditCurrentNode_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SelectNode.Text))
            {
                using (FormSetTopic formSetTopic = new FormSetTopic())
                {
                    formSetTopic.SelectNode = SelectNode;
                    formSetTopic.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Выберите тему!", "Тема не выбрана", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButtonSetSubtopic_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(toolStripTextBoxSetSubtopic.Text))
            {
                int id = 0;
                string nodeValue = toolStripTextBoxSetSubtopic.Text;
                Ostarbeiter ost = new Ostarbeiter();

                //добавляем тему
                id = ost.SetSingleTopic(Properties.Settings.Default.PathFile, nodeValue, SelectNode.Name);

                //рисуем узел
                AddNode(id.ToString(), nodeValue);

                toolStripTextBoxSetSubtopic.Text = "";
                //toolStripStatusLabel2.Text = "";
            }
            else
            {
                MessageBox.Show("Введите название подтемы!", "Подтема не введена", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AddNode(string nodeName, string nodeValue)
        {
            SelectNode.Nodes.Add(nodeName, nodeValue);
            SelectNode.Expand();
        }

        private void toolStripButtonDeleteTopic_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SelectNode.Text))
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

                    //int index = SelectNode.Parent.Index;
                    string idNodeParent = SelectNode.Parent.Name;
                    //TreeNode newSelectNode = SelectNode.Parent;
                    int indexNode = SelectNode.Index;
                    //int levelNode = SelectNode.Level;

                    treeViewCreator.Nodes.Clear();
                    DrawAllTree();
                    root.Expand();

                    treeViewCreator.SelectedNode = root;

                    if (idNodeParent == "0")
                    {

                        treeViewCreator.TopNode.BackColor = Color.Yellow;
                        treeViewCreator.TopNode.ForeColor = Color.Black;                   

                        if(indexNode >= 1)
                        {
                            treeViewCreator.Nodes[0].Nodes[indexNode-1].EnsureVisible();
                            treeViewCreator.Nodes[0].Nodes[indexNode-1].Expand();
                        }

                        SelectNode = root;
                        //SelectNode = newSelectNode;
                        //выводим название выбранной темы в строку состояния
                        toolStripStatusLabel2.Text = SelectNode.Text;
                        toolStripStatusLabel2.ForeColor = Color.DarkGreen;

                        ////подсвечиваем тему жёлтеньким
                        //newSelectNode.BackColor = Color.Yellow;
                        //newSelectNode.ForeColor = Color.Black;


                        ShowAllExp();
                    }
                    else
                    {
                        //родительский узел делаем текущим
                        SelectedNodeRecursive(treeViewCreator.Nodes[0], idNodeParent);

                        //выводим название выбранной темы в строку состояния
                        toolStripStatusLabel2.Text = SelectNode.Text;
                        toolStripStatusLabel2.ForeColor = Color.DarkGreen;

                        //формируем таблицу выражений
                        DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, idNodeParent);
                        dataGridViewExpressionsWork.DataSource = dt;
                        DataTable dtSearch = SearchExp();
                        dataGridViewExpressionsWork.DataSource = dtSearch;

                        labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();

                    }
       
                }
            }
            else
            {
                MessageBox.Show("Выберите тему!", "Тема не выбрана", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SelectedNodeRecursive(TreeNode treeNode, string nodeName)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                if (tn.Name == nodeName)
                {
                    tn.EnsureVisible();
                    tn.ExpandAll();
                    tn.BackColor = Color.Yellow;
                    tn.ForeColor = Color.Black;
                    SelectNode = tn;
                    return;
                }
                SelectedNodeRecursive(tn, nodeName);
            }
        }

        private void RemoveSelectedNodeRecursive(TreeNode treeNode)
        {
            Ostarbeiter ost = new Ostarbeiter();
            foreach (TreeNode tn in treeNode.Nodes)
            {
                RemoveSelectedNodeRecursive(tn);
                ost.UpdatePerentIdTopicWithId(Properties.Settings.Default.PathFile, tn.Name, treeNode.Parent.Name);
                //tn.Remove();
            }
        }

        private void DeleteTopicRecursive(TreeNode treeNode)
        {

            foreach (TreeNode tn in treeNode.Nodes)
            {
                DeleteTopicRecursive(tn);
                Ostarbeiter ost = new Ostarbeiter();
                ost.DeleteSingleTopic(Properties.Settings.Default.PathFile, tn.Name);
                //tn.Remove();
            }
        }

        private void toolStripButtonSetExp_Click(object sender, EventArgs e)
        {
            SetExpSingleWithTextBox();
        }

        private void SetExpSingleWithTextBox()
        {
            int resultId = 0;
            if (!String.IsNullOrEmpty(toolStripTextBoxSetExp.Text))
            {
                if (SelectNode.Name != "0")
                {
                    Ostarbeiter ost = new Ostarbeiter();
                    resultId = ost.SetSingleExpression(Properties.Settings.Default.PathFile, toolStripTextBoxSetExp.Text, SelectNode.Name);
                    toolStripTextBoxSetExp.Text = "";

                    DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                    dataGridViewExpressionsWork.DataSource = dt;

                    SearchId(resultId.ToString());

                    //MessageBox.Show(resultId.ToString());

                    labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();

                }
                else
                {
                    MessageBox.Show("Выберите не корневой узел!", "Текущий узел:" + SelectNode.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Введите выражение!", "Выражение не введено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '\b') //если жмём backspace на textbox'e поиска то заново перерисовываем таблицу выражений согласно выбранной теме
            {
               
                Ostarbeiter ost = new Ostarbeiter();
                if (SelectNode.Name != "0")
                {
                    DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                    dataGridViewExpressionsWork.DataSource = dt;
                }
                else
                {
                    DataTable dtt = ost.GetDataTableFromDB(Properties.Settings.Default.PathFile);
                    dataGridViewExpressionsWork.DataSource = dtt;
                }
                

            }
        }

        private void textBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (SelectNode.Name != "0")
            {
                DataTable dt = SearchExp();
                dataGridViewExpressionsWork.DataSource = dt;
            }
            else
            {
                DataTable dtt = SearchExpAll();
                dataGridViewExpressionsWork.DataSource = dtt;
            }

            labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();

        }

        public DataTable SearchExp()
        {
            //int rowIndex = 0;
            DataTable dt = new DataTable("From DBDD");
            string selectString = "Text Like '%" + textBoxSearch.Text.Trim() + "%'";
            try
            {
                DataRowCollection allRows = ((DataTable)dataGridViewExpressionsWork.DataSource).Rows;

                DataRow[] searchedRows = ((DataTable)dataGridViewExpressionsWork.DataSource).Select(selectString);

                


                DataColumn columnId = new DataColumn("Id", typeof(int));
                DataColumn columnText = new DataColumn("Text", typeof(string));
                DataColumn columnIdTopic = new DataColumn("Topic", typeof(int));

                //добавляем столбцы в таблицу
                dt.Columns.AddRange(new DataColumn[] { columnId, columnText, columnIdTopic });

                foreach (DataRow rows in searchedRows)
                {
                    //MessageBox.Show(row[1].ToString());
                    //формируем строку
                    DataRow row = dt.NewRow();
                    row["Id"] = rows[0].ToString();
                    row["Text"] = rows[1].ToString();
                    row["Topic"] = rows[2].ToString();

                    //добавляем строку в таблицу
                    dt.Rows.Add(row);

                }

                return dt;
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable SearchExpAll()
        {
            //int rowIndex = 0;
            DataTable dt = new DataTable("From DBDD");
            string selectString = "Text Like '%" + textBoxSearch.Text.Trim() + "%'";
            try
            {
                DataRowCollection allRows = ((DataTable)dataGridViewExpressionsWork.DataSource).Rows;

                DataRow[] searchedRows = ((DataTable)dataGridViewExpressionsWork.DataSource).Select(selectString);




                DataColumn columnId = new DataColumn("Id", typeof(int));
                DataColumn columnText = new DataColumn("Text", typeof(string));
                DataColumn columnIdTopic = new DataColumn("Topic", typeof(int));
                DataColumn columnTextTopic = new DataColumn("Topics", typeof(string));

                //добавляем столбцы в таблицу
                dt.Columns.AddRange(new DataColumn[] { columnId, columnText, columnIdTopic, columnTextTopic });

                foreach (DataRow rows in searchedRows)
                {
                    //MessageBox.Show(row[1].ToString());
                    //формируем строку
                    DataRow row = dt.NewRow();
                    row["Id"] = rows[0].ToString();
                    row["Text"] = rows[1].ToString();
                    row["Topic"] = rows[2].ToString();
                    row["Topics"] = rows[3].ToString();

                    //добавляем строку в таблицу
                    dt.Rows.Add(row);

                }

                return dt;
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public void SearchId(string idText)
        {
            int rowIndex = 0;
            string selectString = "Id = " + idText.Trim();
            //MessageBox.Show(idText.Trim(), "123");
            try
            {
                DataRowCollection allRows = ((DataTable)dataGridViewExpressionsWork.DataSource).Rows;

                DataRow[] searchedRows = ((DataTable)dataGridViewExpressionsWork.DataSource).Select(selectString);
                if (searchedRows != null)
                    if (allRows.IndexOf(searchedRows[0]) != 0 && allRows.IndexOf(searchedRows[0]) != null)
                    {
                        rowIndex = allRows.IndexOf(searchedRows[0]);
                    }

                dataGridViewExpressionsWork.CurrentCell = dataGridViewExpressionsWork[1, rowIndex];

                //dataGridViewExpressionsWork[1, rowIndex].Style.BackColor = Color.Crimson;
               

            }
            catch (Exception ex)
            {

            }
        }

        private void toolStripButtonEditExp_Click(object sender, EventArgs e)
        {

            if (dataGridViewExpressionsWork.SelectedCells.Count > 0)
            {

                int index = dataGridViewExpressionsWork.CurrentCell.RowIndex;
                int indexDisplayed = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;
                string ExpText = dataGridViewExpressionsWork.Rows[index].Cells[1].Value.ToString();
                string idText = dataGridViewExpressionsWork.Rows[index].Cells[0].Value.ToString();
                string idParentText = dataGridViewExpressionsWork.Rows[index].Cells[2].Value.ToString();



                // MessageBox.Show(ExpText + "\n" + idText + "\n" + idParentText, "Text");
                if (!String.IsNullOrEmpty(idText) && !String.IsNullOrEmpty(ExpText))
                {
                    using (FormOptionsExpressions formOptionExp = new FormOptionsExpressions())
                    {
                        formOptionExp.SelectExpText = ExpText;
                        formOptionExp.SelectExpId = idText;
                        formOptionExp.SelectExpParentId = idParentText;
                        formOptionExp.ManyExpFlag = false;
                        formOptionExp.TreeFont = treeViewCreator.Font;
                        formOptionExp.TreeColor = treeViewCreator.ForeColor;
                        formOptionExp.TextFont = dataGridViewExpressionsWork.Font;
                        formOptionExp.TextColor = dataGridViewExpressionsWork.ForeColor;

                        //formSetTopic.SelectNode = e.Node;
                        formOptionExp.ShowDialog();

                    }

                    //заново рисуем таблицу выражений т.к. содержание её могло измениться
                    if (SelectNode.Name != "0")
                    {
                        Ostarbeiter ost = new Ostarbeiter();
                        DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                        dataGridViewExpressionsWork.DataSource = dt;
                        DataTable dtSearch = SearchExp();
                        dataGridViewExpressionsWork.DataSource = dtSearch;

                        labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();
                    }
                    else
                    {
                        ShowAllExp();

                    }
                    //выделяем и показываем редактируемый узел как и было до редактирования:
                    dataGridViewExpressionsWork.CurrentCell = dataGridViewExpressionsWork.Rows[index].Cells[1];
                    dataGridViewExpressionsWork.Rows[index].Cells[1].Selected = true;
                    dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = indexDisplayed;

                }
            }
        }

       

        #region DragAndDrop tree
        private void treeViewCreator_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeViewCreator.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewCreator_DragDrop(object sender, DragEventArgs e)
        {
            TreeViewHitTestInfo hti = treeViewCreator.HitTest(treeViewCreator.PointToClient(new Point(e.X, e.Y)));
            if (hti.Node != null)
            {
                if (e.Data.GetDataPresent(typeof(TreeNode))) //если перемещаем другую тему
                {
                    TreeNode droppedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                    TreeNode clonedNode = (TreeNode)droppedNode.Clone();
                    if (hti.Node != droppedNode) //если это не тот же самый узел
                    {
                        TreeNode[] tn = droppedNode.Nodes.Find(hti.Node.Name, true); //получаем коллекцию дочерних узлов перемещаемого узла, которые соответствуют выбранному

                        if (tn.Length == 0) //если родительский узел не перемещается в дочерний
                        {
                            Ostarbeiter ost = new Ostarbeiter();
                            //изменяем в базе данных id родительского узла для перемещённого узла
                            ost.UpdatePerentIdTopicWithId(Properties.Settings.Default.PathFile, droppedNode.Name, hti.Node.Name);
                            droppedNode.Remove(); //удаляем перемещаемый
                            hti.Node.Nodes.Add(clonedNode); //перемещаем его в новое место
                            hti.Node.Expand();
                        }
                    }
                }
                if (e.Data.GetDataPresent(typeof(DataGridViewRow))) //если перемещаем выделенные выражения в тему
                {
                    //string rowText = "";
                    //string idText = "";
                    List<int> listIdExp = new List<int>();
                    Ostarbeiter ost = new Ostarbeiter();
                    //DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                    //dataGridViewExpressionsWork.Rows.RemoveAt(rowIndexFromMouseDown);
                    //dataGridViewExpressionsWork.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
                    //MessageBox.Show(rowToMove.Cells[1].Value.ToString(), "Что-то перетащили");
                    try
                    {
                        int index = dataGridViewExpressionsWork.CurrentCell.RowIndex;
                        int indexDisplayed = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;

                        for (int i = 0; i < dataGridViewExpressionsWork.RowCount; i++)
                        {
                            // for (int j = 0; j < dataGridViewExpressionsWork.ColumnCount; j++)
                            //{
                                if (dataGridViewExpressionsWork[1, i].Selected)
                                {

                                    //rowText = dataGridViewExpressionsWork[1, i].Value.ToString();
                                    
                                    listIdExp.Add(Int32.Parse(dataGridViewExpressionsWork[0, i].Value.ToString()));
                                    
                                    
                                    //ost.UpdateSingleExpressionIdParentWithId(Properties.Settings.Default.PathFile, idText, hti.Node.Name); //меняем тему для данного выражения
                                    
                                }
                            //}
                        }

                        ost.UpdateListExpressionsIdParentWithId(Properties.Settings.Default.PathFile, listIdExp, hti.Node.Name);



                        if (SelectNode.Name != "0")
                        {
                            DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name); //перерисовываем таблицу выражений
                            dataGridViewExpressionsWork.DataSource = dt;

                            DataTable dtSearch = SearchExp();
                            dataGridViewExpressionsWork.DataSource = dtSearch;
                        }
                        else
                        {
                            DataTable dt = ost.GetDataTableFromDB(Properties.Settings.Default.PathFile); //перерисовываем таблицу выражений
                            dataGridViewExpressionsWork.DataSource = dt;

                            DataTable dtSearch = SearchExpAll();
                            dataGridViewExpressionsWork.DataSource = dtSearch;
                        }


                        dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = indexDisplayed;

                        labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Микола, нельзя выделить и переместить пустую строку!\n System info:\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void treeViewCreator_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                e.Effect = DragDropEffects.Move;
            }

            //скроллинг
            TreeView tv = sender as TreeView;
            Point pt = tv.PointToClient(new Point(e.X, e.Y));
            try
            {
                int delta = tv.Height - pt.Y;
                if ((delta < tv.Height / 2) && (delta > 0))
                {
                    TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
                    if (tn.NextVisibleNode != null)
                        tn.NextVisibleNode.EnsureVisible();
                }
                if ((delta > tv.Height / 2) && (delta < tv.Height))
                {
                    TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
                    if (tn.PrevVisibleNode != null)
                        tn.PrevVisibleNode.EnsureVisible();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region DragAndDrop dataGrid
        private void dataGridViewExpressionsWork_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            /*
            // Get the index of the item the mouse is below.
            dataGridViewExpressionsWork.ColumnHeadersVisible = false;


            Point clientPoint = dataGridViewExpressionsWork.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below.
            //rowIndexOfItemUnderMouseToDrop = dataGridViewExpressionsWork.HitTest(clientPoint.X, clientPoint.Y).RowIndex;



            rowIndexFromMouseDown = dataGridViewExpressionsWork.HitTest(e.X, e.Y).RowIndex;
            //rowIndexFromMouseDown = dataGridViewExpressionsWork.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            
            //MessageBox.Show(dataGridViewExpressionsWork.Rows[rowIndexFromMouseDown].Cells[1].Value.ToString());
            MessageBox.Show(rowIndexFromMouseDown.ToString());

            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.               
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
            }
            dataGridViewExpressionsWork.ColumnHeadersVisible = true;
            */
        }

        private void dataGridViewExpressionsWork_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            /*
            //dataGridViewExpressionsWork.ColumnHeadersVisible = false;
            // Если курсор вышел за пределы ListView - начинаем перетаскивание
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                //MessageBox.Show("right press begin move!");
                 // If the mouse moves outside the rectangle, start the drag.
                 if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                 {
                   // MessageBox.Show("begin move!");
                     // Proceed with the drag and drop, passing in the list item.                   
                     DragDropEffects dropEffect = dataGridViewExpressionsWork.DoDragDrop(dataGridViewExpressionsWork.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                 }
            }
           // dataGridViewExpressionsWork.ColumnHeadersVisible = true;
           */
        }
            
        private void dataGridViewExpressionsWork_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            //скроллинг
            /* DataGridView tv = sender as DataGridView;
             Point pt = tv.PointToClient(new Point(e.X, e.Y));
             Point clientPoint = dataGridViewExpressionsWork.PointToClient(new Point(e.X, e.Y));

             // Get the row index of the item the mouse is below.
             int rowIndexOf = dataGridViewExpressionsWork.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
             try
             {
                 int delta = tv.Height - pt.Y;
                 if ((delta < tv.Height / 2) && (delta > 0))
                 {

                     //dataGridViewExpressionsWork.FirstDisplayedCell = dataGridViewExpressionsWork.Rows[rowIndexOf - 1].Cells[1];
                     dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex + 1;
                     //dataGridViewExpressionsWork.
                 }
                 if ((delta > tv.Height / 2) && (delta < tv.Height))
                 {

                     //dataGridViewExpressionsWork.FirstDisplayedCell = dataGridViewExpressionsWork.Rows[rowIndexOf + 1].Cells[1];
                     dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex - 1;

                 }
             }
             catch (Exception ex)
             {

             }*/
            try
            {
                if (e.Y <= PointToScreen(new Point(dataGridViewExpressionsWork.Location.X, dataGridViewExpressionsWork.Location.Y)).Y + 80)
                {
                    if (dataGridViewExpressionsWork.RowCount > 1)
                    {
                        dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex -= 1;
                    }
                    //MessageBox.Show("Есть");
                }

                if (e.Y >= PointToScreen(new Point(dataGridViewExpressionsWork.Location.X, dataGridViewExpressionsWork.Location.Y + dataGridViewExpressionsWork.Height)).Y - 10)
                {
                    dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex += 1;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridViewExpressionsWork_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be
            // converted to client coordinates.
            Point clientPoint = dataGridViewExpressionsWork.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below.
            rowIndexOfItemUnderMouseToDrop = dataGridViewExpressionsWork.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                try
                {
                    if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
                    {
                        //меняем строки местами
                        string buffer = dataGridViewExpressionsWork.Rows[rowIndexOfItemUnderMouseToDrop].Cells[1].Value.ToString();
                        dataGridViewExpressionsWork.Rows[rowIndexOfItemUnderMouseToDrop].Cells[1].Value = dataGridViewExpressionsWork.Rows[rowIndexFromMouseDown].Cells[1].Value;
                        dataGridViewExpressionsWork.Rows[rowIndexFromMouseDown].Cells[1].Value = buffer;
                        
                    } 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("микола, нельзя строки поменять местами\nНет второй строки!\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        /// <summary>
        /// Добавляем несколько выражений одновременно (выражения вводятся через перенос строки)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonAddManyExp_Click(object sender, EventArgs e)
        {
            int index = 0;
            int indexDisplayed = 0;
            if (dataGridViewExpressionsWork.RowCount > 0)
            {
                index = dataGridViewExpressionsWork.CurrentCell.RowIndex;
                indexDisplayed = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;
            }

            using (FormOptionsExpressions formOptionExp = new FormOptionsExpressions())
            {
                formOptionExp.SelectExpText = "";
                formOptionExp.SelectExpId = "";
                formOptionExp.SelectExpParentId = SelectNode.Name;
                formOptionExp.TreeFont = treeViewCreator.Font;
                formOptionExp.TreeColor = treeViewCreator.ForeColor;
                formOptionExp.TextFont = dataGridViewExpressionsWork.Font;
                formOptionExp.TextColor = dataGridViewExpressionsWork.ForeColor;
                //MessageBox.Show(SelectNode.Name);
                formOptionExp.ManyExpFlag = true;
                formOptionExp.ShowDialog();
            }
            
            //заново рисуем таблицу выражений т.к. содержание её могло измениться
            Ostarbeiter ost = new Ostarbeiter();
            if (SelectNode.Name != "0")
            {
                DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                dataGridViewExpressionsWork.DataSource = dt;

                DataTable dtSearch = SearchExp();
                dataGridViewExpressionsWork.DataSource = dtSearch;
            }

            labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();

            if (dataGridViewExpressionsWork.RowCount > 0)
            {
                //выделяем и показываем редактируемый узел как и было до редактирования:
                dataGridViewExpressionsWork.CurrentCell = dataGridViewExpressionsWork.Rows[index].Cells[1];
                dataGridViewExpressionsWork.Rows[index].Cells[1].Selected = true;
                dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = indexDisplayed;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedExp();
        }

        private void DeleteSelectedExp()
        {
            string idText = "";
            Ostarbeiter ost = new Ostarbeiter();

            if (dataGridViewExpressionsWork.SelectedCells.Count > 0)
            {
                int indexDisplayed = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;
                DialogResult r = MessageBox.Show("Удалить выделенные выражения?\nКоличество: " +
            dataGridViewExpressionsWork.SelectedCells.Count, "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                {
                    try
                    {


                        for (int i = 0; i < dataGridViewExpressionsWork.RowCount; i++)
                            for (int j = 0; j < dataGridViewExpressionsWork.ColumnCount; j++)
                            {
                                if (dataGridViewExpressionsWork[j, i].Selected)
                                {

                                    idText = dataGridViewExpressionsWork[j - 1, i].Value.ToString();
                                    ost.DeleteSingleExpression(Properties.Settings.Default.PathFile, idText);
                                }
                            }

                        if (SelectNode.Name != "0")
                        {
                            DataTable dt = ost.GetDateTableWithId(Properties.Settings.Default.PathFile, SelectNode.Name);
                            dataGridViewExpressionsWork.DataSource = dt;

                            DataTable dtSearch = SearchExp();
                            dataGridViewExpressionsWork.DataSource = dtSearch;
                        }
                        else
                        {
                            ShowAllExp();
                        }

                        labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();

                        //dataGridViewExpressionsWork.CurrentCell = dataGridViewExpressionsWork.Rows[index].Cells[1];
                        //dataGridViewExpressionsWork.Rows[index].Cells[1].Selected = true;
                        dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = indexDisplayed;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Микола, " + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

   

        private void toolStripButtonTopics_Click(object sender, EventArgs e)
        {
            fontDialogMain.ShowColor = true;

            fontDialogMain.Font = treeViewCreator.Font;
            fontDialogMain.Color = treeViewCreator.ForeColor;

            if (fontDialogMain.ShowDialog() != DialogResult.Cancel)
            {
                treeViewCreator.Font = fontDialogMain.Font;
                treeViewCreator.ForeColor = fontDialogMain.Color;
                Properties.Settings.Default.FontTree = fontDialogMain.Font;
                Properties.Settings.Default.ColorTree = fontDialogMain.Color;
                Properties.Settings.Default.Save();

            }
        }

        private void toolStripButtonExp_Click(object sender, EventArgs e)
        {
            fontDialogMain.ShowColor = true;

            fontDialogMain.Font = dataGridViewExpressionsWork.Font;
            fontDialogMain.Color = dataGridViewExpressionsWork.ForeColor;

            if (fontDialogMain.ShowDialog() != DialogResult.Cancel)
            {
                dataGridViewExpressionsWork.Font = fontDialogMain.Font;
                dataGridViewExpressionsWork.ForeColor = fontDialogMain.Color;
                Properties.Settings.Default.FontExpressions = fontDialogMain.Font;
                Properties.Settings.Default.ColorExpressions = fontDialogMain.Color;
                Properties.Settings.Default.Save();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            
        }

        private void saveFileDialogMain_FileOk(object sender, CancelEventArgs e)
        {
            /*MessageBox.Show("ddddddd");
            if (saveFileDialogMain.FileName != "")
            {
                string name = saveFileDialogMain.FileName;
                string dbName = Properties.Settings.Default.PathFile;
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialogMain.OpenFile();
                MessageBox.Show(dbPath, "имя источника");
                MessageBox.Show(name, "имя файла назначения");




                //File.Copy()

            }*/
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialogMain = new SaveFileDialog();
            saveFileDialogMain.InitialDirectory = Path.Combine(Application.StartupPath, "backup");
            saveFileDialogMain.Filter = "database files (*.db3)|*.db3|All files (*.*)|*.*";
            saveFileDialogMain.Title = "Save an db3 File";
            saveFileDialogMain.ShowDialog();
            if (saveFileDialogMain.FileName != "")
            {
                try
                {
                    string pathNewFile = saveFileDialogMain.FileName;
                    string dbName = Properties.Settings.Default.PathFile;
                    var dbPath = Path.Combine(Application.StartupPath, dbName);
                    //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialogMain.OpenFile();
                    //MessageBox.Show(dbPath, "имя источника");
                    //MessageBox.Show(name, "имя файла назначения");
                    File.Delete(pathNewFile);
                    File.Copy(dbPath, pathNewFile);

                    Properties.Settings.Default.PathFileOpen = pathNewFile;
                    Properties.Settings.Default.Save();

                    //имя открытого файла в статус-строке
                    string a = System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.PathFileOpen);
                    toolStripStatusLabelFileOpen.Text = a;
                    toolStripStatusLabelFileOpen.ForeColor = Color.DarkCyan;

                    this.Text = "Ideographic Creator v1.5.8  File open: " + Properties.Settings.Default.PathFileOpen;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripButtonExpAll_Click(object sender, EventArgs e)
        {
            //сохраняем положение текущего выражения  в списке выражений для предыдущего узла
            dictCurrentExp[SelectNode.Name] = dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex;

            //предыдущую выбранную тему перерисовываем системным цветом
            SelectNode.BackColor = Color.White;
            SelectNode.ForeColor = treeViewCreator.ForeColor;

            SelectNode = root; //выбираем корневой узел
            toolStripStatusLabel2.Text = SelectNode.Text;

            //подсвечиваем тему жёлтеньким
            SelectNode.BackColor = Color.Yellow;
            SelectNode.ForeColor = Color.Black;

            ShowAllExp();
        }

        private void ShowAllExp()
        {
            Ostarbeiter ost = new Ostarbeiter();
            DataTable dt = ost.GetDataTableFromDB(Properties.Settings.Default.PathFile);
            dataGridViewExpressionsWork.DataSource = dt;
            //dataGridViewExpressionsWork.ColumnHeadersVisible = false;
            dataGridViewExpressionsWork.Columns[0].Visible = false;
            dataGridViewExpressionsWork.Columns[2].Visible = false;
            dataGridViewExpressionsWork.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //SelectNode = root;

            DataTable dtSearch = SearchExpAll();
            dataGridViewExpressionsWork.DataSource = dtSearch;

            //выводим количество выражений
            labelCountExp.Text = dataGridViewExpressionsWork.RowCount.ToString();


            SetCurrentRow();

        }

        private void SetCurrentRow()
        {
            //показываем то выражение на котором остановились в прошлый раз
            if (dataGridViewExpressionsWork.RowCount > 0)
            {
                dataGridViewExpressionsWork.ClearSelection();
                if ((dataGridViewExpressionsWork.RowCount - 1) > (dictCurrentExp[SelectNode.Name]))
                {
                    dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = dictCurrentExp[SelectNode.Name];
                    dataGridViewExpressionsWork.Rows[dictCurrentExp[SelectNode.Name]].Selected = true;
                }
                else
                {
                    dataGridViewExpressionsWork.FirstDisplayedScrollingRowIndex = dataGridViewExpressionsWork.RowCount - 1;
                }
            }
        }
        

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogMain = new OpenFileDialog();

            openFileDialogMain.InitialDirectory = Path.Combine(Application.StartupPath, "backup"); 
            openFileDialogMain.Filter = "database files (*.db3)|*.db3|All files (*.*)|*.*";
            openFileDialogMain.FilterIndex = 2;
            openFileDialogMain.RestoreDirectory = true;

            if (!String.IsNullOrEmpty(Properties.Settings.Default.PathFileOpen) && System.IO.File.Exists(Properties.Settings.Default.PathFileOpen))
            {
                DialogResult dialogResult = MessageBox.Show("Сохранить файл " + Properties.Settings.Default.PathFileOpen + " ?", "Текущий файл не сохранён", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    
                    string dbName = Properties.Settings.Default.PathFile;
                    var dbPath = Path.Combine(Application.StartupPath, dbName);
                    //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialogMain.OpenFile();
                    //MessageBox.Show(dbPath, "имя источника");
                    //MessageBox.Show(name, "имя файла назначения");
                    File.Delete(Properties.Settings.Default.PathFileOpen);
                    File.Copy(dbPath, Properties.Settings.Default.PathFileOpen);
                }
                //else if (dialogResult == DialogResult.No)
                //{
                //    //do something else
                //}
            }

            if (openFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialogMain.FileName != "")
                {

                    try
                    {
                        string pathOpenFile = openFileDialogMain.FileName;
                        string dbName = Properties.Settings.Default.PathFile;
                        var dbPath = Path.Combine(Application.StartupPath, dbName);
                        //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialogMain.OpenFile();
                        //MessageBox.Show(dbPath, "имя источника");
                        //MessageBox.Show(name, "имя файла назначения");
                        Properties.Settings.Default.PathFileOpen = pathOpenFile;
                        Properties.Settings.Default.Save();
                        File.Delete(dbPath);
                        File.Copy(pathOpenFile, dbPath);
                        ShowAllExp();
                        treeViewCreator.Nodes.Clear();
                        DrawAllTree();
                        root.Expand();

                        //имя открытого файла в статус-строке
                        string a = System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.PathFileOpen);
                        toolStripStatusLabelFileOpen.Text = a;
                        toolStripStatusLabelFileOpen.ForeColor = Color.DarkCyan;

                        this.Text = "Ideographic Creator v1.5.8  File open: " + Properties.Settings.Default.PathFileOpen;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
        }

        private void toolStripTextBoxSetExp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                SetExpSingleWithTextBox();
            }

        }

        private void checkBoxSortTree_CheckedChanged(object sender, EventArgs e)
        {
                DrawAllTree();
        }

        private void dataGridViewExpressionsWork_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            //dataGridViewExpressionsWork.ColumnHeadersVisible = false;


            //Point clientPoint = dataGridViewExpressionsWork.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below.
            //rowIndexOfItemUnderMouseToDrop = dataGridViewExpressionsWork.HitTest(clientPoint.X, clientPoint.Y).RowIndex;



            rowIndexFromMouseDown = dataGridViewExpressionsWork.HitTest(e.X, e.Y).RowIndex;
            //rowIndexFromMouseDown = dataGridViewExpressionsWork.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            //MessageBox.Show(dataGridViewExpressionsWork.Rows[rowIndexFromMouseDown].Cells[1].Value.ToString());
            //MessageBox.Show(rowIndexFromMouseDown.ToString());

            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.               
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
            }
            //dataGridViewExpressionsWork.ColumnHeadersVisible = true;
        }

        private void dataGridViewExpressionsWork_MouseMove(object sender, MouseEventArgs e)
        {
            //dataGridViewExpressionsWork.ColumnHeadersVisible = false;
            // Если курсор вышел за пределы ListView - начинаем перетаскивание
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                //MessageBox.Show("right press begin move!");
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // MessageBox.Show("begin move!");
                    // Proceed with the drag and drop, passing in the list item.                   
                    DragDropEffects dropEffect = dataGridViewExpressionsWork.DoDragDrop(dataGridViewExpressionsWork.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
            // dataGridViewExpressionsWork.ColumnHeadersVisible = true;
        }

        private void buttonChoiseTopics_Click(object sender, EventArgs e)
        {
            if (flagTopic == 0)
            {
                flagTopic = 1;
                treeViewCreator.CheckBoxes = true;
                buttonChoiseTopics.Text = "Работать с выбранными";
                return;
            }
            if (flagTopic == 1)
            {
                flagTopic = 2;
                buttonChoiseTopics.Text = "Показать все темы";
                //int countIndex = 0;
                //string selectedNode = "Selected customer nodes are : ";
                listCheckedTopics.Clear();
                CallRecursive(treeViewCreator);
                treeViewCreator.Nodes.Clear();
                DrawCheckedTree();
                treeViewCreator.CheckBoxes = false;
                treeViewCreator.ExpandAll();
                /*
                foreach (string str in listCheckedTopics)
                {
                    MessageBox.Show(str);
                }*/


                //TreeChekedShowRecursive(treeViewCreator.Node[1]);
                return;
            }

            if (flagTopic == 2)
            {
                flagTopic = 0;
                
                
                buttonChoiseTopics.Text = "Отметить темы для работы";
                treeViewCreator.Nodes.Clear();
                DrawAllTree();
                //treeViewCreator.ExpandAll();
                root.Expand();
                return;

            }
        }

        private void TreeChekedShowRecursive(TreeNode treeNode)
        {
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    TreeChekedShowRecursive(tn);

                    if (tn.Checked == true || tn.Name == "0")
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

        private void DrawCheckedTree()
        {

            Ostarbeiter ost = new Ostarbeiter();

            if (treeViewCreator.GetNodeCount(true) == 0)
            {
                root = ost.GetTreeNodeTopicsChecked(Properties.Settings.Default.PathFile, listCheckedTopics);
                treeViewCreator.Nodes.Add(root);
            }

            if (checkBoxSortTree.Checked == true)
            {
                treeViewCreator.Sort();
            }

            //treeViewCreator.TreeViewNodeSorter =
        }

        private void FormMainCreator_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.PathFileOpen) || System.IO.File.Exists(Properties.Settings.Default.PathFileOpen))
            {
                try
                {
                    string pathNewFile = Properties.Settings.Default.PathFileOpen;
                    string dbName = Properties.Settings.Default.PathFile;
                    var dbPath = Path.Combine(Application.StartupPath, dbName);
                    //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialogMain.OpenFile();
                    //MessageBox.Show(dbPath, "имя источника");
                    //MessageBox.Show(name, "имя файла назначения");
                    File.Delete(pathNewFile);
                    File.Copy(dbPath, pathNewFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewExpressionsWork_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                MessageBox.Show("Ya-aaaa-choooo! IdTopic = " + dataGridViewExpressionsWork.Rows[e.RowIndex].Cells[2].Value.ToString()
                    + " TopicName = " + dataGridViewExpressionsWork.Rows[e.RowIndex].Cells[3].Value.ToString(), "Info Clic");
            }
        }
    }
}
