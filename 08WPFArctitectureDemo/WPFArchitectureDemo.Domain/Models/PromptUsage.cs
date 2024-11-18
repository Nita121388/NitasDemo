namespace WPFArchitectureDemo.Domain.Models
{
    public class PromptUsage
    {
        public long ID { get; set; }
        public long PromptID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Prompt Prompt { get; set; }
    }
}
