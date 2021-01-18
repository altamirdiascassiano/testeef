using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeef.Data;
using testeef.Models;
using System.Linq;

namespace testeef.Controllers{
    
    [ApiController]
    [Route("v1/products")]
    public class ProductController: ControllerBase{

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context){
            return await context.Products.Include(x => x.Category).ToListAsync();                   
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id){
            return await context.Products.Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync( x => x.Id == id);
        }

        [HttpGet]
        [Route("GetByCategory/{id:int}")]                                       
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id){
            return await context.Products.Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync();
        }        

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context,[FromBody] Product model){
            if(ModelState.IsValid){
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            return BadRequest(ModelState);
        } 
    }
}