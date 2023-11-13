using AsyncronousProgramming_MVC.Entities.Concrete;
using AsyncronousProgramming_MVC.Infrastructure.Context;
using AsyncronousProgramming_MVC.Infrastructure.Services.Interfaces;

namespace AsyncronousProgramming_MVC.Infrastructure.Services.Concrete
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
