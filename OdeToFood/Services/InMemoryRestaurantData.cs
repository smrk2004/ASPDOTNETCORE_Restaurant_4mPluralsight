using OdeToFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Services
{
    public class InMemoryRestaurantData : IRestaurantData
	{
		public InMemoryRestaurantData()
		{
			_restaurants = new List<Restaurant>() {
				new Restaurant(){ Id = 1, Name = "Mohan's Pizzarama" },
				new Restaurant(){ Id = 2, Name = "Bajji's" },
				new Restaurant(){ Id = 3, Name = "Teja's Extravagance" },
			};
		}

		public IEnumerable<Restaurant> GetAll()
		{
			return _restaurants.OrderBy(r => r.Name);	// OrderBy = ascending default

			// NOTE: List is not thread-safe! And if we use in memory restaurant data, a single instance across multiple REQs -> MAY run into THREADING issues!
			//			Later address by talking to SQL server 4 this, and will have no threading issues
		}

		public Restaurant Get(int id)
		{
			return _restaurants.FirstOrDefault(r => r.Id == id);//First matching val
																//			OR
																//		default val for that type,
																//			restaurant is reference type,
																//				so default value = NULL; ret'd if NO id match!
							 //.Where(		   r => r.Id == id).FirstOrDefault();
		}

		public Restaurant Add(Restaurant restaurant)
		{
				restaurant.Id = _restaurants.Max(r => r.Id) + 1;
			   _restaurants.Add(restaurant);

			return restaurant;
		}

		// private fields go to bottom of class
		private List<Restaurant> _restaurants;
	}
}
