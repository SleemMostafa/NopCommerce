using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Nop.Plugin.Api.Customer.Services;

public class CustomerCrmService
{
    private ServiceClient _serviceClient;
    private readonly ILogger<CustomerCrmService> _logger;

    public CustomerCrmService(IConfiguration configuration, ILogger<CustomerCrmService> logger)
    {
        _logger = logger;

        try
        {
            _logger.LogInformation("Initializing CustomerCrmService...");

            var connectionString = $"AuthType={configuration["CrmConnection:AuthType"]};" +
                                   $"Url={configuration["CrmConnection:Url"]};" +
                                   $"ClientId={configuration["CrmConnection:ClientId"]};" +
                                   $"ClientSecret={configuration["CrmConnection:ClientSecret"]};";

            ConnectServerClient(connectionString);

            _logger.LogInformation("CustomerCrmService initialized successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Critical error occurred during CustomerCrmService initialization.");
            throw;
        }
    }

    private void ConnectServerClient(string connectionString)
    {
        try
        {
            _logger.LogInformation("Attempting to connect to Dataverse...");
            _serviceClient = new ServiceClient(connectionString);

            if (_serviceClient.IsReady)
            {
                _logger.LogInformation("Connection to Dataverse successful.");
            }
            else
            {
                _logger.LogError("Failed to connect to Dataverse. Connection string may be invalid.");
                throw new InvalidOperationException("Connection to Dataverse failed.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while connecting to Dataverse.");
            throw;
        }
    }

    public bool ContactExists(string email)
    {
        _logger.LogInformation("Checking if contact with email {Email} exists...", email);

        try
        {
            var query = new QueryExpression("contact") { ColumnSet = new ColumnSet("emailaddress1"), Criteria = new FilterExpression() };
            query.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, email);

            var contacts = _serviceClient.RetrieveMultiple(query);

            var exists = contacts.Entities.Any();
            _logger.LogInformation("Contact with email {Email} exists: {Exists}", email, exists);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking if contact with email {Email} exists.", email);
            throw;
        }
    }

    public void CreateContact(string firstName, string lastName, string email)
    {
        _logger.LogInformation("Creating new contact: {FirstName} {LastName} ({Email})", firstName, lastName, email);

        try
        {
            var contact = new Entity("contact") { ["firstname"] = firstName, ["lastname"] = lastName, ["emailaddress1"] = email };

            _serviceClient.Create(contact);

            _logger.LogInformation("Contact created successfully: {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating contact with email {Email}.", email);
            throw;
        }
    }
}