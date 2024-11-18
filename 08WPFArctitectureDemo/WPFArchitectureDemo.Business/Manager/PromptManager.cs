using Newtonsoft.Json;
using System.Xml;
using WPFArchitectureDemo.Business.DTO;
using WPFArchitectureDemo.Business.IManager;
using WPFArchitectureDemo.Data.IRepository;
using WPFArchitectureDemo.Data.RepositoryFactory;
using WPFArchitectureDemo.Domain.Models;

namespace WPFArchitectureDemo.Business.Manager
{
    public class PromptManager : IPromptManager
    {
        private readonly RepositoryFactory _repositoryFactory;

        public PromptManager(RepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public List<PromptDTO> Get()
        {
            var prompts = _repositoryFactory.GetPromptManager()
                .Get()
                .Select(p => ConvertToDTO(p))
                .ToList();

            return prompts;
        }

        public PromptDTO Add(string title, string content)
        {
            var newPrompt = new Prompt
            {
                Title = title,
                Content = content,
                CreateDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now
            };

            newPrompt = _repositoryFactory.GetPromptManager().Add(newPrompt);

            return ConvertToDTO(newPrompt);

        }

        public PromptDTO Update(PromptDTO prompt)
        {
            var existingPrompt = _repositoryFactory.GetPromptManager().Get(prompt.ID);
            if (existingPrompt != null)
            {
                existingPrompt.Title = prompt.Title;
                existingPrompt.Content = prompt.Content;
                existingPrompt.UpdateDateTime = DateTime.Now;
                _repositoryFactory.GetPromptManager().Update(existingPrompt);
            }
            return GetById(prompt.ID);
        }

        public PromptDTO Delete(long id)
        {
            _repositoryFactory.GetPromptManager().Delete(id);
            return GetById(id);
        }

        public PromptDTO Use(long promptId)
        {
            var newUsage = new PromptUsage
            {
                PromptID = promptId,
                CreateDateTime = DateTime.Now
            };
            var promptDTO = GetById(promptId);

            var newUsageDTO = new PromptUsageDTO
            {
                ID = newUsage.ID,
                PromptID = newUsage.PromptID,
                CreateDateTime = newUsage.CreateDateTime
            };
            promptDTO.Usages.Add(newUsageDTO);
            _repositoryFactory.GetPromptUsageManager().Add(newUsage);
            return GetById(promptId);
        }

        public PromptDTO GetById(long id)
        {
            var prompt = _repositoryFactory.GetPromptManager().Get(id);
            if (prompt != null)
            {
                return ConvertToDTO(prompt);
            }
            return null;
        }

        private PromptDTO ConvertToDTO(Prompt prompt)
        {
            return new PromptDTO
            {
                ID = prompt.ID,
                Title = prompt.Title,
                Content = prompt.Content,
                CreateDateTime = prompt.CreateDateTime,
                UpdateDateTime = prompt.UpdateDateTime,
                IsDelete = prompt.IsDelete,
                Usages = _repositoryFactory.GetPromptUsageManager().GetByForeignKey(prompt.ID).Select(u => new PromptUsageDTO
                {
                    ID = u.ID,
                    PromptID = u.PromptID,
                    CreateDateTime = u.CreateDateTime
                }).ToList()
            };
        }

        public string Export(string filePath)
        {
            var prompts = _repositoryFactory.GetPromptManager().Get().Where(x => !x.IsDelete);

            var json = JsonConvert.SerializeObject(prompts, Newtonsoft.Json.Formatting.Indented);

            var fileName = "prompts_"+DateTime.Now.ToString("yyyyMMddHHmmss")+".json";
            filePath = Path.Combine(filePath, fileName);
            File.WriteAllText(filePath, json);

            return filePath;
        }

        public bool Import(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var prompts = JsonConvert.DeserializeObject<List<Prompt>>(json);
                var allPrompts = _repositoryFactory.GetPromptManager().Get().Where(x => !x.IsDelete);
                foreach (var prompt in prompts)
                {
                    if (!allPrompts.Any(x => x.Title == prompt.Title && x.Content == prompt.Content))
                    {
                        _repositoryFactory.GetPromptManager().Add(prompt);
                    }
                }

                return true;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        public List<PromptDTO> Select(string filterSQL, string orderBySQL)
        {
            var prompts = _repositoryFactory.GetPromptManager().Select(filterSQL, orderBySQL);
            return prompts.Select(p => ConvertToDTO(p)).ToList();
        }

        public List<PromptDTO> FuzzySelect(string titleKey)
        {
            var filterSQL = $"Title like '%{titleKey}%' And IsDelete = 0";
            var prompts = _repositoryFactory.GetPromptManager().Select(filterSQL,"");
            return prompts.Select(p => ConvertToDTO(p)).ToList();
        }
    }
}
