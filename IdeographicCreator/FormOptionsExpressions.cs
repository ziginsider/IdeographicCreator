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
            }
            else
            {
                buttonChangeExpOptionExp.Location = new Point(22, 623);
                buttonFormSetOptionExpClose.Location = new Point(340, 623);
                labelSaveExpText.Location = new Point(165, 630);
                labelSaveExpText.Text = "Выражения не введены";
                buttonTopicSetOptionExp.Location = new Point(22, 568);
                button1.Location = new Point(295, 500);
                labelTopicSetOptionSet.Location = new Point(165, 574);
                label1.Location = new Point(18, 506);
                labelTopicOptionExp.Location = new Point(115, 506);
                buttonDeleteExp.Visible = false;
                //textBoxExpOptionExp.Size = new System.Drawing.Size(502, 438);
                textBoxExpOptionExp.Height = 438;
                labelExpOptionExp.Text = "Введите выражения (каждое новое с новой строки):";
                this.Height = 700;
                this.Width = 800;

                if (SelectExpParentId != "0")
                {
                    Ostarbeiter ost = new Ostarbeiter();
                    labelTopicOptionExp.Text = ost.GetTopicTextWithId(Properties.Settings.Default.PathFile, SelectExpParentId);
                    labelTopicSetOptionSet.Text = labelTopicOptionExp.Text;
                }
                else
                {
                    labelTopicOptionExp.Text = "Topics";
                    labelTopicSetOptionSet.Text = labelTopicOptionExp.Text;
                }
                labelTopicOptionExp.ForeColor = Color.DarkGreen;
                labelTopicSetOptionSet.ForeColor = Color.DarkGreen;
                SelectNodeId = SelectExpParentId;
                SelectNodeName = labelTopicOptionExp.Text;

            }

            textBoxExpOptionExp.Font = TextFont;
            textBoxExpOptionExp.ForeColor = TextColor;
            labelTopicSetOptionSet.Font = TextFont;
            labelTopicOptionExp.Font = TextFont;
            DrawAllTree();

            panelKeyboard.Visible = false;


        }

        private void DrawAllTree()
        {
            TreeNode root;//, child;
            Ostarbeiter ost = new Ostarbeiter();

            if (treeViewOptionExp.GetNodeCount(true) == 0)
            {
                root = ost.GetTreeNodeTopicsAll(Properties.Settings.Default.PathFile);
                treeViewOptionExp.Nodes.Add(root);
                treeViewOptionExp.ExpandAll();

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
                    if (SelectNodeId != "0")
                    {
                        //Ostarbeiter ost = new Ostarbeiter();
                        //ost.UpdateSingleExpressionIdParentWithId(Properties.Settings.Default.PathFile, SelectExpId, SelectNodeId);
                        labelTopicOptionExp.Text = SelectNodeName;
                    }
                    else
                    {
                        MessageBox.Show("Нельзя назначить корень!", "Назначена корневая тема", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
    }
}
