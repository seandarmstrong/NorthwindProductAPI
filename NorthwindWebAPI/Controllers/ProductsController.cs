using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NorthwindWebAPI.DAL;

namespace NorthwindWebAPI.Controllers
{
    public class ProductsController : ApiController
    {
        private NorthwindWebAPIModel db = new NorthwindWebAPIModel();

        // GET: api/Products
        public IQueryable<ProductViewModel> GetProducts()
        {
            return db.Products.ToList().Select(p => new ProductViewModel
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName
            }).AsQueryable();
        }

        public IQueryable<Product> GetProducts(string id)
        {
            var productId = Convert.ToInt32(Request.GetQueryNameValuePairs().Where(nv => nv.Key == "id").Select(nv => nv.Value)
                .FirstOrDefault());
            var productById = db.Products.Where(m => m.ProductID == productId);
            return productById;
        }

        public IQueryable<Product> GetProductsByCategoryId(string categoryid)
        {
            var categoryId = Convert.ToInt32(Request.GetQueryNameValuePairs().Where(nv => nv.Key == "categoryid").Select(nv => nv.Value)
                .FirstOrDefault());
            var productByCategoryId = db.Products.Where(m => m.CategoryID == categoryId);
            return productByCategoryId;
        }

        public IQueryable<Product> GetProductsBySupplierId(string supplierid)
        {
            var supplierId = Convert.ToInt32(Request.GetQueryNameValuePairs().Where(nv => nv.Key == "supplierid").Select(nv => nv.Value)
                .FirstOrDefault());
            var productBySupplierId = db.Products.Where(m => m.SupplierID == supplierId);
            return productBySupplierId;
        }

        public IQueryable<Product> GetProductsByMaxPrice(string maxprice)
        {
            var maxPrice = Convert.ToDecimal(Request.GetQueryNameValuePairs().Where(nv => nv.Key == "maxprice").Select(nv => nv.Value)
                .FirstOrDefault());
            var productByMaxPrice = db.Products.Where(m => m.UnitPrice <= maxPrice);
            return productByMaxPrice;
        }

        // GET: api/Products/5
        //[ResponseType(typeof(Product))]
        //public IHttpActionResult GetProduct(int id)
        //{
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(product);
        //}

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}