using AsyncronousProgramming_MVC.Entities.Concrete;
using AsyncronousProgramming_MVC.Models.DTO_s.CategoryDTO_s;
using AutoMapper;

namespace AsyncronousProgramming_MVC.Infrastructure.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
        }
    }
}
