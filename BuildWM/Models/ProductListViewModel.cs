public class ProductListViewModel
{
    public List<Product> Products { get; set; } = new();
    public string? Query { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int Total { get; set; }
    public int TotalPages => PageSize <= 0 ? 1 : (int)System.Math.Ceiling((double)Total / PageSize);
}
