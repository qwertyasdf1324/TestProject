using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using System;

namespace SimpleWebApi.Models
{
    public class CompanyCertificate
    {
        public virtual int Id { get; set; }
        public virtual int CompanyId { get; set; }
        public virtual int CertificateId { get; set; }

        public sealed class NHibernateMap : ClassMap<CompanyCertificate>
        {
            public NHibernateMap()
            {
                Table("CompanyCertificate");
                DynamicInsert();
                DynamicUpdate();
                Id(_ => _.Id)
                    .GeneratedBy.Native()
                    .Unique()
                    .Not.Nullable();
                Map(_ => _.CompanyId);
                Map(_ => _.CertificateId);
            }
        }
    }
}
