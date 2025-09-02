using System;

namespace DTI.Domain
{
    public class Lead
    {
        public int Id { get; set; }
        public string Category { get; set; } = "";
        public string ContactFirstName { get; set; } = "";
        public string ContactLastName { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Description { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsContacted { get; set; } = false;
        public string PhoneNumber { get; set; } = "";
        public decimal Price { get; set; } = 0m;
        public string Suburb { get; set; } = "";

        // Valor padrão para Status
        public string Status { get; set; } = "Pendente";
    }
}
