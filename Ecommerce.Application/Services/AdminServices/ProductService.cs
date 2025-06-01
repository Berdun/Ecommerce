using Ecommerce.Application.ViewModels;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.AdminServices
{
    public class ProductService
    {
        private readonly IUnitOfWork _uow;
        private string _wwwRootPath;

        public ProductService(IUnitOfWork uow, IWebHostEnvironment env)
        {
            _uow = uow;
            _wwwRootPath = env.WebRootPath;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var productList = _uow.Product.GetAll();
            return productList;
        }

        public ProductVM GetProductVM(int? id)
        {
            var productVM = new ProductVM
            {
                Product = new Product(),
                CategoryList = _uow.Category.GetAll()
                .Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                })
            };

            if (id.HasValue && id > 0)
            {
                productVM.Product = _uow.Product
                    .GetFirstOrDefault(x => x.Id == id);
            }

            return productVM;
        }

        public void UpsertProduct(ProductVM productVM, IFormFile file)
        {
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploadRoot = Path.Combine(_wwwRootPath, "img", "products");
                var extension = Path.GetExtension(file.FileName);

                if (!string.IsNullOrEmpty(productVM.Product.Picture))
                {
                    var oldPicPath = Path.Combine(_wwwRootPath, productVM.Product.Picture);
                    if (File.Exists(oldPicPath))
                    {
                        File.Delete(oldPicPath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploadRoot, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVM.Product.Picture = Path.Combine(@"\img", "products", fileName + extension);
            }

            if (productVM.Product.Id <= 0)
            {
                _uow.Product.Add(productVM.Product);
            }
            else
            {
                _uow.Product.Update(productVM.Product);
            }
            _uow.Save();
        }

        public void Delete(int? id)
        {
            var product = _uow.Product.GetFirstOrDefault(x => x.Id == id);
            _uow.Product.Remove(product);
            _uow.Save();
        }


    }
}
