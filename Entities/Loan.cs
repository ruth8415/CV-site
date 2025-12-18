namespace GmachAPI.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "active"; // active, returned, overdue
        public string Notes { get; set; } = string.Empty;
    }
}
