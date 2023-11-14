using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AsyncronousProgramming_MVC.Models.DTO_s.CategoryDTO_s
{
    public class UpdateCategoryDTO
    {
        public int Id { get; set; }


        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmelisiniz!")]
        [RegularExpression(@"^[a-zA-Z- ]+$", ErrorMessage = "Sadece harf girebilirsiniz!")]//Kategori isimleri sadece küçübüyük harf ve boşluk karakteri kabul edilecek, semboller kabul edilmeyecek.
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
