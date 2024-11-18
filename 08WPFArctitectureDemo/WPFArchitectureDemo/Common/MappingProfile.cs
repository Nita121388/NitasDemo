using AutoMapper;
using System.Collections.ObjectModel;
using WPFArchitectureDemo.Business.DTO;
using WPFArchitectureDemo.Service.IService;
using WPFArchitectureDemo.UI.ViewModels;

namespace WPFArchitectureDemo.UI.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // PromptUsageViewModel => PromptUsageDTO
            CreateMap<PromptUsageViewModel, PromptUsageDTO>();

            //PromptUsageDTO => PromptUsageViewModel
            CreateMap<PromptUsageDTO, PromptUsageViewModel>();

            // PromptViewModel => PromptDTO
            CreateMap<PromptViewModel, PromptDTO>()
                .ForMember(
                dest => dest.Usages, 
                opt => opt.MapFrom(src => src.Usages.ToList()));

            CreateMap<PromptDTO, PromptViewModel>()
                .ForMember(
                dest => dest.Usages,
                opt => opt.MapFrom(src => src.Usages.ToList()));

        }
    }
}
