using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace IdeographicCreator
{
    class Ostarbeiter : DBWorker
    {
        /// <summary>
        /// возвращает таблицу всех выражений и соответствующих им тем
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <returns></returns>
        public DataTable GetDataTableFromDB(string dbName)
        {

            DataTable dt = new DataTable("From DB");
            List<Expressions> expressions = new List<Expressions>();
            List<Topics> topics = new List<Topics>();

            DataColumn columnId = new DataColumn("Id", typeof(int));
            DataColumn columnText = new DataColumn("Text", typeof(string));
            DataColumn columnIdTopic = new DataColumn("Topic", typeof(int));
            DataColumn columnTextTopic = new DataColumn("Topics", typeof(string));
            
            //добавляем столбцы в таблицу
            dt.Columns.AddRange(new DataColumn[] { columnId, columnText, columnIdTopic, columnTextTopic});

            //получаем список записей (выражений) из бд
            expressions = GetListExpressions(dbName);
            topics = GetListTopics(dbName);

            int i = 0;

            foreach (var exp in expressions)
            {
                //формируем строку
                DataRow row = dt.NewRow();
                row["Id"] = exp._id;
                row["Text"] = exp.ExText;
                row["Topic"] = exp.IdTopic;
                
                foreach (var top in topics)
                {
                    if (exp.IdTopic == top._id)
                    {
                        row["Topics"] = top.TopicText;
                        break;
                    }
                }
                         
                //добавляем строку в таблицу
                dt.Rows.Add(row);
            }

            return dt;
        }

        public DataTable GetDateTableWithId(string dbName, string idString)
        {
            DataTable dt = new DataTable("From DB");
            List<Expressions> expressions = new List<Expressions>();
            //идентификатор темы
            int idTop = 0;

            DataColumn columnId = new DataColumn("Id", typeof(int));
            DataColumn columnText = new DataColumn("Text", typeof(string));
            DataColumn columnIdTopic = new DataColumn("Topic", typeof(int));

            //добавляем столбцы в таблицу
            dt.Columns.AddRange(new DataColumn[] { columnId, columnText, columnIdTopic });

            idTop = Int32.Parse(idString);

            //получаем список записей (выражений) из бд
            expressions = GetListExpressionsWithIdTopic(dbName, idTop);

            foreach (var exp in expressions)
            {
                //формируем строку
                DataRow row = dt.NewRow();
                row["Id"] = exp._id;
                row["Text"] = exp.ExText;
                row["Topic"] = exp.IdTopic;

                //добавляем строку в таблицу
                dt.Rows.Add(row);
            }

            return dt;
        }

        public TreeNode GetTreeNodeTopicsAll(string dbName)
        {
            TreeNode root = new TreeNode("Topics");
            root.Name = "0";
            TreeNode child, child2, child3, child4, child5, child6, child7, child8, child9, child10, child11, child12;
            List<Topics> topics = new List<Topics>();
            

            topics = GetListTopics(dbName);

            try
            {
                foreach (Topics topCursor in topics)
                {
                    if (topCursor.IdParent == 0)
                    {
                        root.Nodes.Add(topCursor._id.ToString(), topCursor.TopicText);
                        child = root.Nodes[topCursor._id.ToString()];
                        //child.Nodes.Add(RecursionTree(child, topics, topCursor.IdParent)); 
                        foreach (Topics topCursor2 in topics)
                        {
                            if (topCursor._id == topCursor2.IdParent)
                            {
                                child.Nodes.Add(topCursor2._id.ToString(), topCursor2.TopicText);
                                child2 = child.Nodes[topCursor2._id.ToString()];
                                foreach (Topics topCursor3 in topics)
                                {
                                    if (topCursor2._id == topCursor3.IdParent)
                                    {
                                        child2.Nodes.Add(topCursor3._id.ToString(), topCursor3.TopicText);
                                        child3 = child2.Nodes[topCursor3._id.ToString()];
                                        foreach (Topics topCursor4 in topics)
                                        {
                                            if (topCursor3._id == topCursor4.IdParent)
                                            {
                                                child3.Nodes.Add(topCursor4._id.ToString(), topCursor4.TopicText);
                                                child4 = child3.Nodes[topCursor4._id.ToString()];
                                                foreach (Topics topCursor5 in topics)
                                                {
                                                    if (topCursor4._id == topCursor5.IdParent)
                                                    {
                                                        child4.Nodes.Add(topCursor5._id.ToString(), topCursor5.TopicText);
                                                        child5 = child4.Nodes[topCursor5._id.ToString()];
                                                        foreach (Topics topCursor6 in topics)
                                                        {
                                                            if (topCursor5._id == topCursor6.IdParent)
                                                            {
                                                                child5.Nodes.Add(topCursor6._id.ToString(), topCursor6.TopicText);
                                                                child6 = child5.Nodes[topCursor6._id.ToString()];
                                                                foreach (Topics topCursor7 in topics)
                                                                {
                                                                    if (topCursor6._id == topCursor7.IdParent)
                                                                    {
                                                                        child6.Nodes.Add(topCursor7._id.ToString(), topCursor7.TopicText);
                                                                        child7 = child6.Nodes[topCursor7._id.ToString()];
                                                                        foreach (Topics topCursor8 in topics)
                                                                        {
                                                                            if (topCursor7._id == topCursor8.IdParent)
                                                                            {

                                                                                child7.Nodes.Add(topCursor8._id.ToString(), topCursor8.TopicText);
                                                                                child8 = child7.Nodes[topCursor8._id.ToString()];
                                                                                foreach (Topics topCursor9 in topics)
                                                                                {
                                                                                    if (topCursor8._id == topCursor9.IdParent)
                                                                                    {

                                                                                        child8.Nodes.Add(topCursor9._id.ToString(), topCursor9.TopicText);
                                                                                        child9 = child8.Nodes[topCursor9._id.ToString()];
                                                                                        foreach (Topics topCursor10 in topics)
                                                                                        {
                                                                                            if (topCursor9._id == topCursor10.IdParent)
                                                                                            {

                                                                                                child9.Nodes.Add(topCursor10._id.ToString(), topCursor10.TopicText);
                                                                                                child10 = child9.Nodes[topCursor10._id.ToString()];
                                                                                                foreach (Topics topCursor11 in topics)
                                                                                                {
                                                                                                    if (topCursor10._id == topCursor11.IdParent)
                                                                                                    {

                                                                                                        child10.Nodes.Add(topCursor11._id.ToString(), topCursor11.TopicText);
                                                                                                        child11 = child10.Nodes[topCursor11._id.ToString()];
                                                                                                        foreach (Topics topCursor12 in topics)
                                                                                                        {
                                                                                                            if (topCursor11._id == topCursor12.IdParent)
                                                                                                            {

                                                                                                                child11.Nodes.Add(topCursor12._id.ToString(), topCursor12.TopicText);
                                                                                                                child12 = child11.Nodes[topCursor12._id.ToString()];
                                                                                                                foreach (Topics topCursor13 in topics)
                                                                                                                {
                                                                                                                    if (topCursor12._id == topCursor13.IdParent)
                                                                                                                    {

                                                                                                                        child12.Nodes.Add(topCursor13._id.ToString(), topCursor13.TopicText);
                                                                                                                        //child12 = child11.Nodes[topCursor12._id.ToString()];
                                                                                                                        //foreach (Topics topCursor13 in topics)
                                                                                                                        //{


                                                                                                                        //}
                                                                                                                    }

                                                                                                                }
                                                                                                            }

                                                                                                        }
                                                                                                    }

                                                                                                }
                                                                                            }

                                                                                        }
                                                                                    }

                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
               
                   //child = root.Nodes.Add(topCursor.TopicText);
                   //child.Nodes.Add(topCursor.TopicText);
                   
                    
                }

                /*foreach (Topics topCursor in topics)
                {

                }*/


            }
            catch (Exception ex)
            {
                //логируем ошибку
                base.string_errors += ex.ToString() + " ";
                base.last_error = ex.ToString();
                do_log(">>>>>>>> Get Tree Node <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return root;
        }



        public TreeNode GetTreeNodeTopicsChecked(string dbName, List<string> idTopics)
        {
            TreeNode root = new TreeNode("Topics");
            root.Name = "0";
            int topicIdInt = 0;
            TreeNode child, child2, child3, child4, child5, child6, child7, child8, child9, child10, child11, child12;
            List<Topics> topics = new List<Topics>();

            foreach (string topicId in idTopics)
            {
                topicIdInt = Convert.ToInt32(topicId);
                //MessageBox.Show(topicId, "ost_foreach");
                topics.Add(GetSingleTopicWithId(dbName, topicIdInt));

            }
            //topics = GetListTopics(dbName);

            try
            {
                foreach (Topics topCursor in topics)
                {
                    if (topCursor.IdParent == 0)
                    {
                        root.Nodes.Add(topCursor._id.ToString(), topCursor.TopicText);
                        child = root.Nodes[topCursor._id.ToString()];
                        //child.Nodes.Add(RecursionTree(child, topics, topCursor.IdParent)); 
                        foreach (Topics topCursor2 in topics)
                        {
                            if (topCursor._id == topCursor2.IdParent)
                            {
                                child.Nodes.Add(topCursor2._id.ToString(), topCursor2.TopicText);
                                child2 = child.Nodes[topCursor2._id.ToString()];
                                foreach (Topics topCursor3 in topics)
                                {
                                    if (topCursor2._id == topCursor3.IdParent)
                                    {
                                        child2.Nodes.Add(topCursor3._id.ToString(), topCursor3.TopicText);
                                        child3 = child2.Nodes[topCursor3._id.ToString()];
                                        foreach (Topics topCursor4 in topics)
                                        {
                                            if (topCursor3._id == topCursor4.IdParent)
                                            {
                                                child3.Nodes.Add(topCursor4._id.ToString(), topCursor4.TopicText);
                                                child4 = child3.Nodes[topCursor4._id.ToString()];
                                                foreach (Topics topCursor5 in topics)
                                                {
                                                    if (topCursor4._id == topCursor5.IdParent)
                                                    {
                                                        child4.Nodes.Add(topCursor5._id.ToString(), topCursor5.TopicText);
                                                        child5 = child4.Nodes[topCursor5._id.ToString()];
                                                        foreach (Topics topCursor6 in topics)
                                                        {
                                                            if (topCursor5._id == topCursor6.IdParent)
                                                            {
                                                                child5.Nodes.Add(topCursor6._id.ToString(), topCursor6.TopicText);
                                                                child6 = child5.Nodes[topCursor6._id.ToString()];
                                                                foreach (Topics topCursor7 in topics)
                                                                {
                                                                    if (topCursor6._id == topCursor7.IdParent)
                                                                    {
                                                                        child6.Nodes.Add(topCursor7._id.ToString(), topCursor7.TopicText);
                                                                        child7 = child6.Nodes[topCursor7._id.ToString()];
                                                                        foreach (Topics topCursor8 in topics)
                                                                        {
                                                                            if (topCursor7._id == topCursor8.IdParent)
                                                                            {
                                                                                child7.Nodes.Add(topCursor8._id.ToString(), topCursor8.TopicText);
                                                                                child8 = child7.Nodes[topCursor8._id.ToString()];
                                                                                foreach (Topics topCursor9 in topics)
                                                                                {
                                                                                    if (topCursor8._id == topCursor9.IdParent)
                                                                                    {
                                                                                        child8.Nodes.Add(topCursor9._id.ToString(), topCursor9.TopicText);
                                                                                        child9 = child8.Nodes[topCursor9._id.ToString()];
                                                                                        foreach (Topics topCursor10 in topics)
                                                                                        {
                                                                                            if (topCursor9._id == topCursor10.IdParent)
                                                                                            {
                                                                                                child9.Nodes.Add(topCursor10._id.ToString(), topCursor10.TopicText);
                                                                                                child10 = child9.Nodes[topCursor10._id.ToString()];
                                                                                                foreach (Topics topCursor11 in topics)
                                                                                                {
                                                                                                    if (topCursor10._id == topCursor11.IdParent)
                                                                                                    {
                                                                                                        child10.Nodes.Add(topCursor11._id.ToString(), topCursor11.TopicText);
                                                                                                        child11 = child10.Nodes[topCursor11._id.ToString()];
                                                                                                        foreach (Topics topCursor12 in topics)
                                                                                                        {
                                                                                                            if (topCursor11._id == topCursor12.IdParent)
                                                                                                            {
                                                                                                                child11.Nodes.Add(topCursor12._id.ToString(), topCursor12.TopicText);
                                                                                                                child12 = child11.Nodes[topCursor12._id.ToString()];
                                                                                                                foreach (Topics topCursor13 in topics)
                                                                                                                {
                                                                                                                    if (topCursor12._id == topCursor13.IdParent)
                                                                                                                    {
                                                                                                                        child12.Nodes.Add(topCursor13._id.ToString(), topCursor13.TopicText);
                                                                                                                        //child13 = child12.Nodes[topCursor13._id.ToString()];

                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                }


            }
            catch (Exception ex)
            {
                //логируем ошибку
                base.string_errors += ex.ToString() + " ";
                base.last_error = ex.ToString();
                do_log(">>>>>>>> Get Tree Node <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return root;
        }

        /*public TreeNode RecursionTree(TreeNode root, List<Topics> topics, int parent)
        {
            TreeNode child;
            foreach (Topics topCursor in topics)
            {
                if(topCursor.Id == parent)
                {
                    child = root.Nodes[topCursor.Id.ToString()];
                    child.Nodes.Add(RecursionTree(child, topics, topCursor.IdParent));
                }
            }
            /*
             * 
             * 
             * void Visit(TreeNodeCollection nodes)
{
foreach (TreeNode SubNode in nodes)
{
//делаешь с узлом то, что тебе надо
Visit(SubNode.Nodes);
}
             





            return child;
        }*/



        /// <summary>
        /// функция вставляет одну строку в таблицу тем 
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="topicName">название темы</param>
        /// <param name="nodeName">имя выбранного узла (содержит идентификатор темы)</param>
        public int SetSingleTopic(string dbName, string topicName, string nodeName)
        {
            int id = 0;
            Topics topic = new Topics { TopicText = topicName, IdParent = Int32.Parse(nodeName), TopicLabels = "" };
            id = InsertSingleTopic(dbName, topic);
            return id;
        }


        /// <summary>
        /// функция вставляет добавляет одно выражение
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="expText">название темы</param>
        /// <param name="topicIdText">имя выбранного узла (содержит идентификатор темы)</param>
        public int SetSingleExpression(string dbName, string expText, string topicIdText)
        {
            int idExp = 0;
            int topicIdInt = Int32.Parse(topicIdText);

            Expressions exp = new Expressions { ExText = expText, IdTopic = topicIdInt };
            idExp = InsertSingleExpressionRetId(dbName, exp);
            return idExp;
        }

        /// <summary>
        /// добавляет список выражений
        /// </summary>
        /// <param name="dbName">база данных</param>
        /// <param name="expTexts">список выражений</param>
        /// <param name="topicIdText">идентификатор темы</param>
        public void SetListExpressions(string dbName, List<string> expTexts, string topicIdText)
        {
            int topicIdInt = Int32.Parse(topicIdText);
            List<Expressions> expressions = new List<Expressions>();
            Expressions addExp = new Expressions();
            //int i = 0;
            foreach (string exp in expTexts)
            {
                //addExp.ExText = exp;
                //addExp.IdTopic = topicIdInt;
                //expressions.Add(addExp);

                expressions.Add(new Expressions { ExText = exp, IdTopic = topicIdInt });
            }
            InsertListExpressions(dbName, expressions);
        }

        /// <summary>
        /// Изменяет название темы по полученному id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">id темы</param>
        /// <param name="newText">новое название</param>
        public void UpdateSingleTopicWithId(string dbName, string idText, string newText)
        {
            int idInt = Int32.Parse(idText);
            UpdateSingleTopic(dbName, idInt, newText);
        }

        /// <summary>
        /// Изменяет id родителя темы по полученному id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">id темы</param>
        /// <param name="newText">новое название</param>
        public void UpdatePerentIdTopicWithId(string dbName, string idText, string newIdParent)
        {
            int idInt = Int32.Parse(idText);
            int newIdParentInt = Int32.Parse(newIdParent);
            UpdateTopicParentId(dbName, idInt, newIdParentInt);
        }


        /// <summary>
        /// Возвращает название темы по заданному идентификатору
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="idText">идентификатор</param>
        /// <returns>название темы</returns>
        public string GetTopicTextWithId(string dbName, string idText)
        {
            string TopicText = "";
            int idInt = Int32.Parse(idText);
            TopicText = GetNameTopicWithId(dbName, idInt);
            return TopicText;
        }

        /// <summary>
        /// Изменяет текст выражения по полученному id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">id выражения</param>
        /// <param name="newText">новый текст</param>
        public void UpdateSingleExpressionWithId(string dbName, string idText, string newText)
        {
            int idInt = Int32.Parse(idText);
            UpdateSingleExpression(dbName, idInt, newText);
        }

        public void UpdateParentIdListExpressionsWithParentId(string dbName, string idParent, string newIdParent)
        {
            int idParentInt = Int32.Parse(idParent);
            int newIdParentInt = Int32.Parse(newIdParent);
            UpdateListExpressionsIdParent(dbName, idParentInt, newIdParentInt);
        }

        /// <summary>
        /// Изменяет тему выражения по полученному id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">id выражения</param>
        /// <param name="newTIdTopic"></param>
        public void UpdateSingleExpressionIdParentWithId(string dbName, string idText, string newIdTopic)
        {
            int idInt = Int32.Parse(idText);
            int newIdTopicInt = Int32.Parse(newIdTopic);
            UpdateSingleExpressionIdParent(dbName, idInt, newIdTopicInt);
        }

        public void UpdateListExpressionsIdParentWithId(string dbName, List<int> listIdExp, string newIdTopic)
        {
            int newIdTopicInt = Int32.Parse(newIdTopic);
            UpdateListExpressionsIdParentWithId(dbName, listIdExp, newIdTopicInt);
        }




        /// <summary>
        /// удаляет выбранную тему из базы данных
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="nodeName">имя выбранного узла (содержит идентификатор темы)</param>
        public void DeleteSingleTopic(string dbName, string nodeName)
        {
            Topics topic = new Topics { _id = Int32.Parse(nodeName) }; //////////////////////////////!!!!! Id = ?
            base.DeleteSingleTopic(dbName, topic);
        }

        public void DeleteSingleExpression(string dbName, string idText)
        {
            int idInt = Int32.Parse(idText);
            DeleteSingleExpressionWithId(dbName, idInt);
        }

        public void DeleteSingleExpressionName(string dbName, string expText)
        {
            DeleteSingleExpressionWithExpText(dbName, expText);
        }

        public void SetTopicLabels(string dbName, string idTopic, List<string> listLabels)
        {
            int idTopicInt = Int32.Parse(idTopic);

            string strLabels = "";

            strLabels = String.Join(",", listLabels);

            UpdateTopicLabels(dbName, idTopicInt, strLabels);

            //MessageBox.Show(strLabels);            
        }

        public List<string> GetTopicLabels(string dbName, string idTopic)
        {
            List<string> listLabels = new List<string>();

            string strLabels = "";

            int idTopicInt = Int32.Parse(idTopic);

            strLabels = GetTopicStrLabels(dbName, idTopicInt);

            listLabels = strLabels.Split(',').ToList();

            return listLabels;
        }

    }
}
