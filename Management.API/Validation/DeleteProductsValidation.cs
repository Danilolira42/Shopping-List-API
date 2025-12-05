using FluentValidation;
using Management.API.ModelDTO;
using Management.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ShoppingList.API.Validation
{
    public class DeleteProductsValidation : AbstractValidator<Product>
    {
        public DeleteProductsValidation()
        {

            var productsList = new ProductDbContext(new DbContextOptions<ProductDbContext>(), new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()).Products.ToList();

            RuleFor(Product => Product.ProductName).Matches(regex).WithMessage("The Product Name cannot has any special character!");
      
        }
    }
}
