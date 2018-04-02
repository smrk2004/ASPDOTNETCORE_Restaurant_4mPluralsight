using Microsoft.EntityFrameworkCore;
using OdeToFood.Models;

namespace OdeToFood.Data
{
	public class OdeToFoodDbContext : DbContext
    {
		public OdeToFoodDbContext(DbContextOptions options) : base(options)
		{
			// To configure the dbcontext to work w/ different dbs
			// DbContextOptions 'options' -> can specify what db to connect to 
			//							- includes info like connString

			// passing options along to 'base()' (base class constructor) ::
			//	=> takes care of look'g @ options + setting up proper db connectns
		}
		public DbSet<Restaurant> Restaurants { get; set; }
	}
}
