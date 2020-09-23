using System;

namespace SimpleWebApi.BusinessLogicLayer.Entities
{
    public class Certificate
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Number { get; set; }
        public virtual DateTime? ExpirationDate { get; set; }
    }
}