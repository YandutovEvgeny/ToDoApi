using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;
using ToDoApi.Services.ItemsService;

namespace ToDoApi.Controllers
{
    [Route("api/ToDoItems")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ItemsService _service;

        public ToDoItemsController(TodoContext context)
        {
            _service = new ItemsService(context);
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetToDoItems()
        {
            var getItems = await _service.GetToDoItems();
            switch (getItems.StatusCode)
            {
                case HttpStatusCode.NotFound: return NotFound();
                default: return new ActionResult<IEnumerable<TodoItemDTO>>(getItems.Items);
                //default: return CreatedAtAction(nameof(GetToDoItems), getItems);
            }
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetToDoItem(long id)
        {
            var getItem = await _service.GetToDoItem(id);
            switch (getItem.StatusCode)
            {
                case HttpStatusCode.NotFound: return NotFound();
                default: return getItem.TodoItem;
            }
        }

        // PUT: api/ToDoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(long id, TodoItemDTO todoItemDTO)
        {
            var statusCode = await _service.PutToDoItems(id, todoItemDTO);

            switch (statusCode)
            {
                case HttpStatusCode.BadRequest: return BadRequest();
                case HttpStatusCode.NotFound: return NotFound();
                default:  return NoContent();
            }
        }

        // POST: api/ToDoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostToDoItem(TodoItemDTO todoItemDTO)
        {
            var postToDoItemModel = await _service.PostToDoItems(todoItemDTO);
            switch (postToDoItemModel.StatusCode)
            {
                case HttpStatusCode.NotFound: return NotFound();
                default: return new ActionResult<ToDoItem>(postToDoItemModel.todoItem);
            }
        }

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(long id)
        {
            var statusCode = await _service.DeleteToDoItem(id);
            switch(statusCode) 
            { 
                case HttpStatusCode.NotFound: return NotFound();
                default: return NoContent();
            }
        }

        
    }
}
