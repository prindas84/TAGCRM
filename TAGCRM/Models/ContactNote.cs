// Models/ContactNote.cs
namespace TAGCRM.Models
{
    public class ContactNote
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string StaffName { get; set; } = string.Empty;
    }
}