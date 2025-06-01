using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.AdminServices
{
    public class CategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            IEnumerable<Category> categoryList = _uow.Category.GetAll();
            return categoryList;
        }

        public void CreateCategory(Category category)
        {
            _uow.Category.Add(category);
            _uow.Save();
        }

        public Category GetCategoryById(int? id)
        {
            Category category = _uow.Category.GetFirstOrDefault(x => x.Id == id);
            return category;
        }

        public void UpdateCategory(Category category)
        {
            _uow.Category.Update(category);
            _uow.Save();
        }

        public bool DeleteCategory(int? id)
        {
            var category = _uow.Category.GetFirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            _uow.Category.Remove(category);
            _uow.Save();
            return true;
        }



    }
}
