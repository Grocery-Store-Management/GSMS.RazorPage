using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class CategoryBusinessEntity
    {
        private IUnitOfWork work;
        public CategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            IEnumerable<Category> categories = await work.Categories.GetAllAsync();
            categories = from category in categories
                         where category.IsDeleted == false
                         select category;
            return categories;
        }

        public async Task<Category> AddCategoryAsync(Category newCategory)
        {
            newCategory.Id = GsmsUtils.CreateGuiId();
            newCategory.IsDeleted = false;
            await work.Categories.AddAsync(newCategory);
            await work.Save();
            return newCategory;
        }
    }
}
