using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Status;

namespace Domain.Repository
{
    public interface IRepository<T> 
           where T : class
    {
        StatusRequest Status { get;}
        List<string> ErrorMessage { get; }

        IEnumerable<T> GetList(); 
        T GetElement(int id); 
        void Create(T item);
        void Update(T item);
        void Delete(int id); 
    }
}
