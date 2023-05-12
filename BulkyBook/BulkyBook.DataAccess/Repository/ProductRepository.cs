using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        public ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var productObj = _db.Products.FirstOrDefault(prod=>prod.Id==product.Id);
            if (productObj != null)
            {
                productObj.Title = product.Title;
                productObj.ISBN = product.ISBN;
                productObj.Price= product.Price;
                productObj.Price50= product.Price50;
                productObj.Price100= product.Price100;
                productObj.ListPrice= product.ListPrice;
                productObj.Description= product.Description;
                productObj.CategoryId= product.CategoryId;
                productObj.Author= product.Author;
                productObj.CoverTypeId= product.CoverTypeId;
                if (product.ImageUrl != null)
                {
                    productObj.ImageUrl = product.ImageUrl;
                }
                _db.Products.Update(productObj);
            }
            else
            {
                _db.Products.Add(product);
            }
        }
    }
}
