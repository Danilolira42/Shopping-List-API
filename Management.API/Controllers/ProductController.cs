using Management.API.ModelDTO;
using Management.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.API.Errors;
using ShoppingList.API.Services;
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
        var products = _db.Products.ToList();

        var exportToExcel = new ExportExcel();

        var content = exportToExcel.ExportToExcel(products);

        return File(
            content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
    }

    [HttpPost("RegisterNewProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        var validate = new RegisterProductsValidation(_db);

        product.RegistrationDate = DateTime.UtcNow;

        var result = validate.Validate(product);

        if (!result.IsValid)
        {
            var errorList = new RegisterProductsError
            {
                Errors = result.Errors.Select(e => e.ErrorMessage).ToList()
            };

            return StatusCode(400, errorList);
        }

        _db.Products.Add(product);

        _db.SaveChanges();

        return CreatedAtAction(nameof(CreateProduct), new { id = product.ProductId }, product);
    }

    [HttpPut("UpdateProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Product request)
    {
        var productsData = await _db.Products.FindAsync(request.ProductId);

        var validator = new UpdateProductsValidation();

        var validation = validator.Validate(request);

        if (productsData == null)
        {
            var errorList = new UpdateProductsError
            {
                Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()

            };
            return StatusCode(404, errorList);
        }

        if (!validation.IsValid)
        {
            var errorList = new UpdateProductsError
            {

                Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()

            };

            return StatusCode(400, errorList);
        }

        var lastProductName = productsData.ProductName;

        productsData.ProductName = request.ProductName;

        var firstLetter = productsData.ProductName[2];

       productsData.ProductName = productsData.ProductName.Replace(firstLetter.ToString(), firstLetter.ToString().ToUpper());

        productsData.RegistrationDate = DateTime.UtcNow;

        _db.SaveChanges();

        return StatusCode(200, $"Product: {lastProductName} updated successfully!");
    }

    [HttpDelete("DeleteProduct/{RequestID}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> DeleteById([FromRoute] int RequestID)
    {

        var product = await _db.Products.FindAsync(RequestID);

        if (product == null)
        {
            return StatusCode(404, "Product not found!");
        }
       
        var deletedProduct = product;

        _db.Products.Remove(product);

        _db.SaveChanges();

        return StatusCode(200, $"Product: {deletedProduct.ProductName} deleted successfully!");
    }

    [HttpDelete("DeleteAllProducts")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public IActionResult DeleteAll()
    {
        var products = _db.Products.Count();

        if (products <= 0)
        {
            return StatusCode(404, "Products not found!");
        }

        _db.Database.ExecuteSqlRaw("DELETE FROM ProductsRegister");

        return StatusCode(200, $"Products deleted successfully!");
    }

}