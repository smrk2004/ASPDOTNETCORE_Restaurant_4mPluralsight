using Microsoft.AspNetCore.Mvc;

namespace OdeToFood.Controllers
{
	// PATH starting with "/about" gets to this controller

	[Route("company/[controller]/[action]")]	// hoisted + combined token syntax; added arbitrary prefix
	/*
	[Route("[controller]")]	// tokenized this route with '[controller]' -> meaning IF controller name changed(to "about2"), this route would adapt to match new name!! - YASSS
	*/
	/*
	[Route("about")]	// This attribute => top-level attribute routing:
						//		Prefix "/about" gets here
						//			(or)
						//		route for this controller should start with "/about"
	*/
	public class About2Controller
    {
		/*
		[Route("")]		// This attribute, does HIERARCHICAL attribute routing:
						//		The "" means if no action specified -> resulting in route "/about" + ""; lands here / acts as default action route!
						// NOTE: however IF next method doesn't have routing-attribute that differentiates it's route from this one, results ambiguity & collision -> ERRORS!
		*/
		public string Phone()
		{
			return "1+461+461+4611";
		}

		/*
		[Route("[action]")]	// This attribute, does HIERARCHICAL attribute routing:
							// tokenized the route with '[action]' -> meaning if action name changed(to "address3"), this route would adapt to match new name!!-WOOT
		*/
		/*
		[Route("address")]	// Specificying a route different from above "" empty string path one, to differentiate!
		*/
		public string Address3()
		{
			return "USA";
		}
    }
}
