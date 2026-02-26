namespace CRM.Infrastructure.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;
using CRM.Domain.ValueObjects;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Type).IsRequired();

        // Document (ValueObject)
        builder.OwnsOne(c => c.Document, doc =>
        {
            doc.Property(d => d.Value)
                .HasColumnName("document_value")
                .HasMaxLength(50);

            doc.Property(d => d.Type)
                .HasColumnName("document_type")
                .HasConversion<int>();
        });

        // Email (ValueObject)
        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .HasMaxLength(200);
        });

        // Phone (ValueObject)
        builder.OwnsOne(c => c.Phone, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("phone")
                .HasMaxLength(20);
        });

        // Address (ValueObject)
        builder.OwnsOne(c => c.Address, addr =>
        {
            addr.Property(a => a.Street).HasColumnName("street").HasMaxLength(200);
            addr.Property(a => a.Number).HasColumnName("number").HasMaxLength(50);
            addr.Property(a => a.Complement).HasColumnName("complement").HasMaxLength(100);
            addr.Property(a => a.Neighborhood).HasColumnName("neighborhood").HasMaxLength(100);
            addr.Property(a => a.City).HasColumnName("city").HasMaxLength(100);
            addr.Property(a => a.State).HasColumnName("state").HasMaxLength(50);
            addr.Property(a => a.ZipCode).HasColumnName("zip_code").HasMaxLength(20);
        });

        builder.Property(c => c.IsActive).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt);
    }
}
