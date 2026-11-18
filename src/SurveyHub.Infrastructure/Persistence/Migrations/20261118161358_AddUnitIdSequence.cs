using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitIdSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "UnitIdBaseSequence",
                schema: "dbo",
                startValue: 60000L,
                minValue: 60000L,
                maxValue: 99999L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "UnitIdBaseSequence",
                schema: "dbo");
        }
    }
}
