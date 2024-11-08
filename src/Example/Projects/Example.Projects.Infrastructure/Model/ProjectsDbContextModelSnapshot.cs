﻿// <auto-generated />
using System;
using Example.Projects.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Example.Projects.Infrastructure.Model
{
    [DbContext(typeof(ProjectsDbContext))]
    partial class ProjectsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Example.Projects.Domain.Packaging.PackagingArtwork", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("PackagingArtworks", "projects");
                });

            modelBuilder.Entity("Example.Projects.Domain.Projects.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<int>("ProjectType")
                        .HasColumnType("int");

                    b.Property<decimal>("Status")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Projects", "projects");

                    b.HasDiscriminator().HasValue("Project");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Example.Projects.Domain.Projects.Statuses.ProjectStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("StatusCode")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Statuses", "projects");
                });

            modelBuilder.Entity("Example.Projects.Domain.Packaging.PackagingProject", b =>
                {
                    b.HasBaseType("Example.Projects.Domain.Projects.Project");

                    b.HasDiscriminator().HasValue("PackagingProject");
                });

            modelBuilder.Entity("Example.Projects.Domain.Packaging.PackagingArtwork", b =>
                {
                    b.HasOne("Example.Projects.Domain.Packaging.PackagingProject", null)
                        .WithMany("ArtworksSelf")
                        .HasForeignKey("ProjectId");

                    b.OwnsOne("Example.Projects.Domain.Packaging.ArtworkNumber", "Number", b1 =>
                        {
                            b1.Property<Guid>("PackagingArtworkId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Text")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("ArtworkNumber");

                            b1.HasKey("PackagingArtworkId");

                            b1.ToTable("PackagingArtworks", "projects");

                            b1.WithOwner()
                                .HasForeignKey("PackagingArtworkId");
                        });

                    b.Navigation("Number")
                        .IsRequired();
                });

            modelBuilder.Entity("Example.Projects.Domain.Projects.Project", b =>
                {
                    b.OwnsOne("Example.Projects.Domain.Projects.ProjectName", "Name", b1 =>
                        {
                            b1.Property<Guid>("ProjectId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Text")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("ProjectName");

                            b1.HasKey("ProjectId");

                            b1.ToTable("Projects", "projects");

                            b1.WithOwner()
                                .HasForeignKey("ProjectId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Example.Projects.Domain.Projects.Statuses.ProjectStatus", b =>
                {
                    b.HasOne("Example.Projects.Domain.Projects.Project", null)
                        .WithMany("StatusesSelf")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("Example.Projects.Domain.Projects.Project", b =>
                {
                    b.Navigation("StatusesSelf");
                });

            modelBuilder.Entity("Example.Projects.Domain.Packaging.PackagingProject", b =>
                {
                    b.Navigation("ArtworksSelf");
                });
#pragma warning restore 612, 618
        }
    }
}
