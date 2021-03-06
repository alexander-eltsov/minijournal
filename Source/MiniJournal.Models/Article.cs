﻿using System.Collections.Generic;

namespace Infotecs.MiniJournal.Models
{
    public class Article
    {
        public Article() : this(null)
        {
        }

        public Article(IList<Comment> comments)
        {
            Comments = comments ?? new List<Comment>();
        }

        public virtual int Id { get; set; }

        public virtual string Caption { get; set; }

        public virtual string Text { get; set; }

        public virtual IList<Comment> Comments { get; protected set; }

        public virtual void AddComment(Comment comment)
        {
            comment.Article = this;
            Comments.Add(comment);
        }
    }
}
