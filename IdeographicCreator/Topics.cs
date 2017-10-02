using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace IdeographicCreator
{
    [Table("Topics")]
    class Topics
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int _id { get; set; }

        [MaxLength(300), NotNull]
        public string TopicText { get; set; }

        public int IdParent { get; set; }

        public string TopicLabels { get; set; }

        public override string ToString()
        {
            return TopicText;
        }
    }
}
