using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WPFArchitectureDemo.Data.Managers;

namespace WPFArchitectureDemo.Business.DTO
{
    public class PromptUsageDTO
    {
        public long ID { get; set; }
        public long PromptID { get; set; }
        public PromptDTO Prompt { get; set; }
        public DateTime CreateDateTime { get; set; }
       
    }
}
