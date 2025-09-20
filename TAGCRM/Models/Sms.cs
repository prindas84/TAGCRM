namespace TAGCRM.Models
{
    public class SmsTemplate
    {
        public int Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string SendFrom { get; set; } = string.Empty;
        public string TemplateContent { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
    }
}