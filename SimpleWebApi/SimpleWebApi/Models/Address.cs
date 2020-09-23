using FluentNHibernate.Mapping;

namespace SimpleWebApi.Models
{
    public class Address
    {
        public virtual int Id { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual int ZipCode { get; set; }
        public virtual string Country { get; set; }

        public sealed class NHibernateMap : ClassMap<Address>
        {
            public NHibernateMap()
            {
                Table("Address");
                DynamicInsert();
                DynamicUpdate();
                Id(_ => _.Id)
                    .GeneratedBy.Native()
                    .Unique()
                    .Not.Nullable();
                Map(_ => _.Street);
                Map(_ => _.City);
                Map(_ => _.ZipCode);
                Map(_ => _.Country);
            }
        }
    }
}