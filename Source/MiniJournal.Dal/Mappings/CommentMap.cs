using FluentNHibernate.Mapping;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Dal.Mappings
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("Comments");
            Id(x => x.Id, "[ID]");
            Map(x => x.Text)
                .Length(512)
                .Not.Nullable();
            Map(x => x.User, "[User]")
                .Length(128)
                .Not.Nullable();
            References(x => x.Article)
                .Column("ArticleID")
                .Not.Nullable();
        }
    }
}
