using System;

namespace Infotecs.MiniJournal.Models
{
    public class Header
    {
        public Header(int id, string caption)
        {
            Id = id;
            Caption = caption;
        }

        public int Id { get; private set; }

        public string Caption { get; private set; }
    }
}
