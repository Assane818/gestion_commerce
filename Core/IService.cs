namespace WebGesCommande.Core;

public interface IService<T>
{
    Task Insert(T entity);
    Task Update(T entity);
    Task<T?> SelectById(int id);
    Task<IEnumerable<T>> SelectAll();
    Task Delete(T entity);
    Task<IEnumerable<T>> SelectAllPaginate(int pageNumber, int limit);
}