using AsyncronousProgramming_MVC.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncronousProgramming_MVC.Entities.Concrete
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //[Column(TypeName = "decimal(9,2)")]
        public decimal UnitPrice { get; set; }
        public string Image { get; set; }

        //Bu sütun database'e gitmemesi için NotMapped attribute'ünü ekledik.
        [NotMapped]
        public IFormFile UploadImage { get; set; }

        public int CategoryId { get; set; }

        //Product yüklenirken kategorininde database'den gelmesi için virtual olarak işaretliyoruz. Bu yükleme türüne LazyLoading denir. 
        public virtual Category Category { get; set; }
    }
}
