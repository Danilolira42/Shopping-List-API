using FluentValidation;
using Management.API.ModelDTO;
using Management.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ShoppingList.API.Validation
{
    public class RegisterProductsValidation : AbstractValidator<Product>
    {
        private readonly ProductDbContext _db;
        public RegisterProductsValidation(ProductDbContext db)
        {
            _db = db;

            var productsList = _db.Products.ToList();

            RuleFor(Product => Product.ProductName)
                .Must(name => !productsList.Any(productsList => productsList.ProductName == name))
                .WithMessage("This products already exists in the list!");

            var regex = new Regex(@"[!""#$%&()*+,\./:;<=>?@\[\]\\^_`{|}~]");

            RuleFor(Product => Product.ProductName)
                .Must(name => !regex.IsMatch(name))
                .WithMessage("The Product Name cannot has any special character!");
        }
    }
}
