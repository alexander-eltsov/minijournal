using FluentNHibernate.Mapping;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Dal.Mappings
{
    public class HeaderMap : ClassMap<Header>
    {
        public HeaderMap()
        {
            ReadOnly();
            Table("Articles");
            Id(x => x.Id, "[ID]");
            Map(x => x.Caption)
                .Length(255)
                .Not.Nullable();
        }
    }
}