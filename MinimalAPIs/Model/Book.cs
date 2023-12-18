namespace MinimalAPIs.Model;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public required string Author { get; set; }
}
