using System.Collections.Generic;

namespace CRM.Infrastructure.Repositories;

using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class CustomerRepository : ICustomerRepository
{
    private readonly CrmDbContext _db;
    public CustomerRepository(CrmDbContext db) => _db = db;

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Customers.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetByDocumentAsync(string documentValue, CancellationToken cancellationToken = default)
    {
        return await _db.Customers.FirstOrDefaultAsync(
            c => c.Document != null && c.Document.Value == documentValue,
            cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(string emailValue, CancellationToken cancellationToken = default)
    {
        return await _db.Customers.FirstOrDefaultAsync(
            c => c.Email != null && c.Email.Value == emailValue,
            cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _db.Customers.AddAsync(customer, cancellationToken);
    }

    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _db.Customers.Update(customer);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
