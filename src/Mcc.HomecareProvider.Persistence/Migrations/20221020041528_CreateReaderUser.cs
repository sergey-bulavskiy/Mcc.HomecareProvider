using Microsoft.EntityFrameworkCore.Migrations;

namespace Mcc.HomecareProvider.Persistence.Migrations
{
    public partial class CreateReaderUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"DO
$do$
BEGIN
IF EXISTS (
      SELECT FROM pg_catalog.pg_roles
      WHERE  rolname = 'reader') THEN

      RAISE NOTICE 'Role ""reader"" already exists. Skipping.';
   ELSE
      CREATE ROLE reader LOGIN PASSWORD 'y6ecm2w9vui79uft';
   END IF;
   GRANT SELECT ON ALL TABLES IN SCHEMA public to reader;
END
$do$;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
