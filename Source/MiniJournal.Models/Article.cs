using System;
using System.Collections.Generic;

namespace Infotecs.MiniJournal.Models
{
    public class Article
    {
        public Article()
        {
            Comments = new List<Comment>();
        }

        public int Id { get; set; }

        public string Caption { get; set; }

        public string Text { get; set; }

        public List<Comment> Comments { get; private set; }
    }
}
