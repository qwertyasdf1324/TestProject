using System;

namespace SimpleWebApi.BusinessLogicLayer.DTOs
{
    public class Certificate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}