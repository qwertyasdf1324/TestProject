using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;

namespace SimpleWebApi.Models
{
    public class Company
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Website { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual Address Address { get; set; }
        public virtual IList<Certificate> Certificates { get; set; }

        public Company() 
        {
            Certificates = new List<Certificate>();
        }

        public sealed class NHibernateMap : ClassMap<Company>
        {
            public NHibernateMap()
            {
                Table("Company");
                DynamicInsert();
                DynamicUpdate();
                Id(_ => _.Id)
                    .GeneratedBy.Native()
                    .Unique()
                    .Not.Nullable();
                Map(_ => _.Name)
                    .Not.Nullable()
                    .Length(100);
                Map(_ => _.Website)
                    .Not.Nullable()
                    .Length(100);
                Map(_ => _.Description);
                Map(_ => _.CreateDate);
                References(_ => _.Address)
                    .Column("Address")
                    .Cascade
                    .All();
                HasManyToMany(_ => _.Certificates)
                    .ParentKeyColumn("CompanyId")
                    .ChildKeyColumn("CertificateId")
                    .Cascade
                    .All()
                    .Table("CompanyCertificate");
            }
        }
    }
}
