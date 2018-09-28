using Microsoft.EntityFrameworkCore;

namespace WebAPIAssignment.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<ApiModel> ApiItems { get; set; }

    }
}