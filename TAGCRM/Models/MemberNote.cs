namespace TAGCRM.Models
{
    public class MemberNote
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string StaffName { get; set; } = string.Empty;
    }
}