using System;

namespace Infotecs.MiniJournal.Models
{
    public class Header
    {
        protected Header()
        {
        }
        public Header(int id, string caption)
        {
            Id = id;
            Caption = caption;
        }

        public virtual int Id { get; protected set; }

        public virtual string Caption { get; protected set; }
    }
}
