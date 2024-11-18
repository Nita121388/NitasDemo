namespace WPFArchitectureDemo.Domain.Models
{
    public class Prompt
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public ICollection<PromptUsage> PromptUsages { get; set; } = new List<PromptUsage>();
    }
}
