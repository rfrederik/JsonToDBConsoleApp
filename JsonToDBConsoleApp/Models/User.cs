namespace JsonToDBConsoleApp.Models
{
    public class User
    {
        public string Id { get; set; } = new Guid().ToString();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Language { get; set; }
        public double? Version { get; set; }
    }
}