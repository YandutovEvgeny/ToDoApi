using System.Net;

namespace ToDoApi.Models
{
    public class PostToDoItemModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public ToDoItem todoItem { get; set; }
    }
}
