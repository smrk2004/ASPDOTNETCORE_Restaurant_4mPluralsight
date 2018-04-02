using System;
using System.Collections.Generic;
using OdeToFood.Models;
using OdeToFood.Data;
using System.Linq;

namespace OdeToFood.Services
{
	public class SqlRestaurantData : IRestaurantData
	{
		public SqlRestaurantData(OdeToFoodDbContext context)
		{
			_context = context;
		}
		public Restaurant Add(Restaurant restaurant)
		{
				_context.Restaurants.Add(restaurant);

			//Insert will NOT occur until below stmt :: ->>
 				_context.SaveChanges();	// Perhaps for PROD, we might NOT call SaveChanges() every-time;
										//						but put into a commit() / saveChanges() / save()  method,
										//						& commit called after a batch of add(/update/remove) statements...& so on
										//
										// ON-success, Id prop gen by DB + put into existg 'restaurant' variable, auto-magically by EF!
			return restaurant;
		}

		public Restaurant Get(int id)
		{ 
			return _context.Restaurants.FirstOrDefault(r => r.Id == id);
		}

		public IEnumerable<Restaurant> GetAll()
		{
			return _context.Restaurants.OrderBy(r => r.Name);
					// IF real-life scenario, with Large # of Restaurants,
					//				DON'T return IEnumerable
					//
					//				=> Use 'IQueryable' instead!!!
		}

		private OdeToFoodDbContext _context;
	}
}
