using AsyncronousProgramming_MVC.Entities.Concrete;
using AsyncronousProgramming_MVC.Infrastructure.Services.Interfaces;
using AsyncronousProgramming_MVC.Models.DTO_s.ProductDTO_s;
using AsyncronousProgramming_MVC.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AsyncronousProgramming_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductRepository productRepo, ICategoryRepository categoryRepo, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetFilteredList
                (
                    select: x => new ProductVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Status = x.Status,
                        CategoryName = x.Category.Name,
                        UnitPrice = x.UnitPrice,
                        Image = x.Image
                    },
                    where: x => x.Status != Entities.Abstract.Status.Passive,
                    orderBy: x => x.OrderByDescending(z => z.CreatedDate),
                    join: x => x.Include(z => z.Category)
                );

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            //Ben listeden seçerken Id'lerine göre seçeceğim, ama sen kullanıcıya Name'lerini göstereksin!
            ViewBag.Categories = new SelectList
                (
                    await _categoryRepo.GetByDefaults(x => x.Status != Entities.Abstract.Status.Passive), "Id", "Name"
                );
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";

                if (model.UploadImage != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    imageName = $"{Guid.NewGuid()}_{model.UploadImage.FileName}"; //4589a-546asd_domates.png;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await model.UploadImage.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                var product = _mapper.Map<Product>(model);
                product.Image = imageName;
                await _productRepo.Add(product);
                TempData["Success"] = "Ürün eklendi!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Lütfen aşağıdaki kurallara uyunuz!";
            return View(model);
        }


    }
}
