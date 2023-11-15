using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncronousProgramming_MVC.Models.DTO_s.ProductDTO_s
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur!")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [Column(TypeName = "decimal(9,2)")] 
        public decimal UnitPrice { get; set; }

        public string? Image { get; set; }
        public IFormFile UploadImage { get; set; } //Resim yüklemek için kullanacağız

        [Required(ErrorMessage = "Kategori seçimi zorunludur!!")]
        public int CategoryId { get; set; }
    }
}
