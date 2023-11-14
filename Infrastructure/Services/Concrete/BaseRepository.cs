using AsyncronousProgramming_MVC.Entities.Abstract;
using AsyncronousProgramming_MVC.Infrastructure.Context;
using AsyncronousProgramming_MVC.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AsyncronousProgramming_MVC.Infrastructure.Services.Concrete
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _table;

        public BaseRepository(ApplicationDbContext context)
        {
            /*
             * Dependecy Injection
             * Eskiden bir sınıftan bir özellik kullanmak için o sınıfın new'lereyek nesne oluştururduk daha sonra o nesne üzerinde özllikleri kullanırdık. Bu yaptığımız class'lar arasında tight couple(sıkı bağlı) ilişk oluşturudu. Ayrıma memory yönetimi açısından sıkı sıkıya bağlı sınıfların maliyet(harcanılan vakit) oluşturduğunu ve RAM2in HEAP alanında yönetilemeyen kaynaklara neden olmaktadır. Sonuç olarak her sınıfın instance'ını çıkardığımızda bu nesnelerin yönetiminde projelerimiz büyüdükçe sıkıntılar yaşamaktayız. Bu yüzden projelerimizde bu tarz bağımlılıklara sebep olan sınıfları DIP ve IoC presipleribne uymak için Dependency Injection deseni kullanılarak gevşek bağlı(loose couple) hale getirmek istiyoruz. 
             * 
             * Inject ederken 3 farklı yol ile inject edebiliriz:
             * 1)Constructor Injection
             * 2)Custom Method Injection
             * 3)Property Injection
             * 
             * DIP => Dependency Inversion Bağımlıklıkların en aza indirilmesi
             * DI => Dependency Injection
             * 
             * DI bir prensip değildir. Hatta DIP ve IoC presiplerini uygulamamızda bize yardımcı olan bir araçtır. ASP .NET Core bu prensipleri projelerimizde rahatlıkla kullanmamız için dizayn edilmiştir.
             */

            _context = context;
            _table = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            entity.Status = Status.Modified;
            //_context.Entry<T>(entity).State = EntityState.Modified;
            _table.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            entity.DeletedDate = DateTime.Now;
            entity.Status = Status.Passive;
            _table.Update(entity);
            //_table.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression) => await _table.AnyAsync(expression);

        public async Task<T> GetByDefault(Expression<Func<T, bool>> expression) => await _table.FirstOrDefaultAsync(expression);

        public async Task<List<T>> GetByDefaults(Expression<Func<T, bool>> expression) => await _table.Where(expression).ToListAsync();

        public async Task<T> GetById(int id) => await _table.FirstOrDefaultAsync(x => x.Id == id && x.Status != Status.Passive);

        public async Task<List<TResult>> GetFilteredList<TResult>(
                                                Expression<Func<T, TResult>> select, 
                                                Expression<Func<T, bool>> where = null, 
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,                                 Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null)
        {
            IQueryable<T> query = _table;

            if (join != null)
            {
                query = join(query);
            }

            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(select).ToListAsync();
            }
            else
            {
                return await query.Select(select).ToListAsync();
            }
        }

        
    }
}
