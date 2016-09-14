using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using System.Windows.Forms;

namespace IdeographicCreator
{
    class DBWorker
    {
        protected string string_errors = "";
        protected string last_error = "";
        
        /// <summary>
        /// Логирование исключений 
        /// </summary>
        /// <param name="quer">исключение</param>
        protected void do_log(string quer)
        {

            if (quer.Length > 1000)
            {
                quer = quer.Substring(0, 1000);
            }
            File.AppendAllText("log.log", "\n[" + DateTime.Now.ToLongTimeString() + " "
                + DateTime.Now.ToLongDateString() + "][SQL error] " +
                this.last_error + " {QUERY} " + quer +
                "\n---------------------------------------------------------------\n " +
                "\n", System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Инициализация базы данных
        /// </summary>
        private void InitDb(string dbName)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.CreateTable<Expressions>();
                    db.CreateTable<Topics>();
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Connect <<<<<<<<<<");
                
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Начало работы с базой
        /// </summary>
        public void StartWorkDB(string dbName)
        {
            //если файла базы еще нет, то создаем его
            if (!File.Exists(Path.Combine(Application.StartupPath, dbName)))
            {
                InitDb(dbName);
            }
            else { MessageBox.Show("Дык ёсць жа база!", "Создание DB", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
            //работаем 

        }

        /// <summary>
        /// функция добавляет одну запись в таблицу
        /// </summary>
        /// <param name="dbName">Имя базы данных</param>
        /// <param name="express">Экземпляр выражения (строка таблицы Expressions)</param>
        public void InsertSingleExpression(string dbName, Expressions express)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.Insert(express);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Insert Single Expression<<<<<<<<<<");
                
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// функция добавляет одну тему в таблицу Topics
        /// </summary>
        /// <param name="dbName">Имя базы данных</param>
        /// <param name="Topic">Экземпляр класса тем (строка таблицы Topics)</param>
        public int InsertSingleTopic(string dbName, Topics topic)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.Insert(topic);
                    return topic.Id;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Insert Single Topic<<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return topic.Id;
        }



        /// <summary>
        /// функция добавляет одно выражение
        /// </summary>
        /// <param name="dbName">Имя базы данных</param>
        /// <param name="exp">Экземпляр класса выражений (строка таблицы Expressions)</param>
        public int InsertSingleExpressionRetId(string dbName, Expressions exp)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.Insert(exp);
                    return exp.Id;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Insert Single Expression<<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return exp.Id;
        }



        /// <summary>
        /// удаляет тему из таблицы тем
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="topic">экземпляр класса тем</param>
        public void DeleteSingleTopic(string dbName, Topics topic)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.Delete<Topics>(topic.Id);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Delete Single Topic <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Удаляет выражение по идентификатору
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">идентификатор</param>
        public void DeleteSingleExpressionWithId(string dbName, int id)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.Delete<Expressions>(id);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Delete Single Expressions <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        protected void DeleteSingleExpressionWithExpText(string dbName, string expText)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.Delete<Expressions>(expText);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Delete Single Expressions <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Изменяет название темы по полученному id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">id темы</param>
        /// <param name="newText">новое название</param>
        protected void UpdateSingleTopic(string dbName, int id, string newText)
        {
            Topics topic = new Topics();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    topic = db.Get<Topics>(id);
                    topic.TopicText = newText;
                    db.Update(topic);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Update Single Topic <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Изменяет id Родительского узла для данного узла по id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">идентификатор темы</param>
        /// <param name="newParentId">новый идентификатор родительской темы</param>
        protected void UpdateTopicParentId(string dbName, int id, int newParentId)
        {
            Topics topic = new Topics();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    topic = db.Get<Topics>(id);
                    topic.IdParent = newParentId;
                    db.Update(topic);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Update Parent Id Topic <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Изменяет в теме строку ссылок на другие темы
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">идентификатор темы</param>
        /// <param name="labels">строка со ссылками через запятую (ссылка - это id темы)</param>
        protected void UpdateTopicLabels(string dbName, int id, string labels)
        {
            Topics topic = new Topics();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    topic = db.Get<Topics>(id);
                    //topic.IdParent = newParentId;
                    topic.TopicLabels = labels;
                    db.Update(topic);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Update Labels Topic <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// возвращает строку со ссылками по id темы
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">идентификатор темы</param>
        protected string GetTopicStrLabels(string dbName, int id)
        {
            Topics topic = new Topics();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    topic = db.Get<Topics>(id);

                    return topic.TopicLabels;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Get Labels Topic <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return topic.TopicLabels;
        }

        /// <summary>
        /// возвращает название темы по заданному идентификатору темы
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="idTopic">идентификатор темы</param>
        /// <returns>название (имя) темы</returns>
        protected string GetNameTopicWithId(string dbName, int idTopic)
        {
            Topics topic = new Topics();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    topic = db.Get<Topics>(idTopic);
                    return topic.TopicText;
                    
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Get Name Topic <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return topic.TopicText;
        }


        /// <summary>
        /// Изменяет текст выражения по полученному id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">id выражения</param>
        /// <param name="newText">новый текст</param>
        protected void UpdateSingleExpression(string dbName, int id, string newText)
        {
            Expressions expression = new Expressions();
            //MessageBox.Show(id.ToString(), "Id");
            //MessageBox.Show(newText, "text");
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    expression = db.Get<Expressions>(id);
                    expression.ExText = newText;
                    db.Update(expression);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Update Single Expression <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Изменяет родительскую тему выражения по полученному id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">id выражения</param>
        /// <param name="newIdTopic">индекс новой темы</param>
        protected void UpdateSingleExpressionIdParent(string dbName, int id, int newIdTopic)
        {
            Expressions expression = new Expressions();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    expression = db.Get<Expressions>(id);
                    expression.IdTopic = newIdTopic;
                    db.Update(expression);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Update Single Expression Parent <<<<<<<<<<");

                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Изменяет идентификатор темы у списка выражений по полученному списку идентификаторов выражений
        /// </summary>
        /// <param name="dbName">база данных</param>
        /// <param name="listIdExp">список идентификаторов выражений которые подлежат изменению</param>
        /// <param name="idParentNew">новый идентификатор темы</param>
        public void UpdateListExpressionsIdParentWithId(string dbName, List<int> listIdExp, int idParentNew)
        {
            List<Expressions> expressions = new List<Expressions>();
            //MessageBox.Show("ddd");
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
    
                    //expressions = (from s in db.Table<Expressions>()
                    //               where s.IdTopic == idParent
                    //               select s).ToList();
                    foreach (int idExp in listIdExp)
                    {
                        expressions.Add(db.Get<Expressions>(idExp));
                    }

                    foreach (Expressions exp in expressions)
                    {
                        exp.IdTopic = idParentNew;
                    }

                    db.UpdateAll(expressions);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Update List Expressions Id Parent <<<<<<<<<<");
                //выводим сообщение
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Изменяет идентификатор темы у списка выражений по полученному старому идентификатору темы
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="idParent"></param>
        /// <param name="idParentNew"></param>
        public void UpdateListExpressionsIdParent(string dbName, int idParent, int idParentNew)
        {
            List<Expressions> expressions = new List<Expressions>();
            //MessageBox.Show("ddd");
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    //считываем данные из базы
                    //expressions = (from i in db.Table<Expressions>() select i).ToList();
                    //foreach (Expressions exp in expressions)
                    //{
                    //    if (exp.IdTopic == idParent)
                    //    {
                    //        exp.IdTopic = idParentNew;
                    //    }
                    //}

                    expressions = (from s in db.Table<Expressions>()
                            where s.IdTopic == idParent
                            select s).ToList();

                    foreach (Expressions exp in expressions)
                    {
                            exp.IdTopic = idParentNew;
                    }

                    db.UpdateAll(expressions);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Update List Expressions Id Parent <<<<<<<<<<");
                //выводим сообщение
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        /// <summary>
        /// Функция добавляет список выражений в таблицу
        /// </summary>
        /// <param name="dbName">Имя базы данных</param>
        /// <param name="expressions">Список персон</param>
        public void InsertListExpressions(string dbName, List<Expressions> expressions)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.InsertAll(expressions);
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Insert List Expressions <<<<<<<<<<");
                
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Функция возвращает выражение по указанному id
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="id">индекс выражения</param>
        /// <returns>экземпляр класса Expressions (строка таблицы Expressions)</returns>
        public Expressions GetSingleExpression(string dbName, int id)
        {
            Expressions expression = new Expressions();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    expression = db.Get<Expressions>(id);
                    return expression;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Get Single Expression <<<<<<<<<<");
                //выводим сообщение
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return expression;
        }

        /// <summary>
        /// Функция возвращает список всех выражений (строк таблицы Expressions)
        /// </summary>
        /// <param name="dbName">Ия базы данных</param>
        /// <returns>список всех выражений (строк таблицы Expressions)</returns>
        public List<Expressions> GetListExpressions(string dbName)
        {
            List<Expressions> expressions = new List<Expressions>();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    //считываем данные из базы
                    expressions = (from i in db.Table<Expressions>() select i).ToList();
                    return expressions;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Get List Expressions <<<<<<<<<<");
                //выводим сообщение
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return expressions;
        }


        public List<Expressions> GetListExpressionsWithIdTopic(string dbName, int idTopic)
        {
            List<Expressions> expressions = new List<Expressions>();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    //считываем данные из базы
                    //expressions = (from i in db.Table<Expressions>() select i).ToList();
                    expressions = db.Query<Expressions>("SELECT * FROM Expressions WHERE IdTopic = " + idTopic.ToString());
                    return expressions;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Get List Expressions With Id Topic <<<<<<<<<<");
                //выводим сообщение
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return expressions;
        }

        /// <summary>
        /// Функция возвращает строку в таблице тем по указанному имени
        /// </summary>
        /// <param name="dbName">имя базы данных</param>
        /// <param name="topicName">название темы</param>
        /// <returns>экземпляр класса Topics (строка таблицы Topics)</returns>
        public Topics GetSingleTopicWithName(string dbName, string topicName)
        {
            Topics topic = new Topics();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    //topic = db.Get<Topics>(topicName);
                    topic = db.ExecuteScalar<Topics>("SELECT * FROM Topics WHERE TopicText = " + topicName);
                    return topic;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Get Single Topic With Name <<<<<<<<<<");
                //выводим сообщение
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return topic;
        }


        public Topics GetSingleTopicWithId(string dbName, int topicId)
        {
            Topics topic = new Topics();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    topic = db.Get<Topics>(topicId);
                    //topic = db.ExecuteScalar<Topics>("SELECT * FROM Topics WHERE Id = " + topicId);
                    return topic;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Get Single Topic With Id <<<<<<<<<<");
                //выводим сообщение
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return topic;
        }

        /// <summary>
        /// Функция возвращает список всех тем (строк таблицы Topics)
        /// </summary>
        /// <param name="dbName">Ия базы данных</param>
        /// <returns>список всех тем (строк таблицы Topics)</returns>
        public List<Topics> GetListTopics(string dbName)
        {
            List<Topics> topics = new List<Topics>();
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, dbName);
                using (var db = new SQLiteConnection(dbPath))
                {
                    //считываем данные из базы
                    topics = (from i in db.Table<Topics>() select i).ToList();
                    return topics;
                }
            }
            catch (Exception ex)
            {
                //логируем ошибку
                this.string_errors += ex.ToString() + " ";
                this.last_error = ex.ToString();
                this.do_log(">>>>>>>> Get List Expressions <<<<<<<<<<");
                //выводим сообщение
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return topics;
        }


        
        /// <summary>
        /// //////////////////////////////////////////////////////////
        /// </summary>
        /*
                public void CopyBases()
                {
                    //создаем свою базу 
                    try
                    {
                        var dbPath = Path.Combine(Application.StartupPath, "Ideographic.db3");
                        using (var db = new SQLiteConnection(dbPath))
                        {
                            db.CreateTable<Expressions>();
                            db.CreateTable<Topics>();
                        }
                    }
                    catch (Exception ex)
                    {
                        //логируем ошибку
                        this.string_errors += ex.ToString() + " ";
                        this.last_error = ex.ToString();
                        this.do_log(">>>>>>>> Connect <<<<<<<<<<");

                        MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    //копируем из чужой в свою
                    List<Expressions> expressions = new List<Expressions>();
                    List<Expression> expList = new List<Expression>();
                    List<Topics> topics = new List<Topics>();
                    List<Topic> topList = new List<Topic>();
                    try
                    {
                        var dbPath = Path.Combine(Application.StartupPath, "DataBase.db3");
                        using (var db = new SQLiteConnection(dbPath))
                        {
                            //считываем данные из базы
                            expList = (from i in db.Table<Expression>() select i).ToList();
                            topList = (from j in db.Table<Topic>() select j).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        //логируем ошибку
                        this.string_errors += ex.ToString() + " ";
                        this.last_error = ex.ToString();
                        this.do_log(">>>>>>>> Get List Expressions <<<<<<<<<<");
                        //выводим сообщение
                        MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    int count = 0;
                    foreach (Expression expCursor in expList)
                    {
                        count = expCursor.id_parent - 4;
                        expressions.Add(new Expressions() { Id = expCursor.id_expression, IdTopic = count, ExText = expCursor.expression });
                    }

                    foreach (Topic topCursor in topList)
                    {
                        topics.Add(new Topics() { Id = topCursor.id_topic, IdParent = topCursor.id_parent, TopicText = topCursor.name });
                    }

                    try
                    {
                        var dbPath = Path.Combine(Application.StartupPath, "Ideographic.db3");
                        using (var db = new SQLiteConnection(dbPath))
                        {
                            db.InsertAll(expressions);
                            db.InsertAll(topics);
                        }
                    }
                    catch (Exception ex)
                    {
                        //логируем ошибку
                        this.string_errors += ex.ToString() + " ";
                        this.last_error = ex.ToString();
                        this.do_log(">>>>>>>> Insert List Expressions <<<<<<<<<<");

                        MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    MessageBox.Show("Wir haben es geschafft!", "Sein und Zeit", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }*/
    }
}
