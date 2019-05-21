﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindikat.Ankete.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ankete",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: false),
                    VrijemeKreiranja = table.Column<DateTime>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ankete", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoviPitanja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VrstaPitanja = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviPitanja", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PopunjeneAnkete",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KorisnikId = table.Column<string>(nullable: false),
                    AnketaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopunjeneAnkete", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PopunjeneAnkete_Ankete_AnketaId",
                        column: x => x.AnketaId,
                        principalTable: "Ankete",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pitanja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TekstPitanja = table.Column<string>(nullable: false),
                    AnketaId = table.Column<int>(nullable: true),
                    TipPitanjaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pitanja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pitanja_Ankete_AnketaId",
                        column: x => x.AnketaId,
                        principalTable: "Ankete",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pitanja_TipoviPitanja_TipPitanjaId",
                        column: x => x.TipPitanjaId,
                        principalTable: "TipoviPitanja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Odgovori",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OdgovorPitanja = table.Column<string>(nullable: false),
                    PopunjenaAnketaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odgovori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odgovori_PopunjeneAnkete_PopunjenaAnketaId",
                        column: x => x.PopunjenaAnketaId,
                        principalTable: "PopunjeneAnkete",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PonudeniOdgovori",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DefiniraniOdgovor = table.Column<string>(nullable: false),
                    PitanjeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PonudeniOdgovori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PonudeniOdgovori_Pitanja_PitanjeId",
                        column: x => x.PitanjeId,
                        principalTable: "Pitanja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Odgovori_PopunjenaAnketaId",
                table: "Odgovori",
                column: "PopunjenaAnketaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanja_AnketaId",
                table: "Pitanja",
                column: "AnketaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanja_TipPitanjaId",
                table: "Pitanja",
                column: "TipPitanjaId");

            migrationBuilder.CreateIndex(
                name: "IX_PonudeniOdgovori_PitanjeId",
                table: "PonudeniOdgovori",
                column: "PitanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_PopunjeneAnkete_AnketaId",
                table: "PopunjeneAnkete",
                column: "AnketaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Odgovori");

            migrationBuilder.DropTable(
                name: "PonudeniOdgovori");

            migrationBuilder.DropTable(
                name: "PopunjeneAnkete");

            migrationBuilder.DropTable(
                name: "Pitanja");

            migrationBuilder.DropTable(
                name: "Ankete");

            migrationBuilder.DropTable(
                name: "TipoviPitanja");
        }
    }
}
