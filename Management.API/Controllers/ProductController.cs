using FluentValidation;
using Management.API.ModelDTO;
using Management.API.Services;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.API.Errors;
using ShoppingList.API.Validation;

namespace Management.API.Controllers;
[ApiController]
[Route("api/Products")]
public class ProductController : ControllerBase
{
    private ProductDbContext _db;

    public ProductController(ProductDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    [HttpGet("GetAllProducts")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]

    public IActionResult GetAllProduct()
    {
        var product = _db.Products.ToList();

        return Ok(product);
    }

    [HttpPost("RegisterNewProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        var validate = new ProductsValidation();

        var result = validate.Validate(product);

        if(!result.IsValid)
        {

            var errors = new RequestErrorsProducts
            {
                Errors = result.Errors.Select(e => e.ErrorMessage).ToList()
            };
      
            return StatusCode(StatusCodes.Status400BadRequest, errors);
        }

        _db.Products.Add(product);
        
        _db.SaveChanges();

        return CreatedAtAction(nameof(CreateProduct), new { id = product.ProductId }, product);
    }

    [HttpPut("UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Product request)
    {
        var productsData = await _db.Products.FindAsync(request.ProductId);

        if (productsData == null)
        {
            return StatusCode(404, "Produto não encontrado!");
        }

        productsData.ProductName = request.ProductName;

        _db.SaveChanges();

        return StatusCode(200, "Produto atualizado com sucesso!");
    }

}