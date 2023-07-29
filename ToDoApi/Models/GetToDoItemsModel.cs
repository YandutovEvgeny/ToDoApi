using System.Net;

namespace ToDoApi.Models
{
    public class GetToDoItemsModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<TodoItemDTO> Items { get; set; }
    }
}
