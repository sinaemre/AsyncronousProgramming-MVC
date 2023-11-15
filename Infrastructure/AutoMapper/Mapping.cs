using AsyncronousProgramming_MVC.Entities.Concrete;
using AsyncronousProgramming_MVC.Models.DTO_s.CategoryDTO_s;
using AsyncronousProgramming_MVC.Models.DTO_s.ProductDTO_s;
using AutoMapper;

namespace AsyncronousProgramming_MVC.Infrastructure.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();

            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
        }
    }
}
