using Microsoft.AspNetCore.Mvc;
using TransactionAPI.Models;

namespace TransactionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private static List<Transaction> _transactions = new List<Transaction>
        {
            new Transaction { Id = 1, Client = "Jean Tremblay", Montant = 150.00m, Date = DateTime.Now, EstSuspecte = false, Description = "Achat normal" },
            new Transaction { Id = 2, Client = "Marie Côté", Montant = 9500.00m, Date = DateTime.Now, EstSuspecte = true, Description = "Montant élevé suspect" },
            new Transaction { Id = 3, Client = "Pierre Roy", Montant = 75.50m, Date = DateTime.Now, EstSuspecte = false, Description = "Achat normal" }
        };

        [HttpGet]
        public ActionResult<List<Transaction>> GetAll()
        {
            return Ok(_transactions);
        }

        [HttpGet("{id}")]
        public ActionResult<Transaction> GetById(int id)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public ActionResult<Transaction> Create(Transaction transaction)
        {
            transaction.Id = _transactions.Count + 1;
            transaction.EstSuspecte = EstimeSuspecte(transaction);
            _transactions.Add(transaction);
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }

        [HttpPut("{id}")]
        public ActionResult<Transaction> Update(int id, Transaction transaction)
        {
            var existing = _transactions.FirstOrDefault(t => t.Id == id);
            if (existing == null)
                return NotFound();

            existing.Client = transaction.Client;
            existing.Montant = transaction.Montant;
            existing.Date = transaction.Date;
            existing.Description = transaction.Description;
            existing.EstSuspecte = EstimeSuspecte(transaction);

            return Ok(existing);
        }

        // Méthode privée — logique de fraude centralisée
        private bool EstimeSuspecte(Transaction transaction)
        {
            // Règle 1 — Montant élevé
            if (transaction.Montant > 5000)
                return true;

            // Règle 2 — Virement international
            if (transaction.Description.ToLower().Contains("international"))
                return true;

            // Règle 3 — Même client, 2 transactions en moins d'1 minute
            var transactionsRecentes = _transactions
                .Where(t => t.Client == transaction.Client
                    && t.Date > DateTime.Now.AddMinutes(-1))
                .Count();
            if (transactionsRecentes >= 2)
                return true;

            return false;
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
                return NotFound();

            _transactions.Remove(transaction);
            return NoContent();
        }
    }
}