using Microsoft.AspNetCore.Mvc;
using OdeToFood.Models;
using OdeToFood.Services;
using OdeToFood.ViewModels;

namespace OdeToFood.Controllers
{
	public class HomeController : Controller
   //public class HomeController
    {
		private IRestaurantData _restaurantData;
		private IGreeter _greeter;

		public HomeController(IRestaurantData restaurantData,
							  IGreeter greeter)
		{
			_restaurantData = restaurantData;
			_greeter = greeter;
		}

		public IActionResult Index()	// IActionResult implementer is invoked & executed at runtime; it is not built beforehand;
										//												but specific implem tells MVC what to do; later in pipeline
										//	Does NOT immediately write to response, IMPORTANT in context of building unit tests and such
										//				CONCEPT: separate what to do, from when to do it (WHAT: return content, WHEN: on demand, say at request receive/runtime, or earlier during a test)
										//						-> Important, because provides additional flexibility! 
										//							Example, can test HomeController w/o setting up webserver or having network comm happen
		//public ContentResult Index()
		//public string Index()
		{
			var model = new HomeIndexViewModel();
				model.Restaurants = _restaurantData.GetAll();
				model.CurrentMessage = _greeter.MessageOfTheDay();

						//new Restaurant() { Id = 1, Name = "Mohan's Pizzarama" };

			return View(model);	//Since no view-name param passed in, MVC assumes we want view, w/ same name as action!
								//					+ passes in above model data
			//return View();	//Since no view-name param passed in, MVC assumes we want view, w/ same name as action!
			//return View("Home");	// Specific view page 'Home.cshtml' requested; must live at Views/Home/Home.cshtml (or at) Views/Shared/Home.cshtml

			//return new ObjectResult(model);	// generate obj; something else down pipeline will decide what to do w/ obj...
			// default assumption in browser, is a JSON serialized   return

			//return Content("Hello from the Home Controller 2");

			/*
			return this.File("path_to_file", "plain/text").ToString();
			return this.BadRequest().ToString();
			*/
			//Eg: this.Response.Headers...
			// Here, after HomeContoller inherits from MVC 'Controller' class,
			//			the 'this.___' gets back many USEFUL contextual methods from Mvc Controller class, to do with REQ handling!, eg. name of action, controller currently in, current http context ('.Request' property); manip methods for '.Response' etc


			// Here, before HomeContoller inherits from MVC 'Controller' class,
			//			the 'this.___' gets back ONLY a few simple inherited methods from System.Object base class!
			//return "Hello from the HomeController!";
		}

		/// <summary>
		///																				// POST-REDIRECT-GET::STEP3/3 -> GET 'Details'
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Details(int id)
		{
			var model = _restaurantData.Get(id);
			if (model == null)
			{
				return  RedirectToAction(nameof(Index));
					  //RedirectToAction("Index"/*, Optional controllerName, if different one*/);
												// Returns 302 status code, indicating redirect to browser
												//		+ name of view to redir to
				//return NotFound();			//HTTP 404 return - use this for an API endpoint;
												//	avoid for website render REQ resp like this one!
				//return View("NotFound");		//explicit redirect to a view that handles NotFound
				//return Content("Not Found");	//Return string content "Not Found"
			}
			return View(model);
		}


		/// <summary>
		///			To View that collects FORM INPUT
		///																				// POST-REDIRECT-GET::STEP1/3 -> POST to the Other 'Create()'{the on that constructs, uses input model};
		///																																		happens in the view returned by this 'Create()'
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
		/// <summary>
		///			ENDPOINT that receives FORM Data POSTED 4m above view and puts in Input Model+/process I/P
		/// </summary>
		/// <returns></returns> 
		[HttpPost]
		public IActionResult Create(RestaurantEditModel model/* input model! */)
		{
			if (ModelState.IsValid)	//ModelState datastruct produced by MVC behind scenes, on rcv'g input model!
			{
				var newRestaurant = new Restaurant()
				{
					Name = model.Name,
					Cuisine = model.Cuisine
				};
				newRestaurant = _restaurantData.Add(newRestaurant);

				return RedirectToAction(nameof(Details), new { id = newRestaurant.Id });    // POST-REDIRECT-GET::STEP2/3 -> Redirect to ACTION 'Details'
			}
			else {
				return View();	// if model NOT valid;
								// re-present the view again 2 user! FOR valid re-Input
								//								& to correct their INFO
								// + NOTE: tagHelpers are going to work w/ ModelState
			}
			//return View("Details", newRestaurant);
			//return Content("POST-ed");
		}
	}
}
