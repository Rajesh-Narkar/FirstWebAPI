using MayBatch1WebAPIProject.Data;
using MayBatch1WebAPIProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MayBatch1WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
            private readonly ApplicationDbContext db;
            private readonly IWebHostEnvironment env;
            public ProductController(ApplicationDbContext db, IWebHostEnvironment environment)
            {
                this.db = db;
                env = environment;
            }

        //public IActionResult Index()
        //{
        //    return View();
        //}

            [HttpPost]
            [Route("AddProductPart")]
            public IActionResult AddProduct([FromBody] Product p)
            {
                if (p == null)
                {
                    return BadRequest("Invalid product data.");
                }

                try
                {
                    db.products.Add(p);
                    db.SaveChanges();
                    return Ok("Product added successfully");
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"Error: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }

            
            [HttpGet]
            [Route("GetProductsPart")]
            public IActionResult GetAllProducts()
            {
                var data = db.products.ToList();
                return Ok(data);
            }

            [HttpDelete]
            [Route("DeleteProductsPart/{id}")]
            public IActionResult DeleteProduct(int id)
            {
                var data = db.products.Find(id);
                if (data != null)
                {
                    db.products.Remove(data);
                    db.SaveChanges();
                    return Ok("Product deleted successfully");
                }
                return NotFound("Product not found");
            }

            //[HttpPut]
            //[Route("UpdateProd/{id}")]
            //public IActionResult UpdateProduct(int id, Product updatedProduct)
            //{
            //    //db.Entry(p).State=EntityState.Modified;
            //    var product = db.products.Find(id);
            //    if (product != null)
            //    {
            //        product.Pname = updatedProduct.Pname;
            //        product.Pcat = updatedProduct.Pcat;
            //        product.Prize = updatedProduct.Prize;

            //        // Add other fields that need to be updated

            //        db.SaveChanges();
            //        return Ok("Product updated successfully");
            //    }
            //    return NotFound("Product not found");

            //}

            [HttpPut]
            [Route("UpdateProductsPart")]
            public IActionResult UpdateProduct(Product p)
            {
                //db.Entry(p).State = EntityState.Modified;
                db.products.Update(p);
                db.SaveChanges();
                return Ok("Products updated Successfully");


            }

            [HttpGet]
            [Route("GetByIdPart/{id}")]

            public IActionResult GetProductByID(int id)
            {
                var data=db.products.Find(id);
                return Ok(data);
            }

            [HttpGet]
            [Route("GetProdByNamePart/{name}")]
            public IActionResult GetProductByName(string name)
            {
                var product = db.products.FirstOrDefault(p => p.Pname == name);
                if (product == null)
                {
                    return NotFound("Product not found");
                }

                return Ok(product);
            }

            [HttpGet]
            [Route("GetFiveProductsPart")]
            public IActionResult GetFirstProducts()
            {
                var data = db.products.Take(5).ToList();
                return Ok(data);
            }

            [HttpDelete]
            [Route("DeleteMulProdPart")]
            public IActionResult DeleteMultipleProducts(List<int> ids)
            {
                if (ids == null || ids.Count == 0)
                {
                    return BadRequest("No product IDs provided");
                }

                var productsToDelete = db.products.Where(p => ids.Contains(p.id)).ToList();

                if (productsToDelete.Count == 0)
                {
                    return NotFound("No products found with the provided IDs");
                }

                db.products.RemoveRange(productsToDelete);
                db.SaveChanges();

                return Ok("Products deleted successfully");
            }

            [HttpPost("upload")]
            public async Task<IActionResult> UploadFile(IFormFile file)
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { filePath = path });
            }

            [HttpGet("download/{fileName}")]
            public async Task<IActionResult> DownloadFile(string fileName)
            {
                var path = Path.Combine("uploads", fileName);

                if (!System.IO.File.Exists(path))
                    return NotFound("File not found.");

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, GetContentType(path), fileName);
            }

            private string GetContentType(string path)
            {
                var types = new Dictionary<string, string>
                {
                    { ".txt", "text/plain" },
                    { ".pdf", "application/pdf" },
                    { ".doc", "application/vnd.ms-word" },
                    { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                    { ".xls", "application/vnd.ms-excel" },
                    { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                    { ".png", "image/png" },
                    { ".jpg", "image/jpeg" },
                    { ".jpeg", "image/jpeg" },
                    { ".gif", "image/gif" },
                    { ".csv", "text/csv" }
                };

                var ext = Path.GetExtension(path).ToLowerInvariant();
                return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
            }
            


        }
    }



