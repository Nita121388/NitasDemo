using WPFArchitectureDemo.Business.DTO;

namespace WPFArchitectureDemo.Service.IService
{
    public interface IPromptService
    {
        Result<List<PromptDTO>> GetAll();
        Result<List<PromptDTO>> Select(string filterSQL,string orderBySQL);
        Result<List<PromptDTO>> FuzzySelect(string keyWord);
        Result<PromptDTO> GetById(long id);
        Result<PromptDTO> Create(string text, string content);
        Result<PromptDTO> Update(PromptDTO prompt);
        Result<PromptDTO> Delete(long id);
        Result<PromptDTO> Use(long promptId);
        Result<string> Export(string filePath);
        Result<string> Import(string filePath);
    }
}
