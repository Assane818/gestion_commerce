
using Microsoft.EntityFrameworkCore;
using WebGesCommande.Data;

namespace WebGesCommande.Core.Impl;

public class Service<T> : IService<T> where T : class
{
    private readonly WebGesCommandeContext _context;

    public Service(WebGesCommandeContext context)
    {
        _context = context;
    }

    public async Task Delete(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        
    }

    public async Task Insert(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> SelectAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> SelectAllPaginate(int pageNumber, int limit)
    {
        return await _context.Set<T>().Skip((pageNumber - 1) * limit).Take(limit).ToListAsync();
    }

    public async Task<T?> SelectById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task Update(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }
}