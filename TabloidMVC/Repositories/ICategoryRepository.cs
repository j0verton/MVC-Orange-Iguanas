using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();

        void Add(Category category);

        public void Delete(int id);

        public Category getCategorybyId(int id);

        public void Edit(Category category);
    }
}