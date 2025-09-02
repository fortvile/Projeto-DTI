using DTI.Domain;
using DTI.Infrastructure.Data;

namespace DTI.Api
{
    public static class SeedData
    {
        public static void Initialize(MyDbContext context)
        {
            if (context.Leads.Any()) return;

            var leads = new List<Lead>
            {
                new Lead { Category = "Vendas", ContactFirstName = "Rafael", ContactLastName = "Pereira", DateCreated = DateTime.Now, Email = "rafael@email.com", IsContacted = false, PhoneNumber = "99999999", Price = 100, Suburb = "Centro" },
                new Lead { Category = "Suporte", ContactFirstName = "Ana", ContactLastName = "Silva", DateCreated = DateTime.Now, Email = "ana@email.com", IsContacted = false, PhoneNumber = "88888888", Price = 200, Suburb = "Jardim" }
            };

            context.Leads.AddRange(leads);
            context.SaveChanges();
        }
    }
}
