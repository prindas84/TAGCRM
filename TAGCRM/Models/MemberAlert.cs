public class MemberAlert
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public int MemberId { get; set; }
    public string Alert { get; set; } = string.Empty;
    public string Start { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
    public int AlertType { get; set; }
}