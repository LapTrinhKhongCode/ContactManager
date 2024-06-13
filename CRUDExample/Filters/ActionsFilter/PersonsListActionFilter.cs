using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionsFilter
{
	public class PersonsListActionFilter : IActionFilter
	{
		private readonly ILogger<PersonsListActionFilter> _logger;	
		public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
		{
			_logger = logger;
		}
		public void OnActionExecuted(ActionExecutedContext context)
		{
			_logger.LogInformation("OnActionExecuted");
			PersonsController personsController = (PersonsController)context.Controller;
			IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)
				context.HttpContext.Items["arguments"];
			if (parameters != null) {
				if (parameters.ContainsKey("searchBy"))
				{
					personsController.ViewData["CurrentSearchBy"] = Convert.ToString(parameters["searchBy"]);
				}
				if (parameters.ContainsKey("searchString"))
				{
					personsController.ViewData["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
				}
				if (parameters.ContainsKey("sortBy"))
				{
					personsController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
				}
				if (parameters.ContainsKey("sortOrder"))
				{
					personsController.ViewData["CurrentSortOrder"] = Convert.ToString(parameters["sortOrder"]);
				}
				personsController.ViewBag.SearchFields = new Dictionary<string, string>()
				{
					{ nameof(PersonResponse.PersonName), "Person Name" },
					{ nameof(PersonResponse.Email), "Email" },
					{ nameof(PersonResponse.DateOfBirth), "Date of Birth" },
					{ nameof(PersonResponse.Gender), "Gender" },
					{ nameof(PersonResponse.CountryID), "Country" },
					{ nameof(PersonResponse.Address), "Address" }
				};
			}
			
				
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			_logger.LogInformation("OnActionExecuting");
			context.HttpContext.Items["arguments"] = context.ActionArguments;
		}
	}
}
