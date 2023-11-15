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

        [HttpGet]
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
            ViewBag.Categories = new SelectList
               (
                   await _categoryRepo.GetByDefaults(x => x.Status != Entities.Abstract.Status.Passive), "Id", "Name"
               );
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

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productRepo.GetById(id);
            if (product != null)
            {
                var model = new UpdateProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    UnitPrice = product.UnitPrice,
                    Image = product.Image,
                    Categories = await _categoryRepo.GetByDefaults(x => x.Status != Entities.Abstract.Status.Passive)
                };

                return View(model);
            }
            TempData["Error"] = "Böyle bir kategori bulunamadı!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                string imageName = model.Image;

                if (model.UploadImage != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

                    if (!string.Equals(model.Image, "noimage.png"))
                    {
                        string oldPath = Path.Combine(uploadDir, model.Image);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    imageName = $"{Guid.NewGuid()}_{model.UploadImage.FileName}";
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await model.UploadImage.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                var product = _mapper.Map<Product>(model);
                product.Image = imageName;
                await _productRepo.Update(product);
                TempData["Success"] = "Ürün güncellendi!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Lütfen aşağıdaki kurallara uyunuz!";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id > 0)
            {
                var product = await _productRepo.GetById(id);
                if (product != null)
                {
                    await _productRepo.Delete(product);
                    TempData["Success"] = "Ürün silinmiştir!";
                    return RedirectToAction("Index");
                }
            }
            TempData["Error"] = "Ürün bulunamadı!";
            return RedirectToAction("Index");
        }

    }
}
