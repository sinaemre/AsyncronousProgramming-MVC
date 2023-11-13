using AsyncronousProgramming_MVC.Entities.Abstract;

namespace AsyncronousProgramming_MVC.Entities.Concrete
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
