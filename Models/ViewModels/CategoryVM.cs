using AsyncronousProgramming_MVC.Entities.Abstract;

namespace AsyncronousProgramming_MVC.Models.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
