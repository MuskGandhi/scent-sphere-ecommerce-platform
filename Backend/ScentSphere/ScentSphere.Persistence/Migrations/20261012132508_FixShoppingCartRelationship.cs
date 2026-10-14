using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScentSphere.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixShoppingCartRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty migration: model relationship fix requires no database schema changes
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty migration rollback: nothing to revert
        }
    }
}
