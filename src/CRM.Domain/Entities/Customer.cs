namespace CRM.Domain.Entities;

using CRM.Domain.Core;
using CRM.Domain.ValueObjects;

public enum CustomerType { Individual, Company }

public class Customer : AggregateRoot
{
    public string Name { get; private set; }
    public CustomerType Type { get; private set; }
    public Document? Document { get; private set; }
    public Email? Email { get; private set; }
    public PhoneNumber? Phone { get; private set; }
    public Address? Address { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public bool IsActive { get; private set; }
    public string? StateRegistration { get; private set; } // IE
    public bool IsStateRegistrationExempt { get; private set; }

    protected Customer() { }

    public static Customer CreateIndividual(string name, Document document, Email email, DateTime birthDate, PhoneNumber? phone = null, Address? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        var today = DateTime.UtcNow.Date;
        var age = (today - birthDate.Date).Days / 365;
        if (age < 18)
            throw new InvalidOperationException("Customer must be at least 18 years old.");

        var customer = new Customer
        {
            Name = name,
            Type = CustomerType.Individual,
            Document = document,
            Email = email,
            BirthDate = birthDate,
            Phone = phone,
            Address = address,
            IsActive = true
        };

        customer.RaiseDomainEvent(new Events.CustomerCreatedEvent(customer.Id, customer.Name));

        return customer;
    }

    public static Customer CreateCompany(string name, Document document, Email email, string? stateRegistration, bool isExempt, PhoneNumber? phone = null, Address? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (!isExempt && string.IsNullOrWhiteSpace(stateRegistration))
            throw new InvalidOperationException("State registration (IE) is required for companies unless marked as exempt.");

        var customer = new Customer
        {
            Name = name,
            Type = CustomerType.Company,
            Document = document,
            Email = email,
            StateRegistration = stateRegistration,
            IsStateRegistrationExempt = isExempt,
            Phone = phone,
            Address = address,
            IsActive = true
        };

        customer.RaiseDomainEvent(new Events.CustomerCreatedEvent(customer.Id, customer.Name));

        return customer;
    }

    public void Deactivate()
    {
        IsActive = false;
        RaiseDomainEvent(new Events.CustomerDeactivatedEvent(Id));
    }
}