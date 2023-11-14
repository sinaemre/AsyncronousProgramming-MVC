using AsyncronousProgramming_MVC.Infrastructure.Services.Interfaces;
using AsyncronousProgramming_MVC.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsyncronousProgramming_MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetFilteredList
                (
                    select: x => new CategoryVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CreatedDate = x.CreatedDate,
                        Status = x.Status,
                        UpdatedDate = x.UpdatedDate
                    },
                    where: x => x.Status != Entities.Abstract.Status.Passive,
                    orderBy: x => x.OrderByDescending(z => z.CreatedDate));
            #region NOT
            //NOT:
            //Get Filtered methodunu yazmasaydık aşağıdaki uzun işlemlerin tamamını yapmak zorundaydık her seferind. Ama bu methodu serviste tanımlayarak controller'ın şişmesini engelledik ve class'ın içindeki işlemlerin class'ın içinde kalmasını sağladık.(Encapsülation)

            //var categoriesList = await _categoryRepo.GetByDefaults(x => x.Status != Entities.Abstract.Status.Passive);
            //List<CategoryVM> models = new List<CategoryVM>();
            //foreach (var category in categoriesList)
            //{
            //    var model = new CategoryVM
            //    {
            //        Id = category.Id,
            //        Name = category.Name,
            //        CreatedDate = category.CreatedDate,
            //        Status = category.Status,
            //        UpdatedDate = category.UpdatedDate
            //    };
            //    models.Add(model);
            //}
            //models.OrderByDescending(x => x.CreatedDate);
            #endregion
            
            return View(categories);
        }
    }
}
