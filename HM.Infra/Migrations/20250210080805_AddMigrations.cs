using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HM.Infra.Migrations
{
    public partial class AddMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "hm");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiresIn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "hm",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AcceptMinors = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    BookingConfirmationTimeInMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 15),
                    AdminUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hotels_Users_AdminUserId",
                        column: x => x.AdminUserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "hm",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "hm",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "hm",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "hm",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelAdmins",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    HotelAdminUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelAdmins_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "hm",
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelAdmins_Users_HotelAdminUserId",
                        column: x => x.HotelAdminUserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelEmployees",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelEmployees_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "hm",
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelEmployees_Users_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelPhotos",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    BucketURL = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ItsMainPhoto = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    HotelUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelPhotos_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "hm",
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelPhotos_Users_HotelUserId",
                        column: x => x.HotelUserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuiteCategories",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    HotelUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuiteCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuiteCategories_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "hm",
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuiteCategories_Users_HotelUserId",
                        column: x => x.HotelUserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suites",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PeopleCapacity = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    SuiteCategoryId = table.Column<int>(type: "integer", nullable: false),
                    DailyPriceDefault = table.Column<decimal>(type: "numeric(21,2)", precision: 21, scale: 2, nullable: false, defaultValue: 10m),
                    HotelUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suites_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "hm",
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suites_SuiteCategories_SuiteCategoryId",
                        column: x => x.SuiteCategoryId,
                        principalSchema: "hm",
                        principalTable: "SuiteCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suites_Users_HotelUserId",
                        column: x => x.HotelUserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    SuiteId = table.Column<int>(type: "integer", nullable: false),
                    SuiteCategoryId = table.Column<int>(type: "integer", nullable: false),
                    ReserveId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(21,2)", precision: 21, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    PaymentDeadline = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "hm",
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_SuiteCategories_SuiteCategoryId",
                        column: x => x.SuiteCategoryId,
                        principalSchema: "hm",
                        principalTable: "SuiteCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Suites_SuiteId",
                        column: x => x.SuiteId,
                        principalSchema: "hm",
                        principalTable: "Suites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuitePhotos",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SuiteId = table.Column<int>(type: "integer", nullable: false),
                    BucketURL = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    HotelUserId = table.Column<int>(type: "integer", nullable: false),
                    ItsMainPhoto = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuitePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuitePhotos_Suites_SuiteId",
                        column: x => x.SuiteId,
                        principalSchema: "hm",
                        principalTable: "Suites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuitePhotos_Users_HotelUserId",
                        column: x => x.HotelUserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuiteSchedules",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SuiteId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuiteSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuiteSchedules_Suites_SuiteId",
                        column: x => x.SuiteId,
                        principalSchema: "hm",
                        principalTable: "Suites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserves",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SuiteId = table.Column<int>(type: "integer", nullable: false),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CustomerUserId = table.Column<int>(type: "integer", nullable: false),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    Paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserves_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "hm",
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserves_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "hm",
                        principalTable: "Invoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reserves_Suites_SuiteId",
                        column: x => x.SuiteId,
                        principalSchema: "hm",
                        principalTable: "Suites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserves_Users_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalSchema: "hm",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReserveCustomerInformations",
                schema: "hm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReserveId = table.Column<int>(type: "integer", nullable: false),
                    CustomerFullname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CustomerBirthdayDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CustomerEmail = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CustomerCellphone = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Actived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReserveCustomerInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReserveCustomerInformations_Reserves_ReserveId",
                        column: x => x.ReserveId,
                        principalSchema: "hm",
                        principalTable: "Reserves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelAdmins_HotelAdminUserId",
                schema: "hm",
                table: "HotelAdmins",
                column: "HotelAdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelAdmins_HotelId",
                schema: "hm",
                table: "HotelAdmins",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelEmployees_EmployeeUserId",
                schema: "hm",
                table: "HotelEmployees",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelEmployees_HotelId",
                schema: "hm",
                table: "HotelEmployees",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelPhotos_HotelId",
                schema: "hm",
                table: "HotelPhotos",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelPhotos_HotelUserId",
                schema: "hm",
                table: "HotelPhotos",
                column: "HotelUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_AdminUserId",
                schema: "hm",
                table: "Hotels",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_HotelId",
                schema: "hm",
                table: "Invoices",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Paid",
                schema: "hm",
                table: "Invoices",
                column: "Paid");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PaymentDate",
                schema: "hm",
                table: "Invoices",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SuiteCategoryId",
                schema: "hm",
                table: "Invoices",
                column: "SuiteCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SuiteId",
                schema: "hm",
                table: "Invoices",
                column: "SuiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReserveCustomerInformations_ReserveId",
                schema: "hm",
                table: "ReserveCustomerInformations",
                column: "ReserveId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_CustomerUserId",
                schema: "hm",
                table: "Reserves",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_HotelId",
                schema: "hm",
                table: "Reserves",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_InvoiceId",
                schema: "hm",
                table: "Reserves",
                column: "InvoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_SuiteId",
                schema: "hm",
                table: "Reserves",
                column: "SuiteId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "hm",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "hm",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuiteCategories_HotelId",
                schema: "hm",
                table: "SuiteCategories",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_SuiteCategories_HotelUserId",
                schema: "hm",
                table: "SuiteCategories",
                column: "HotelUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SuitePhotos_HotelUserId",
                schema: "hm",
                table: "SuitePhotos",
                column: "HotelUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SuitePhotos_SuiteId",
                schema: "hm",
                table: "SuitePhotos",
                column: "SuiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Suites_HotelId",
                schema: "hm",
                table: "Suites",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Suites_HotelUserId",
                schema: "hm",
                table: "Suites",
                column: "HotelUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Suites_SuiteCategoryId",
                schema: "hm",
                table: "Suites",
                column: "SuiteCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SuiteSchedules_SuiteId",
                schema: "hm",
                table: "SuiteSchedules",
                column: "SuiteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "hm",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "hm",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "hm",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "hm",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "hm",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelAdmins",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "HotelEmployees",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "HotelPhotos",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "ReserveCustomerInformations",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "SuitePhotos",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "SuiteSchedules",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "Reserves",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "Suites",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "SuiteCategories",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "Hotels",
                schema: "hm");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "hm");
        }
    }
}
