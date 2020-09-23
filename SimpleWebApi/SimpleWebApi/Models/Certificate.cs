using FluentNHibernate.Mapping;
using System;

namespace SimpleWebApi.Models
{
    public class Certificate
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Number { get; set; }
        public virtual DateTime? ExpirationDate { get; set; }

        public sealed class NHibernateMap : ClassMap<Certificate>
        {
            public NHibernateMap()
            {
                Table("Certificate");
                DynamicInsert();
                DynamicUpdate();
                Id(_ => _.Id)
                    .GeneratedBy.Native()
                    .Unique()
                    .Not.Nullable();
                Map(_ => _.Name);
                Map(_ => _.Number);
                Map(_ => _.ExpirationDate);
            }
        }
    }
}