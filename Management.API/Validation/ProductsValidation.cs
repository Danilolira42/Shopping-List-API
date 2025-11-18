using FluentValidation;
using Management.API.ModelDTO;
using Management.API.Services;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.API.Validation
{
    public class ProductsValidation : AbstractValidator<Product>
    {

        public ProductsValidation()
        {
            RuleFor(Product => Product.ProductName);

            var productsList = new ProductDbContext(new DbContextOptions<ProductDbContext>(), new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()).Products.ToList();

            if (productsList != null && productsList.Count > 0)
            {
                RuleFor(Product => Product.ProductName).Equal(productsList[0].ProductName).Must(name =>
                {
                    foreach (var product in productsList)
                    {
                        if (product.ProductName == name)
                        {
                            return false;
                        }
                    }
                    return true;
                }).
                WithMessage("ProductName must be unique.");
            }
        }
    }
}
