using Management.API.ModelDTO;
using Microsoft.EntityFrameworkCore;

namespace Management.API.Services;

public class ProductDbContext : DbContext
{

    private IConfiguration _configuration;
    
    public DbSet<Product> Products { get; set; }
   
    public ProductDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var typeDataBase = _configuration["TypeDataBase"];

        if (typeDataBase == null) {

            throw new ArgumentNullException("Erro ao realizar conexão!");
        }

        var connectionString = _configuration.GetConnectionString(typeDataBase);
        
        if (typeDataBase == "MySqlConnection")
        {
            
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
