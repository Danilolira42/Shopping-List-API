using FluentValidation;
using Management.API.ModelDTO;
using Management.API.Services;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.API.Validation
{
    public class UpdateProductsValidation : AbstractValidator<Product>
    {
        public UpdateProductsValidation()
        {

            var productsList = new ProductDbContext(new DbContextOptions<ProductDbContext>(), new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()).Products.ToList();

            RuleFor(Product => Product.ProductName)
               .Must(name => !productsList.Any(productsList => productsList.ProductName == name))
               .WithMessage("This products already exists in the list!");
           
            RuleFor(Product => Product.ProductName).NotEmpty().WithMessage("Product not found!");
        }
    }
}
