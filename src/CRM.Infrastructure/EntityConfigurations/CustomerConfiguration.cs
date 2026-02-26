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
        builder.OwnsOne(typeof(Document), "Document", doc =>
        {
            doc.Property<string>("Value").HasColumnName("document_value").HasMaxLength(50);
            doc.Property<int>("Type").HasColumnName("document_type");
        });

        // Email (ValueObject)
        builder.OwnsOne(typeof(Email), "Email", email =>
        {
            email.Property<string>("Value").HasColumnName("email").HasMaxLength(200);
        });

        // Phone (ValueObject)
        builder.OwnsOne(typeof(PhoneNumber), "Phone", phone =>
        {
            phone.Property<string>("Value").HasColumnName("phone").HasMaxLength(20);
        });

        // Address (ValueObject)
        builder.OwnsOne(typeof(Address), "Address", addr =>
        {
            addr.Property<string>("Street").HasColumnName("street").HasMaxLength(200);
            addr.Property<string>("Number").HasColumnName("number").HasMaxLength(50);
            addr.Property<string?>("Complement").HasColumnName("complement").HasMaxLength(100);
            addr.Property<string>("Neighborhood").HasColumnName("neighborhood").HasMaxLength(100);
            addr.Property<string>("City").HasColumnName("city").HasMaxLength(100);
            addr.Property<string>("State").HasColumnName("state").HasMaxLength(50);
            addr.Property<string>("ZipCode").HasColumnName("zip_code").HasMaxLength(20);
        });

        builder.Property(c => c.IsActive).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt);
    }
}
