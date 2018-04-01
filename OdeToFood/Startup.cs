using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OdeToFood.Services;

namespace OdeToFood
{
    public class Startup
    {
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddSingleton<IRestaurantData, InMemoryRestaurantData>();	// USE BEC, Scoped is for single REQ only in context of in memory db we want same additive effect of a real db's transactions to be simulated when NEW restaurant data added
				  //.AddScoped<IRestaurantData, InMemoryRestaurantData>();  // NOTE: for NOW - NOT thread safe!
																			// AddScoped - create & use	 service instance once per every HTTP request/reused throughout that request then thrown away!
																			//	-> typically what we want for a data access component!

			// use to register services!
			services.AddSingleton<	IGreeter, Greeter>();	// AddSingleton - one persistent service instance for entire application
			//services.AddTransient<IGreeter, Greeter>();	// AddTransient - on demand		 service instance create & dispose
			//services.AddScoped<	IGreeter, Greeter>();	// AddScoped	- create & use	 service instance once per every HTTP request/reused throughout that request then thrown away! 
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app,
							  IHostingEnvironment env,
							  //IConfiguration configuration
							  IGreeter greeter,
							  ILogger<Startup> logger // A logger specifically configured for the startup class!
							  )
		{

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();    // this middle-ware is pass-through;
													// ONLY cares about response & Not incoming request;
													//	+ 'IFF' rest of APP has unhandled exception; prettifies & presents dev-dbg UI!
			}
			//else {
			//	app.UseExceptionHandler();	// USE generic 500 handler for PROD or other env
			//}

			app.UseStaticFiles();

			app.UseMvc(ConfigureRoutes);	// Add our own 'Custom routing initializer'-method call ON MVC setup!
			//app.UseMvc();					// Add MVC framework middleware! -> NO routes to Configure; Framework won't know how to route/map an incomoing REQ
			//app.UseMvcWithDefaultRoute();	// Add MVC framework middleware! w/ SET 'default' route, of template = '{controller=Home}/{action=Index}/{id?}'

			/*
				app.UseDefaultFiles();	// must go before use static files for loading index.html at default paths(app root "localhost:port") & not just for specific path
				app.UseStaticFiles(new StaticFileOptions(
					// can configure
				)); // To ALLOW loading files from 'wwwroot' folder!
			*/
			//app.UseFileServer();		// combines UseDefaultFiles() + UseStaticFiles() + additional options!
			/*

			//To install middle-ware, add
			//app.Use_[middlewarename]_

			// Example 2: even more custom/granular user-defined middleware!
			app.Use(next => {
				// middleware below invoked ONCE per HTTP request
				// middleware begin ----->
				return async context =>
				{
					logger.LogInformation("Request incoming myMiddleware");
					if (context.Request.Path.StartsWithSegments("/myMiddleware"))
					{
						await context.Response.WriteAsync("Hit my middleware!");
						logger.LogInformation("Request handled");
					}
					else
					{
						await next(context); // call next() passing context to next delegate in middleware pipeline!
						logger.LogInformation("Response outgoing");
					}
				};
				// middleware end   <-----|
			});

			// Example 1: custom middleware for welcome page
			app.UseWelcomePage(new WelcomePageOptions {
				Path = "/wp"
			});

			*/
			// Example 0 : our main middleware handler
			app.Run(async (context) =>
            {
				context.Response.ContentType = "text/plain"; // Explicitly specifying Content's MIME type
				await context.Response.WriteAsync($"NOT Found!");
				/*
					//throw new Exception("Error!!!");

					var greeting = greeter.MessageOfTheDay();
									//configuration["Greeting"];	// key value storage / access!

					await context.Response.WriteAsync($"{greeting} : {env.EnvironmentName}");
				*/
			});
        }

		private void ConfigureRoutes(IRouteBuilder routeBuilder)
		{
			/* CONVENTIONAL Template Based Routing! */

												// Use the routeBuilder -> '.Map___()' methods to setup routing!
			//		/Home/Index
			//	or	/Home/Index/4
			routeBuilder.MapRoute("Default","{controller=Home}/{action=Index}/{id?}");// MapRoute() takes friendlyName & template for the route

			//		/customRandomRoutingPathString/Home/Index
			//	or	/customRandomRoutingPathString/Home/Index/4
			routeBuilder.MapRoute("Default2", "customRandomRoutingPathString/{controller=Home}/{action=Index}/{id?}");
		}
	}
}
