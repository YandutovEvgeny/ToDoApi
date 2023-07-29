using System.Net;

namespace ToDoApi.Models
{
    public class GetToDoItemModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public TodoItemDTO TodoItem { get; set; }
    }
}
