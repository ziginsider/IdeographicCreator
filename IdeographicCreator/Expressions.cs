using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace IdeographicCreator
{
    [Table("Expressions")]
    class Expressions
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [MaxLength(3000), NotNull]
        public string ExText { get; set; }

        public int IdTopic { get; set; }

        public override string ToString()
        {
            return ExText;
        }
    }
}
