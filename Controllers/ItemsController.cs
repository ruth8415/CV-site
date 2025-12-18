using Microsoft.AspNetCore.Mvc;
using GmachAPI.Entities;

namespace GmachAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        // Static list to act as a temporary database
        private static List<Item> _items = new List<Item>
        {
            new Item { Id = 1, Name = "עגלת תינוק", Description = "עגלת תינוק במצב מצוין", Category = "תינוקות", Status = "available", Quantity = 2 },
            new Item { Id = 2, Name = "כיסא גלגלים", Description = "כיסא גלגלים מתקפל", Category = "רפואי", Status = "borrowed", Quantity = 1 },
            new Item { Id = 3, Name = "שולחן מתקפל", Description = "שולחן לאירועים", Category = "אירועים", Status = "available", Quantity = 5 }
        };
        private static int _nextId = 4;

        // GET: /items
        // GET: /items?status=available
        [HttpGet]
        public ActionResult<List<Item>> GetAll(string? status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                var filtered = _items.Where(i => i.Status == status).ToList();
                return Ok(filtered);
            }
            return Ok(_items);
        }

        // GET: /items/1
        [HttpGet("{id}")]
        public ActionResult<Item> GetById(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} not found");
            }
            return Ok(item);
        }

        // POST: /items
        [HttpPost]
        public ActionResult<Item> Create([FromBody] Item item)
        {
            item.Id = _nextId++;
            item.CreatedAt = DateTime.Now;
            _items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // PUT: /items/1
        [HttpPut("{id}")]
        public ActionResult<Item> Update(int id, [FromBody] Item updatedItem)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} not found");
            }

            item.Name = updatedItem.Name;
            item.Description = updatedItem.Description;
            item.Category = updatedItem.Category;
            item.Status = updatedItem.Status;
            item.Quantity = updatedItem.Quantity;

            return Ok(item);
        }

        // DELETE: /items/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} not found");
            }

            _items.Remove(item);
            return NoContent();
        }

        // GET: /items/1/availability - Check item availability
        [HttpGet("{id}/availability")]
        public ActionResult GetAvailability(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} not found");
            }

            return Ok(new 
            { 
                ItemId = item.Id, 
                ItemName = item.Name, 
                IsAvailable = item.Status == "available",
                Status = item.Status,
                AvailableQuantity = item.Quantity
            });
        }

        // GET: /items/1/loans - Loan history for a specific item
        [HttpGet("{id}/loans")]
        public ActionResult GetItemLoans(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} not found");
            }

            // This would normally query the loans, but since we don't have access 
            // to LoansController's list here, we return a placeholder message
            return Ok(new { Message = $"Loan history for item {id}", ItemName = item.Name });
        }
    }
}
