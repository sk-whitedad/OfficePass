﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OfficePass.Domain;

#nullable disable

namespace OfficePass.Migrations
{
    [DbContext(typeof(OfficepassdbContext))]
    partial class OfficepassdbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OfficePass.Domain.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "IT"
                        });
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<int>("Company_Id")
                        .HasColumnType("integer");

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Specializations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Системный администратор"
                        });
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("UserProfileId")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "Admin",
                            Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                            RoleId = 1,
                            UserProfileId = 1
                        });
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsBoss")
                        .HasColumnType("boolean");

                    b.Property<string>("Lastname")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<int>("SpecializationId")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SpecializationId");

                    b.ToTable("UserProfiles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Firstname = "Иван",
                            GroupId = 1,
                            IsBoss = true,
                            Lastname = "Иванов",
                            PhoneNumber = "000",
                            SpecializationId = 1,
                            Surname = "Иванович"
                        });
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Guest", b =>
                {
                    b.HasOne("OfficePass.Domain.Entities.Company", "Company")
                        .WithMany("Guests")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.User", b =>
                {
                    b.HasOne("OfficePass.Domain.Entities.Role", "Role")
                        .WithMany("User")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OfficePass.Domain.Entities.UserProfile", "UserProfile")
                        .WithOne("User")
                        .HasForeignKey("OfficePass.Domain.Entities.User", "UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.UserProfile", b =>
                {
                    b.HasOne("OfficePass.Domain.Entities.Group", "Group")
                        .WithMany("UserProfile")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OfficePass.Domain.Entities.Specialization", "Specialization")
                        .WithMany("UserProfiles")
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Company", b =>
                {
                    b.Navigation("Guests");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Group", b =>
                {
                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Role", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.Specialization", b =>
                {
                    b.Navigation("UserProfiles");
                });

            modelBuilder.Entity("OfficePass.Domain.Entities.UserProfile", b =>
                {
                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
