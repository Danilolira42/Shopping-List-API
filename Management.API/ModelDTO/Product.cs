using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.API.ModelDTO;

[Table("ProductsRegister")]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

}
