using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFArchitectureDemo.Business.DTO;
using WPFArchitectureDemo.Domain.Models;

namespace WPFArchitectureDemo.Business.IManager
{
    public interface IPromptManager
    {
        PromptDTO GetById(long id);
        List<PromptDTO> Get();
        List<PromptDTO> Select(string filterSQL, string orderBySQL);
        List<PromptDTO> FuzzySelect(string titleKey);
        PromptDTO Add(string title, string content);
        PromptDTO Update(PromptDTO prompt);
        PromptDTO Delete(long id);
        PromptDTO Use(long promptId);
        public string Export(string filePath);
        public bool Import(string filePath);
    }
}
