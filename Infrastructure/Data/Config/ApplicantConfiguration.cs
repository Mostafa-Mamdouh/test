using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name)
                .IsRequired();
            builder.Property(e => e.FamilyName)
                .IsRequired();
            builder.Property(e => e.Address)
                .IsRequired();
            builder.Property(e => e.CountryOfOrigin)
                .IsRequired();
            builder.Property(e => e.EMailAdress)
                .IsRequired();
            builder.Property(e => e.Age)
                .IsRequired();
            builder.Property(e => e.Hired).HasDefaultValue(false)
                 .IsRequired();
            builder.Property(e => e.CreateDate).HasColumnType("datetime")
                .IsRequired();
            builder.Property(e => e.UpdateDate).HasColumnType("datetime")
                .IsRequired();
            builder.Property(e => e.Deleted).HasDefaultValue(false)
                .IsRequired();
        }
    }
}