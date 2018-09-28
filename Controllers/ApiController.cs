using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using WebAPIAssignment.Models;
using System.Linq;

namespace WebAPIAssignment.Controllers
{
    [Route("route/[controller]")]
    public class ApiController : Controller
    {
        private readonly ApiContext apiContext;

        public ApiController(ApiContext context)
        {
            apiContext = context;

            if (apiContext.ApiItems.Count() == 0)
            {
                
                //Add default value to the Model
                apiContext.ApiItems.Add(new ApiModel { FirstName = "Mickey", LastName = "Mouse", Gender = "Male", Age = 20});
                
                apiContext.SaveChanges();
            }
        }

        //Returns all the values corresponding to get
        [HttpGet]
        public IEnumerable<ApiModel> GetAll()
        {  
            return apiContext.ApiItems.ToList();
        }


        //Returns value specific to particular ID
        [HttpGet("{Id}", Name = "GetTodo")]
        public IActionResult GetById(long Id)
        {
            var item = apiContext.ApiItems.FirstOrDefault(t => t.Id == Id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        //Creates the value and stores it in model
        [HttpPost]
        public IActionResult Create([FromBody] ApiModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            apiContext.ApiItems.Add(item);
            apiContext.SaveChanges();

            return CreatedAtRoute("GetTodo", new { Id = item.Id }, item);
        }

        //Updates the item from model, when ID is passed from URL
        [HttpPut("{Id}")]
        public IActionResult Update(long Id, [FromBody] ApiModel item)
        {
            if (item == null || item.Id != Id)
            {
                return BadRequest();
            }

            var todo = apiContext.ApiItems.FirstOrDefault(t => t.Id == Id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.FirstName = item.FirstName;
            todo.LastName = item.LastName;
            todo.Gender = item.Gender;
            todo.Age = item.Age;

            apiContext.ApiItems.Update(todo);
            apiContext.SaveChanges();
            return new NoContentResult();
        }

        //Updates speific value based on ID and field passed in URL
        [HttpPatch("{Id}")]
        public IActionResult Patch(long Id, [FromBody] JsonPatchDocument<ApiModel> item)
        {

            var todo = apiContext.ApiItems.FirstOrDefault(t => t.Id == Id);
            if (todo == null)
            {
                return NotFound();
            }

            if(item == null){
               return BadRequest();
            }

            //Copy the value from source (item) to destination (todo).
            item.ApplyTo(todo); 

            apiContext.ApiItems.Update(todo);
            apiContext.SaveChanges();
            return new NoContentResult();
        }

        //Deletes the value based on ID passed from URL
        [HttpDelete("{Id}")]
        public IActionResult Delete(long Id)
        {
            var todo = apiContext.ApiItems.FirstOrDefault(t => t.Id == Id);
            if (todo == null)
            {
                return NotFound();
            }

            apiContext.ApiItems.Remove(todo);
            apiContext.SaveChanges();
            return new NoContentResult();
        }

    }
}