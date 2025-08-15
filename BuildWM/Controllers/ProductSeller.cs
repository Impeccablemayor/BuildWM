using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class ProductsController : Controller
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    // /Products?q=laptop&page=1
    public async Task<IActionResult> Index(string? q, int page = 1, int pageSize = 12)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 12;

        int skip = (page - 1) * pageSize;

        ProductResponse? resp;
        if (!string.IsNullOrWhiteSpace(q))
            resp = await _productService.SearchProductsAsync(q, pageSize, skip);
        else
            resp = await _productService.GetProductsAsync(pageSize, skip);

        var vm = new ProductListViewModel
        {
            Products = resp?.Products ?? new(),
            Query = q,
            Page = page,
            PageSize = pageSize,
            Total = resp?.Total ?? 0
        };

        return View(vm);
    }
}
