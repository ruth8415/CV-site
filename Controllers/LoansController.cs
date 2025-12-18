using Microsoft.AspNetCore.Mvc;
using GmachAPI.Entities;

namespace GmachAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoansController : ControllerBase
    {
        // Static list to act as a temporary database
        private static List<Loan> _loans = new List<Loan>
        {
            new Loan { Id = 1, ItemId = 2, BorrowerId = 1, LoanDate = DateTime.Now.AddDays(-7), DueDate = DateTime.Now.AddDays(7), Status = "active" },
            new Loan { Id = 2, ItemId = 1, BorrowerId = 2, LoanDate = DateTime.Now.AddDays(-14), DueDate = DateTime.Now.AddDays(-7), ReturnDate = DateTime.Now.AddDays(-5), Status = "returned" },
            new Loan { Id = 3, ItemId = 3, BorrowerId = 3, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(-3), Status = "overdue" }
        };
        private static int _nextId = 4;

        // GET: /loans
        // GET: /loans?status=active
        // GET: /loans?overdue=true
        [HttpGet]
        public ActionResult<List<Loan>> GetAll(string? status, bool? overdue)
        {
            var result = _loans.AsEnumerable();

            if (!string.IsNullOrEmpty(status))
            {
                result = result.Where(l => l.Status == status);
            }

            if (overdue == true)
            {
                result = result.Where(l => l.DueDate < DateTime.Now && l.ReturnDate == null);
            }

            return Ok(result.ToList());
        }

        // GET: /loans/1
        [HttpGet("{id}")]
        public ActionResult<Loan> GetById(int id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null)
            {
                return NotFound($"Loan with ID {id} not found");
            }
            return Ok(loan);
        }

        // POST: /loans
        [HttpPost]
        public ActionResult<Loan> Create([FromBody] Loan loan)
        {
            loan.Id = _nextId++;
            loan.LoanDate = DateTime.Now;
            loan.Status = "active";
            _loans.Add(loan);
            return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
        }

        // PUT: /loans/1
        [HttpPut("{id}")]
        public ActionResult<Loan> Update(int id, [FromBody] Loan updatedLoan)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null)
            {
                return NotFound($"Loan with ID {id} not found");
            }

            loan.ItemId = updatedLoan.ItemId;
            loan.BorrowerId = updatedLoan.BorrowerId;
            loan.DueDate = updatedLoan.DueDate;
            loan.Notes = updatedLoan.Notes;

            return Ok(loan);
        }

        // DELETE: /loans/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null)
            {
                return NotFound($"Loan with ID {id} not found");
            }

            _loans.Remove(loan);
            return NoContent();
        }

        // PUT: /loans/1/return - Close loan (register return)
        [HttpPut("{id}/return")]
        public ActionResult ReturnLoan(int id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null)
            {
                return NotFound($"Loan with ID {id} not found");
            }

            if (loan.Status == "returned")
            {
                return BadRequest($"Loan with ID {id} has already been returned");
            }

            loan.ReturnDate = DateTime.Now;
            loan.Status = "returned";

            return Ok(new 
            { 
                Message = "Item returned successfully", 
                Loan = loan 
            });
        }
    }
}
