using AutoMapper;
using DevTools.data;
using DevTools.Dto.Category;
using DevTools.Dto.Plugins;
using Plugins.DevTool;

namespace DevTools.Helper.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // CreateMap<CreatePluginDTO, Plugin>();
            CreateMap<Plugin, PluginsResponeDTO>();


            CreateMap<IDevToolPlugin, CreatePluginDTO>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));


            CreateMap<PluginCategory, PLuginCategoryDTO>()
           .ForMember(dest => dest.plugins, opt => opt.MapFrom(src => src.Plugins));

            CreateMap<Plugin, PluginMinimize>();

            CreateMap<CreatePluginDTO, Plugin>()
    .ForMember(dest => dest.CategoryId, opt => opt.Ignore()) 
    .ForMember(dest => dest.AccessiableRoleId, opt => opt.Ignore()) 
    .ForMember(dest => dest.category, opt => opt.Ignore()); // Bỏ qua mapping cho category



        }
    }
}