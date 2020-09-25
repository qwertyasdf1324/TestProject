using System;

namespace SimpleWebApi.BusinessLogicLayer.DTOs
{
    public class Certificate
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}