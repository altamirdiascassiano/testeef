using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeef.Data;
using testeef.Models;


namespace testeef.Controllers{
    
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController: ControllerBase{

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context){
            return await context.Categories.ToListAsync();       
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> Post([FromServices] DataContext context,[FromBody] Category model){
            if(ModelState.IsValid){
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            return BadRequest(ModelState);
        } 
    }
}