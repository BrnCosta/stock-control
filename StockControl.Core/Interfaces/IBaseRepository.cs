using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces
{
  public interface IBaseRepository<T> where T : class
  {
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    IEnumerable<T> GetAll();
  }
}
