using Microsoft.EntityFrameworkCore;
using StockControl.Core.Interfaces;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
  public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> where T : class
  {
    protected readonly AppDbContext _context = context;

    public void Create(T entity)
    {
      _context.Add(entity);
    }

    public void Delete(T entity)
    {
      _context.Remove(entity);
    }

    public void Update(T entity)
    {
      _context.Update(entity);
    }

    public IEnumerable<T> GetAll()
    {
      return _context.Set<T>().AsNoTracking();
    }
  }
}
