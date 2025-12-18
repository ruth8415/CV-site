using Microsoft.AspNetCore.Mvc;
using GmachAPI.Entities;

namespace GmachAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BorrowersController : ControllerBase
    {
        // Static list to act as a temporary database
        private static List<Borrower> _borrowers = new List<Borrower>
        {
            new Borrower { Id = 1, FirstName = "ישראל", LastName = "ישראלי", Phone = "050-1234567", Email = "israel@email.com", Address = "רחוב הרצל 10, תל אביב" },
            new Borrower { Id = 2, FirstName = "שרה", LastName = "כהן", Phone = "052-9876543", Email = "sarah@email.com", Address = "רחוב ויצמן 5, חיפה" },
            new Borrower { Id = 3, FirstName = "משה", LastName = "לוי", Phone = "054-5555555", Email = "moshe@email.com", Address = "רחוב בן גוריון 20, ירושלים" }
        };
        private static int _nextId = 4;

        // GET: /borrowers
        [HttpGet]
        public ActionResult<List<Borrower>> GetAll()
        {
            return Ok(_borrowers);
        }

        // GET: /borrowers/1
        [HttpGet("{id}")]
        public ActionResult<Borrower> GetById(int id)
        {
            var borrower = _borrowers.FirstOrDefault(b => b.Id == id);
            if (borrower == null)
            {
                return NotFound($"Borrower with ID {id} not found");
            }
            return Ok(borrower);
        }

        // POST: /borrowers
        [HttpPost]
        public ActionResult<Borrower> Create([FromBody] Borrower borrower)
        {
            borrower.Id = _nextId++;
            borrower.CreatedAt = DateTime.Now;
            borrower.IsActive = true;
            _borrowers.Add(borrower);
            return CreatedAtAction(nameof(GetById), new { id = borrower.Id }, borrower);
        }

        // PUT: /borrowers/1
        [HttpPut("{id}")]
        public ActionResult<Borrower> Update(int id, [FromBody] Borrower updatedBorrower)
        {
            var borrower = _borrowers.FirstOrDefault(b => b.Id == id);
            if (borrower == null)
            {
                return NotFound($"Borrower with ID {id} not found");
            }

            borrower.FirstName = updatedBorrower.FirstName;
            borrower.LastName = updatedBorrower.LastName;
            borrower.Phone = updatedBorrower.Phone;
            borrower.Email = updatedBorrower.Email;
            borrower.Address = updatedBorrower.Address;
            borrower.IsActive = updatedBorrower.IsActive;

            return Ok(borrower);
        }

        // DELETE: /borrowers/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var borrower = _borrowers.FirstOrDefault(b => b.Id == id);
            if (borrower == null)
            {
                return NotFound($"Borrower with ID {id} not found");
            }

            _borrowers.Remove(borrower);
            return NoContent();
        }

        // GET: /borrowers/1/history - Get borrower's loan history
        [HttpGet("{id}/history")]
        public ActionResult GetHistory(int id)
        {
            var borrower = _borrowers.FirstOrDefault(b => b.Id == id);
            if (borrower == null)
            {
                return NotFound($"Borrower with ID {id} not found");
            }

            // This would normally query the loans, but since we don't have access 
            // to LoansController's list here, we return a placeholder message
            return Ok(new { 
                Message = $"Loan history for borrower {id}", 
                BorrowerName = $"{borrower.FirstName} {borrower.LastName}" 
            });
        }

        // GET: /borrowers/1/loans - Get borrower's current loans
        [HttpGet("{id}/loans")]
        public ActionResult GetLoans(int id)
        {
            var borrower = _borrowers.FirstOrDefault(b => b.Id == id);
            if (borrower == null)
            {
                return NotFound($"Borrower with ID {id} not found");
            }

            return Ok(new { 
                Message = $"Current loans for borrower {id}", 
                BorrowerName = $"{borrower.FirstName} {borrower.LastName}" 
            });
        }
    }
}
