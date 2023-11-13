using AsyncronousProgramming_MVC.Entities.Concrete;
using AsyncronousProgramming_MVC.Infrastructure.Context;
using AsyncronousProgramming_MVC.Infrastructure.Services.Interfaces;

namespace AsyncronousProgramming_MVC.Infrastructure.Services.Concrete
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
