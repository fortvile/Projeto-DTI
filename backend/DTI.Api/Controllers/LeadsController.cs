using DTI.Domain;
using DTI.Infrastructure.Data;
using DTI.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DTI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly FakeEmailService _emailService;

        public LeadsController(MyDbContext context)
        {
            _context = context;
            _emailService = new FakeEmailService();
        }

        // GET /api/leads?status=Pendente|Aceito|Recusado
        [HttpGet]
        public async Task<IActionResult> GetLeads([FromQuery] string? status)
        {
            var query = _context.Leads.AsQueryable();
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(l => l.Status == status);

            var leads = await query.ToListAsync();
            return Ok(leads);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead([FromBody] Lead lead)
        {
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLeads), new { id = lead.Id }, lead);
        }

        // ✅ Accept
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptLead(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return NotFound();

            if (lead.Price > 500)
                lead.Price *= 0.9m; // desconto 10%

            lead.Status = "Aceito";
            await _context.SaveChangesAsync();

            // notificação fake
            _emailService.SendEmail("vendas@test.com", 
                "Novo Lead Aceito", 
                $"O lead {lead.ContactFirstName} {lead.ContactLastName} foi ACEITO.\nPreço final: {lead.Price}");

            return Ok(lead);
        }

        // ❌ Decline
        [HttpPut("{id}/decline")]
        public async Task<IActionResult> DeclineLead(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return NotFound();

            lead.Status = "Recusado";
            await _context.SaveChangesAsync();

            return Ok(lead);
        }
    }
}
