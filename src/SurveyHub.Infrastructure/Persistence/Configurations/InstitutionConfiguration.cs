using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyHub.Domain;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.Infrastructure.Persistence.Configurations;

internal sealed class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder.ToTable("Institutions");

        builder.HasKey(i => i.UnitId);
        builder.Property(i => i.UnitId).ValueGeneratedNever();

        builder.Property(i => i.Name).HasMaxLength(Institution.NameMaxLength);
        builder.Property(i => i.State).HasMaxLength(2).IsFixedLength();
    }
}