using System.Collections;
using System.Collections.Generic;
using BankApp.Models;

namespace BankApp.Repository
{
    public interface IRepository<T> where  T : BaseEntity
    {
        void Add(T item);
        void Remove(int id);
        void Update(T item); 
        T FindByID(int id);
        T FindByEmail(string email);//vpizdu
        IEnumerable<T> FindALL();
    }
}