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
    public partial class FormOptionsExpressions : Form
    {
        public string SelectExpText { get; set; }
        public string SelectExpId { get; set; }
        public string SelectExpParentId { get; set; }
        public string SelectNodeId { get; set; }
        public string SelectNodeName { get; set; }

        //public TreeNode SelectNode { get; set; }
        public Font TreeFont { get; set; }
        public Font TextFont { get; set; }
        public Color TreeColor { get; set; }
        public Color TextColor { get; set; }
        public bool ManyExpFlag { get; set; }

        public List<string> listTopicLabels = new List<string>();

        private bool keyTimer = true;
        private bool flagKeyboard = false;
        private int cursorTextExp = 0;

        public FormOptionsExpressions()
        {
            InitializeComponent();
        }

        private void FormOptionsExpressions_Load(object sender, EventArgs e)
        {
            if (ManyExpFlag == false)
            {
                Ostarbeiter ost = new Ostarbeiter();

                textBoxExpOptionExp.Text = SelectExpText;
                if (SelectExpParentId != "0")
                {
                    labelTopicOptionExp.Text = ost.GetTopicTextWithId(Properties.Settings.Default.PathFile, SelectExpParentId);
                }
                else
                {
                    labelTopicOptionExp.Text = "Topics";
                }

                labelSaveExpText.Text = "";
                //labelSaveExpText.ForeColor = Color.DarkGreen;

                labelTopicSetOptionSet.Text = "";
                labelTopicOptionExp.ForeColor = Color.DarkGreen;

                SelectNodeId = SelectExpParentId;
                SelectNodeName = labelTopicOptionExp.Text;
            }
            else
            {
                buttonChangeExpOptionExp.Location = new Point(22, 530);
                buttonChangeExpOptionExp.Width -= 35;
                
                buttonFormSetOptionExpClose.Location = new Point(22, 623);
                labelSaveExpText.Location = new Point(115, 535);
                labelSaveExpText.Text = "Выражения не введены";
                //buttonTopicSetOptionExp.Location = new Point(22, 568);
                buttonTopicSetOptionExp.Visible = false;
                button1.Location = new Point(22, 576);
                //labelTopicSetOptionSet.Location = new Point(165, 574);
                labelTopicSetOptionSet.Location = new Point(60, 498);
                label1.Location = new Point(18, 501);
                label1.Text = "Tема:";
                //labelTopicOptionExp.Location = new Point(115, 506);
                labelTopicOptionExp.Visible = false;
                buttonDeleteExp.Visible = false;
                //textBoxExpOptionExp.Size = new System.Drawing.Size(502, 438);
                textBoxExpOptionExp.Height = 438;

                lstBxLabels.Location = new Point(255, 520);
                btnLabels.Location = new Point(255, 623);
                lblLabels.Location = new Point(255, 501);
                lstBxLabels.Height -= 15;

                labelExpOptionExp.Text = "Введите выражения (каждое новое с новой строки):";
                this.Height = 700;
                this.Width = 800;

                if (SelectExpParentId != "0")
                {
                    Ostarbeiter ost = new Ostarbeiter();
                    //labelTopicOptionExp.Text = ost.GetTopicTextWithId(Properties.Settings.Default.PathFile, SelectExpParentId);
                    labelTopicSetOptionSet.Text = ost.GetTopicTextWithId(Properties.Settings.Default.PathFile, SelectExpParentId);
                }
                else
                {
                    //labelTopicOptionExp.Text = "Topics";
                    labelTopicSetOptionSet.Text = "Topics";
                }
                //labelTopicOptionExp.ForeColor = Color.DarkGreen;
                labelTopicSetOptionSet.ForeColor = Color.DarkGreen;
                SelectNodeId = SelectExpParentId;
                SelectNodeName = labelTopicSetOptionSet.Text;

            }

            textBoxExpOptionExp.Font = TextFont;
            textBoxExpOptionExp.ForeColor = TextColor;
            labelTopicSetOptionSet.Font = TextFont;
            //labelTopicOptionExp.Font = TextFont;
            DrawAllTree();

            panelKeyboard.Visible = false;

            //ссылки не доступны для корневого узла:
            if (SelectNodeId == "0")
            {
                btnLabels.Enabled = false;
                lstBxLabels.Enabled = false;
            }
            else
            {
                btnLabels.Enabled = true;
                lstBxLabels.Enabled = true;
                ShowCurrentLabels();
            }


        }

        private void DrawAllTree()
        {
            TreeNode root;//, child;
            Ostarbeiter ost = new Ostarbeiter();

            if (treeViewOptionExp.GetNodeCount(true) == 0)
            {
                root = ost.GetTreeNodeTopicsAll(Properties.Settings.Default.PathFile);
                treeViewOptionExp.Nodes.Add(root);
                //treeViewOptionExp.ExpandAll();
                treeViewOptionExp.Nodes[0].Expand();

               // SelectNode = root;
            }

            treeViewOptionExp.Font = TreeFont;
            treeViewOptionExp.ForeColor = TreeColor;

            
        }

        private void treeViewOptionExp_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            labelTopicSetOptionSet.Text = e.Node.Text;
            labelTopicSetOptionSet.ForeColor = Color.DarkGreen;

            //SelectNode.BackColor = Color.White;
           // SelectNode.ForeColor = treeViewOptionExp.ForeColor;

            //SelectNode = e.Node;

           // SelectNode.BackColor = Color.Yellow;
            //SelectNode.ForeColor = Color.Black;

            SelectNodeId = e.Node.Name;
            SelectNodeName = e.Node.Text;
            //MessageBox.Show(SelectNodeId);

            if (SelectNodeId == "0")
            {
                btnLabels.Enabled = false;
                lstBxLabels.Enabled = false;
            }
            else
            {
                btnLabels.Enabled = true;
                lstBxLabels.Enabled = true;
                ShowCurrentLabels();
            }

        }

        private void buttonChangeExpOptionExp_Click(object sender, EventArgs e)
        {
            Ostarbeiter ost = new Ostarbeiter();

            if (ManyExpFlag == false)
            {
                if (!String.IsNullOrEmpty(SelectExpId) && !String.IsNullOrEmpty(textBoxExpOptionExp.Text))
                {
                    //MessageBox.Show("AAAAAAAAAAAAAAAAAAAAAAA!");
                    //MessageBox.Show(SelectExpId.ToString(), "Id exp");
                    //MessageBox.Show(textBoxExpOptionExp.Text, "New text");
                    ost.UpdateSingleExpressionWithId(Properties.Settings.Default.PathFile, SelectExpId, textBoxExpOptionExp.Text);
                }
            }
            else
            {
                if (textBoxExpOptionExp.Text != "")
                {
                    String ManyExp = textBoxExpOptionExp.Text;
                    Char delimiter = '\n';
                    String[] Expressions = ManyExp.Split(delimiter);
                    int count = 0;

                    List<string> str = new List<string>();

                    foreach (var substring in Expressions)
                    {
                        String substringTrim = substring.Trim();
                        //if (!String.IsNullOrEmpty(substringTrim))
                        //{

                        //    ost.SetSingleExpression(Properties.Settings.Default.PathFile, substringTrim, SelectNodeId);
                        //}


                        if (!String.IsNullOrEmpty(substringTrim))
                        {
                            str.Add(substringTrim);  
                            count++;  
                        }
                    
                        

                    }
                    ost.SetListExpressions(Properties.Settings.Default.PathFile, str, SelectNodeId);
                    MessageBox.Show("Добавлено выражений: " + count.ToString() + " \nв тему \"" + SelectNodeName + "\"", "Выражения добавлены", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxExpOptionExp.Text = "";

                }
                else
                {
                    MessageBox.Show("Введите выражения..", "Выражения не введены", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            labelSaveExpText.Text = "Сохранено!";
            labelSaveExpText.ForeColor = Color.DarkGreen;
        }

        private void textBoxExpOptionExp_TextChanged(object sender, EventArgs e)
        {
            labelSaveExpText.Text = "Изменения не сохранены!";
            labelSaveExpText.ForeColor = Color.DarkRed;
        }

        private void buttonFormSetOptionExpClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDeleteExp_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Удалить выражение\n" + SelectExpText + "?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {
                Ostarbeiter ost = new Ostarbeiter();
                ost.DeleteSingleExpression(Properties.Settings.Default.PathFile, SelectExpId);
                //SelectNode.Remove();
            }
            this.Close();
        }

        private void buttonTopicSetOptionExp_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(labelTopicSetOptionSet.Text))
            {
                if (ManyExpFlag == false)
                {
                    if (SelectNodeId != "0")
                    {
                        Ostarbeiter ost = new Ostarbeiter();
                        ost.UpdateSingleExpressionIdParentWithId(Properties.Settings.Default.PathFile, SelectExpId, SelectNodeId);
                        labelTopicOptionExp.Text = SelectNodeName;
                    }
                    else
                    {
                        MessageBox.Show("Нельзя назначить корень!", "Назначена корневая тема", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else //если вводим много выражений
                {
                    //if (SelectNodeId != "0")
                    //{
                    //    //Ostarbeiter ost = new Ostarbeiter();
                    //    //ost.UpdateSingleExpressionIdParentWithId(Properties.Settings.Default.PathFile, SelectExpId, SelectNodeId);
                    //    labelTopicOptionExp.Text = SelectNodeName;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Нельзя назначить корень!", "Назначена корневая тема", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                }
            }
            else
            {
                MessageBox.Show("Назначте тему!","Тема не назначена", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxExpOptionExp_MouseDown(object sender, MouseEventArgs e)
        {
            /*if (textBoxExpOptionExp.SelectedText.Length > 0)
            {
                textBoxExpOptionExp.DoDragDrop(textBoxExpOptionExp.SelectedText, DragDropEffects.Copy | DragDropEffects.Move);
            }*/
        }

        private void textBoxExpOptionExp_DragEnter(object sender, DragEventArgs e)
        {
            /*if (e.Data.GetDataPresent(DataFormats.FileDrop) &&
                ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move))

                e.Effect = DragDropEffects.Move;*/

            if(e.Data.GetDataPresent(DataFormats.Text))
            {
                if (((e.KeyState & 8) != 0) && ((e.AllowedEffect & DragDropEffects.Copy) != 0))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }


        }

        private void textBoxExpOptionExp_DragDrop(object sender, DragEventArgs e)
        {
            /*if (e.Data.GetDataPresent(DataFormats.FileDrop) && e.Effect == DragDropEffects.Move)
            {
                string[] objects = (string[])e.Data.GetData(DataFormats.FileDrop);
                // В objects хранятся пути к папкам и файлам
               textBoxExpOptionExp.Text = null;
                for (int i = 0; i < objects.Length; i++)
                    textBoxExpOptionExp.Text += objects[i] + "\r\n";
            }*/
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                textBoxExpOptionExp.Text = (string)e.Data.GetData(DataFormats.Text);
                /*if (((e.AllowedEffect & DragDropEffects.Copy) == 0) || ((e.KeyState & 8) == 0))
                {
                    textBoxExpOptionExp.Text = "";
                }*/
            }
        }

        #region Phonetic Keyboard

        private void buttonKeyboard4_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard5_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard6_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard7_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard8_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard9_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard10_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard11_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard12_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard13_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard14_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard15_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard16_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard17_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard18_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard19_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard21_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard22_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard23_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard24_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard25_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard26_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard27_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard28_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard29_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard30_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard31_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard32_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard33_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard34_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard35_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard36_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard37_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard38_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard39_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard40_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard41_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard42_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard43_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            flagKeyboard = false;
            keyTimer = true;
            timerKeyboard.Start();
        }

        private void timerKeyboard_Tick(object sender, EventArgs e)
        {
            if (keyTimer)
            {
                AddS(10);
            }
            else
            {
                timerKeyboard.Stop();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!flagKeyboard)
            {
                panelKeyboard.Visible = true;
                flagKeyboard = true;
                keyTimer = true;
                button1.Text = "Фонетическая клавиатура <<<";
                timerKeyboard.Start();
            }
            else
            {
                flagKeyboard = false;
                keyTimer = true;
                button1.Text = "Фонетическая клавиатура >>>";
                timerKeyboard.Start();
            }
        }

        public void AddS(int x)
        {
            if ((flagKeyboard) && (keyTimer))
            {
                panelKeyboard.Width += x;
            }
            else
            {
                panelKeyboard.Width -= x;
            }

            if ((panelKeyboard.Width <= 5) || (panelKeyboard.Width >= 250))
            {
                keyTimer = false;
            }
            else
            {
                keyTimer = true;
            }

            if (panelKeyboard.Width <= 5)
            {
                panelKeyboard.Visible = false;
            }
        }

        /// <summary>
        /// вставляет фонетический символ в строку ввода выражения по положению курсора
        /// </summary>
        /// <param name="text"></param>
        private void insertPhoneticSymbolToTextBoxExp(string text)
        {
            cursorTextExp = textBoxExpOptionExp.SelectionStart;
            textBoxExpOptionExp.Text = textBoxExpOptionExp.Text.Insert(cursorTextExp, text);
            textBoxExpOptionExp.SelectionStart = cursorTextExp + text.Length;
        }

        private void buttonKeyboard1_Click_1(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);   
        }

        private void buttonKeyboard2_Click_1(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
            
        }

        private void buttonKeyboard3_Click_1(object sender, EventArgs e)
        {
           insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard20_Click_1(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard44_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard45_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard46_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard47_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }

        private void buttonKeyboard48_Click(object sender, EventArgs e)
        {
            insertPhoneticSymbolToTextBoxExp(((System.Windows.Forms.Button)sender).Text);
        }
#endregion


        private void treeViewOptionExp_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeViewOptionExp.DoDragDrop(e.Item, DragDropEffects.Move);
        }



        private void lstBxLabels_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
        }

        private void lstBxLabels_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode))) //если перемещаем другую тему
            {

                TreeNode droppedNode = (TreeNode)e.Data.GetData(typeof(TreeNode)); //получаем перемещаемый узел

                //проверка: 1) если это не текущий узел 2)если его уже нет в списке ссылок у текущего узла
                if (droppedNode.Name != SelectNodeId)
                {
                    Ostarbeiter ost = new Ostarbeiter();

                    List<string> listCurrentLabels = ost.GetTopicLabels(Properties.Settings.Default.PathFile, SelectNodeId);

                    if (!listCurrentLabels.Contains(droppedNode.Name))
                    {
                        //List<string> listLabels = new List<string>();

                        listCurrentLabels.Add(droppedNode.Name); //добавляем в список единственный элемент - id перемещаемого узла

                        ost.SetTopicLabels(Properties.Settings.Default.PathFile, SelectNodeId, listCurrentLabels);

                        ShowCurrentLabels();
                    }
                }
            }
        }

        private void ShowCurrentLabels()
        {
            Ostarbeiter ost = new Ostarbeiter();

            listTopicLabels.Clear();
            lstBxLabels.Items.Clear();

            listTopicLabels = ost.GetTopicLabels(Properties.Settings.Default.PathFile, SelectNodeId);

            string currentItem = "";


            foreach (string label in listTopicLabels)
            {
                if (!String.IsNullOrWhiteSpace(label))
                {
                    currentItem = ost.GetTopicTextWithId(Properties.Settings.Default.PathFile, label);
                    lstBxLabels.Items.Add(currentItem);

                }
            }
        }

        private void btnLabels_Click(object sender, EventArgs e)
        {
            using (FormSetLabels formSetLabels = new FormSetLabels())
            {

                formSetLabels.selectNodeName = SelectNodeId;
                formSetLabels.TreeFont = treeViewOptionExp.Font;
                formSetLabels.TreeColor = treeViewOptionExp.ForeColor;

                formSetLabels.ShowDialog();
            }

            ShowCurrentLabels();
        }

        private void lstBxLabels_DoubleClick(object sender, EventArgs e)
        {
            if (lstBxLabels.SelectedItem.ToString() != null)
            {
                CallRecursiveWithText(treeViewOptionExp, lstBxLabels.SelectedItem.ToString());

                labelTopicSetOptionSet.Text = lstBxLabels.SelectedItem.ToString();
                labelTopicSetOptionSet.ForeColor = Color.DarkGreen;

                //SelectNodeId = 
                //SelectNodeName = lstBxLabels.SelectedItem.ToString();
                //MessageBox.Show(SelectNodeId);

                if (SelectNodeId == "0")
                {
                    btnLabels.Enabled = false;
                    lstBxLabels.Enabled = false;
                }
                else
                {
                    btnLabels.Enabled = true;
                    lstBxLabels.Enabled = true;
                    ShowCurrentLabels();
                }
            }
        }


        private void NodeSelectRecursiveWithText(TreeNode treeNode, string nodeText)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                NodeSelectRecursiveWithText(tn, nodeText);

                if (tn.Text == nodeText)
                {
                    tn.EnsureVisible();
                    tn.ExpandAll();
                    //tn.BackColor = Color.Yellow;
                    //tn.ForeColor = Color.Black;
                    //SelectNode = tn;
                    SelectNodeId = tn.Name;
                    SelectNodeName = tn.Text;
                    return;
                }
            }
        }

        private void CallRecursiveWithText(TreeView treeView, string nodeText)
        {
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                NodeSelectRecursiveWithText(n, nodeText);
            }
        }






    }
}
