﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TvShows.Database;

#nullable disable

namespace TvShows.Database.Migrations
{
    [DbContext(typeof(TvShowsDbContext))]
    partial class TvShowsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PersonTvShow", b =>
                {
                    b.Property<int>("CastId")
                        .HasColumnType("integer");

                    b.Property<int>("TvShowsId")
                        .HasColumnType("integer");

                    b.HasKey("CastId", "TvShowsId");

                    b.HasIndex("TvShowsId");

                    b.ToTable("PersonTvShow");
                });

            modelBuilder.Entity("TvShows.Database.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("BirthDay")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("People", (string)null);
                });

            modelBuilder.Entity("TvShows.Database.Models.TvShow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("CastAdded")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TvShows", (string)null);
                });

            modelBuilder.Entity("PersonTvShow", b =>
                {
                    b.HasOne("TvShows.Database.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("CastId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TvShows.Database.Models.TvShow", null)
                        .WithMany()
                        .HasForeignKey("TvShowsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
