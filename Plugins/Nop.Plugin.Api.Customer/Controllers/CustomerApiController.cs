using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nop.Plugin.Api.Customer.Models;
using Nop.Plugin.Api.Customer.Services;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Api.Customer.Controllers;

public class CustomerApiController : BasePluginController
{
    private readonly CustomerCrmService _customerCrmService;
    private readonly ILogger<CustomerApiController> _logger;

    public CustomerApiController(CustomerCrmService customerCrmService, ILogger<CustomerApiController> logger)
    {
        _customerCrmService = customerCrmService;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult CheckContactExists([FromBody] CheckerContactExistsModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            if (_customerCrmService.ContactExists(model.Email))
            {
                _logger.LogInformation("Contact with email {Email} already exists.", model.Email);
                return Conflict(new CustomerCrmResponse("Contact already exists.",false));
            }

            _customerCrmService.CreateContact(model.FirstName, model.LastName, model.Email);
            _logger.LogInformation("New contact created: {Email}", model.Email);

            return Ok(new CustomerCrmResponse("Contact created successfully",true));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking or creating a contact.");
            return BadRequest(new CustomerCrmResponse("An error occurred",false));
        }
    }

    private record CustomerCrmResponse(string Message, bool IsSuccess);
}
