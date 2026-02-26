namespace ToDoWebAPIProject.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }

        public int Priority { get; set; }

    }
}
