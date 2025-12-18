namespace GmachAPI.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = "available";
        public int Quantity { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
