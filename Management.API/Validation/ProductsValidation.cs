using FluentValidation;
using Management.API.ModelDTO;
using Management.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShoppingList.API.Validation
{
    public class ProductsValidation : AbstractValidator<Product>
    {

        public ProductsValidation()
        {

            var productsList = new ProductDbContext(new DbContextOptions<ProductDbContext>(), new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()).Products.ToList();

            RuleFor(Product => Product.ProductName).Must(name => !productsList.Any(productsList => productsList.ProductName == name)).WithMessage("Este produto já existe na lista!");
        }
    }
}
