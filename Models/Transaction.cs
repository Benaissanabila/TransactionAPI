namespace TransactionAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Client { get; set; } = "";
        public decimal Montant { get; set; }
        public DateTime Date { get; set; }
        public bool EstSuspecte { get; set; }
        public string Description { get; set; } = "";
    }
}
