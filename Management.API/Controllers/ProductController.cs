using Management.API.ModelDTO;
using Management.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Management.API.Controllers;
[Route("ShoppingProducts")]
[ApiController]
public class ProductController : ControllerBase
{
    private ProductDbContext _db;

    public ProductController(ProductDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    [HttpGet]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]

    public IActionResult GetAllProduct()
    {
        var product = _db.Products.ToList();

        return Ok();
    }

}