using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFArchitectureDemo.Data.Managers;
using WPFArchitectureDemo.Domain.Models;

namespace WPFArchitectureDemo.Business.DTO
{
    public class PromptDTO
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public List<PromptUsageDTO> Usages { get; set; }

    }
}
