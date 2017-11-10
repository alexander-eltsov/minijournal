using FluentNHibernate.Mapping;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Dal.Mappings
{
    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("Articles");
            Id(x => x.Id, "[ID]");
            Map(x => x.Caption)
                .Length(255)
                .Not.Nullable();
            Map(x => x.Text)
                .Length(1024)
                .Nullable();
            HasMany(x => x.Comments)
                .Inverse()
                .Cascade.All();
        }
    }
}
