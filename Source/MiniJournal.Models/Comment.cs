namespace Infotecs.MiniJournal.Models
{
    public class Comment
    {
        public virtual int Id { get; set; }

        public virtual string User { get; set; }

        public virtual string Text { get; set; }

        public virtual Article Article { get; set; }
    }
}
