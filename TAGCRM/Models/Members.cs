namespace TAGCRM.Models
{
    public class Member
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string MemberId { get; set; } = string.Empty;
        public string LegacyMemberID { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string LegalName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PrimaryContactId { get; set; }
        public int RelationshipManager { get; set; }
        public bool EmailSubscribed { get; set; }
        public int LegalStructure { get; set; }
        public string Abn { get; set; } = string.Empty;
        public int BuildingOrganisation { get; set; }
        public string LicenceNumber { get; set; } = string.Empty;
        public int EstimatingPackage { get; set; }
        public List<int> Regions { get; set; } = new();
        
        // Business address
        public string BusinessAddress { get; set; } = string.Empty;
        public string BusinessAddress2 { get; set; } = string.Empty;
        public string BusinessCity { get; set; } = string.Empty;
        public string BusinessState { get; set; } = string.Empty;
        public string BusinessPostCode { get; set; } = string.Empty;
        
        // Postal address
        public string PostalAddress { get; set; } = string.Empty;
        public string PostalAddress2 { get; set; } = string.Empty;
        public string PostalCity { get; set; } = string.Empty;
        public string PostalState { get; set; } = string.Empty;
        public string PostalPostCode { get; set; } = string.Empty;
        
        // Online presence
        public string Website { get; set; } = string.Empty;
        public string Facebook { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string Linkedin { get; set; } = string.Empty;
        public string Tiktok { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        
        // Membership details
        public int ReferredBy { get; set; }
        public int Category { get; set; }
        public int Class { get; set; }
        public string MembershipStart { get; set; } = string.Empty;
        public string MembershipEnd { get; set; } = string.Empty;
        public decimal MembershipValue { get; set; }
        public int PaymentMethod { get; set; }
    }
}