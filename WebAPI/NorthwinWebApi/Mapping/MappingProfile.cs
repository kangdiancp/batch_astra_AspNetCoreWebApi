using AutoMapper;
using Northwind.Entities.Models;
using Northwind.Entities.DataTransferObject;

namespace NorthwinWebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //get category
            CreateMap<Category,CategoryDto>();
            
            //post categery
            CreateMap<CategoryDto, Category>();
        }
    }
}
