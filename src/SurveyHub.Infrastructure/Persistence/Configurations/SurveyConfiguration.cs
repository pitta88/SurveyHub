using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyHub.Domain.Surveys;

namespace SurveyHub.Infrastructure.Persistence.Configurations;

internal sealed class SurveyConfiguration : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder) 
    {
        builder.ToTable("Surveys");

        builder.HasKey(s => s.Id);
        
        builder.HasIndex(s=>s.Code).IsUnique();
        builder.Property(s=>s.Code).HasMaxLength(Survey.CodeMaxLength);
        builder.Property(s=>s.Title).HasMaxLength(Survey.TitleMaxLength);
    }
}