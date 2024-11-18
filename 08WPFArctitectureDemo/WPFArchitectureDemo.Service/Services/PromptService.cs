using WPFArchitectureDemo.Business.DTO;
using WPFArchitectureDemo.Business.IManager;
using WPFArchitectureDemo.Domain.Models;
using WPFArchitectureDemo.Service.IService;

namespace WPFArchitectureDemo.Service.Services
{
    public class PromptService : IPromptService
    {
        private readonly IPromptManager _promptManager;

        public PromptService(IPromptManager promptManager)
        {
            _promptManager = promptManager;
        }

        public Result<PromptDTO> Create(string text, string content)
        {
            var promptDTO = _promptManager.Add(text, content);
            return Result<PromptDTO>.Success(promptDTO);
        }

        public Result<PromptDTO> Delete(long id)
        {
            var promptDTO = _promptManager.Delete(id);
            return Result<PromptDTO>.Success(promptDTO);
        }


        public Result<List<PromptDTO>> GetAll()
        {
            var promptDTOs = _promptManager.Get();
            return Result<List<PromptDTO>>.Success(promptDTOs);
        }

        public Result<PromptDTO> GetById(long id)
        {
            var promptDTO = _promptManager.GetById(id);
            return Result<PromptDTO>.Success(promptDTO);
        }


        public Result<PromptDTO> Update(PromptDTO prompt)
        {
            var promptDTO = _promptManager.Update(prompt);
            return Result<PromptDTO>.Success(promptDTO);
        }

        public Result<PromptDTO> Use(long promptId)
        {
            var promptDTO = _promptManager.Use(promptId);
            return Result<PromptDTO>.Success(promptDTO);
        }
        public Result<string> Import(string filePath)
        {
            var isSucess = _promptManager.Import(filePath);
            if (isSucess)
            {
                return Result<string>.Success("Sucess");
            }
            else
            {
                return Result<string>.Error("Error");
            }
        }
        public Result<string> Export(string filePath)
        {
            var promptDTO = _promptManager.Export(filePath);
            return Result<string>.Success(promptDTO);
        }

        public Result<List<PromptDTO>> Select(string filterSQL, string orderBySQL)
        {
            var promptDTOs = _promptManager.Select(filterSQL, orderBySQL);
            return Result<List<PromptDTO>>.Success(promptDTOs);
        }

        public Result<List<PromptDTO>> FuzzySelect(string keyWord)
        {
            var promptDTOs = _promptManager.FuzzySelect(keyWord);
            return Result<List<PromptDTO>>.Success(promptDTOs);
        }
    }
}
