namespace CRM.API.Controllers;

using System.Linq;
using CRM.API.Contracts.Requests;
using CRM.API.Contracts.Responses;
using CRM.API.Mappings;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerRepository repository, ILogger<CustomersController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _repository.GetAllAsync(cancellationToken);
        return Ok(customers.Select(customer => customer.ToResponse()));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
            return NotFound();

        return Ok(customer.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var document = Document.Create(request.Document, request.DocumentType);
            var email = Email.Create(request.Email);
            var phone = request.Phone.ToValueObject();
            var address = request.Address.ToValueObject();

            var existingDocument = await _repository.GetByDocumentAsync(document.Value, cancellationToken);
            if (existingDocument is not null)
                return Conflict(new { field = "document", message = "A customer with this document already exists." });

            var existingEmail = await _repository.GetByEmailAsync(email.Value, cancellationToken);
            if (existingEmail is not null)
                return Conflict(new { field = "email", message = "A customer with this email already exists." });

            Customer customer = request.Type switch
            {
                CustomerType.Individual => Customer.CreateIndividual(
                    request.Name,
                    document,
                    email,
                    request.BirthDate!.Value,
                    phone,
                    address),
                CustomerType.Company => Customer.CreateCompany(
                    request.Name,
                    document,
                    email,
                    request.StateRegistration,
                    request.IsStateRegistrationExempt,
                    phone,
                    address),
                _ => throw new ArgumentOutOfRangeException(nameof(request.Type), "Unsupported customer type.")
            };

            await _repository.AddAsync(customer, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            var response = customer.ToResponse();
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            _logger.LogWarning(ex, "Failed to create customer.");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> Update(Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
            return NotFound();

        try
        {
            var email = Email.Create(request.Email);
            var phone = request.Phone.ToValueObject();
            var address = request.Address.ToValueObject();

            var existingEmail = await _repository.GetByEmailAsync(email.Value, cancellationToken);
            if (existingEmail is not null && existingEmail.Id != customer.Id)
                return Conflict(new { field = "email", message = "A customer with this email already exists." });

            customer.UpdateDetails(
                request.Name,
                email,
                phone,
                address,
                request.BirthDate,
                request.StateRegistration,
                request.IsStateRegistrationExempt);

            await _repository.UpdateAsync(customer, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Ok(customer.ToResponse());
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            _logger.LogWarning(ex, "Failed to update customer {CustomerId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
            return NotFound();

        customer.Deactivate();
        await _repository.UpdateAsync(customer, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
