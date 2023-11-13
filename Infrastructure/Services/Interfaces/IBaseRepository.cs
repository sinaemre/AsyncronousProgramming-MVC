using AsyncronousProgramming_MVC.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AsyncronousProgramming_MVC.Infrastructure.Services.Interfaces
{
    /*
     * Asenkron Programlama(Eş zamansız Programlama)
     * Bugüne kadar yaptığımız çalışmalarda senkron programlama(eş zamanlı programlama) yaptık. Bu yüzden bir iş(business) yapıldığında kullanıcı arayüz(UI-UserInterface) sadece yapılan bu işe bütün eforunu sarfetmekteydi. Örneğin bir web servisten data çekmek istiyorsunuz ve request attınız, response olarak gelen data'nın listelenmesi işlemine UI thread'i kitlendi. Böylelikle kullanıcı uygulamanın ona yan tarafta verdiği not tutma bölümünmü kullanamaz hale geldi. Senkron programlama burada yetersiz kaldı. Bizim problemimizi yani daya listelenirken arayüz üzerinde not tutma işini asenkron programlama ile yapabiliriz. Asenkron programlama aynı anda bir birinden bağımsız olarak işlemler yapmamızı temin edecektir. 
     */
    public interface IBaseRepository<T> where T : BaseEntity
    {
        //Bu projede elimizin asenkron programlamaya alışması için bütün methodlar asenron yazıcaz. Lakin Create, Update ve Delete işlemleri çok aksi bir business olmadığı sürece asenkron programlanmaz. Buna gerek yoktur. Bunun yanında bizim asıl odaklanmamız gereken nokta Read operasyonlarıdır. 
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

        //READ Operations
        Task<List<T>> GetByDefaults(Expression<Func<T, bool>> expression);
        Task<T> GetByDefault(Expression<Func<T, bool>> expression);
        Task<T> GetById(int id);
        Task<bool> Any(Expression<Func<T, bool>> expression);


        //Repository Design Patterns Filtered List Method
        //Filtreleme Methodu
        //SELECT, WHERE, ORDERBY, JOIN
        Task<List<TResult>> GetFilteredList<TResult>(
                                                    Expression<Func<T, TResult>> select,
                                                    Expression<Func<T, bool>> where = null,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                    Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null
                                                    );
    }
}
