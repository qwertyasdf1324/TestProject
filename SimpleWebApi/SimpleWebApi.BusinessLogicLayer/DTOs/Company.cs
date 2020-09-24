using System;
using System.Collections.Generic;

namespace SimpleWebApi.BusinessLogicLayer.DTOs
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public Address Address { get; set; }
        public IList<Certificate> Certificates { get; set; }

        public Company()
        {
            Certificates = new List<Certificate>();
        }
    }
}
