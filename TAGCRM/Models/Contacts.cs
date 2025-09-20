namespace TAGCRM.Models
{
    public class Contact
    {
        // Required by pages/services
        public int Id { get; set; }
        public bool Active { get; set; } = true;
        // New split name fields
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        // Back-compat: many places still read/write Name (e.g., JSON, searches)
        public string Name { get; set; } = string.Empty;
        // Core contact info
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        // Membership / company (JSON uses string memberId)
        public string MemberId { get; set; } = string.Empty;
        // Filled at read-time from members.json; also shown in tables
        public string BusinessName { get; set; } = string.Empty;
        // Optional extras you asked for
        public string PreferredName { get; set; } = string.Empty;
        public string Pronounced { get; set; } = string.Empty;
        public string JobRole { get; set; } = string.Empty;
        public bool EmailSubscribed { get; set; } = true;
        public string Spouse { get; set; } = string.Empty;
        public string Children { get; set; } = string.Empty;
        // Keep as string to match your existing JSON like "09/03/1980"
        public string Birthday { get; set; } = string.Empty;
        public string Drink { get; set; } = string.Empty;
        public string Tools { get; set; } = string.Empty;
        public string Vehicle { get; set; } = string.Empty;
        public string Hobbies { get; set; } = string.Empty;
        public string Other { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;

        // Convenience: compute a display name without breaking legacy "Name"
        public string DisplayName
        {
            get
            {
                var first = (FirstName ?? "").Trim();
                var last = (Surname ?? "").Trim();
                var full = string.Join(" ", new[] { first, last }.Where(s => !string.IsNullOrEmpty(s)));
                return string.IsNullOrEmpty(full) ? (Name ?? "") : full;
            }
        }
        public bool PortalAccess { get; set; } = true;
        public bool BusinessDetails { get; set; } = true;
        public bool ContactsPermissions { get; set; } = true;
        public bool MemberSubscription { get; set; } = true;
        public bool InvoiceHistory { get; set; } = true;
    }
}
