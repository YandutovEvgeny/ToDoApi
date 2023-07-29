using Microsoft.EntityFrameworkCore;
using System.Net;
using ToDoApi.Models;

namespace ToDoApi.Services.ItemsService
{
    public class ItemsService
    {
        private readonly TodoContext _context;

        public ItemsService(TodoContext context)
        {
            _context = context;
        }

        public async Task<GetToDoItemsModel> GetToDoItems()
        {
            if (_context.ToDoItems == null)
            {
                return new GetToDoItemsModel { Items = null, StatusCode = HttpStatusCode.NotFound };
            }
            var dataList = await _context.ToDoItems.
                  Select(x => ItemToDTO(x)).ToListAsync();
            return new GetToDoItemsModel { Items = dataList, StatusCode = HttpStatusCode.OK };
        }

        public async Task<GetToDoItemModel> GetToDoItem(long id)
        {
            if (_context.ToDoItems == null)
            {
                return new GetToDoItemModel { TodoItem = null, StatusCode = HttpStatusCode.NotFound };
            }

            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return new GetToDoItemModel { TodoItem = null, StatusCode = HttpStatusCode.NotFound };
            }

            return new GetToDoItemModel { TodoItem = ItemToDTO(toDoItem), StatusCode = HttpStatusCode.OK };
        }

        public async Task<HttpStatusCode> PutToDoItems(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return HttpStatusCode.BadRequest;
            }

            var todoItem = await _context.ToDoItems.FindAsync(id);
            if (todoItem == null)
            {
                return HttpStatusCode.NotFound;
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
                return HttpStatusCode.NoContent;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
                {
                    return HttpStatusCode.NotFound;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<PostToDoItemModel> PostToDoItems(TodoItemDTO todoItemDTO)
        {
            if (todoItemDTO == null)
            {
                return new PostToDoItemModel { todoItem = null, StatusCode = HttpStatusCode.NotFound };
            }

            var todoItem = new ToDoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };
            _context.ToDoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return new PostToDoItemModel { todoItem = todoItem, StatusCode = HttpStatusCode.OK };
        }

        public async Task<HttpStatusCode> DeleteToDoItem(long id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return HttpStatusCode.NotFound;
            }

            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return HttpStatusCode.NoContent;
        }

        private bool ToDoItemExists(long id)
        {
            return (_context.ToDoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static TodoItemDTO ItemToDTO(ToDoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
