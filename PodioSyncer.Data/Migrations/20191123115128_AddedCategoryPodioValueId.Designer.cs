﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PodioSyncer.Data;

namespace PodioSyncer.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191123115128_AddedCategoryPodioValueId")]
    partial class AddedCategoryPodioValueId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PodioSyncer.Data.Models.CategoryMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AzureValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FieldMappingId")
                        .HasColumnType("int");

                    b.Property<int>("FieldType")
                        .HasColumnType("int");

                    b.Property<string>("PodioValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PodioValueId")
                        .HasColumnType("int");

                    b.Property<bool>("Required")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("FieldMappingId");

                    b.ToTable("CategoryMappings");
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.FieldMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AzureFieldName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FieldType")
                        .HasColumnType("int");

                    b.Property<string>("PodioFieldName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PrefixValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeMappingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeMappingId", "PodioFieldName");

                    b.ToTable("FieldMappings");
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.PodioApp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PodioAppId")
                        .HasColumnType("int");

                    b.Property<string>("PodioTypeExternalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Verified")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("PodioApps");
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.PodioAzureItemLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AzureId")
                        .HasColumnType("int");

                    b.Property<int>("AzureRevision")
                        .HasColumnType("int");

                    b.Property<string>("AzureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PodioAppId")
                        .HasColumnType("int");

                    b.Property<int>("PodioId")
                        .HasColumnType("int");

                    b.Property<int>("PodioRevision")
                        .HasColumnType("int");

                    b.Property<string>("PodioUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PodioAppId");

                    b.HasIndex("AzureId", "PodioId");

                    b.ToTable("PodioAzureItemLinks");
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.SyncEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AzureRevision")
                        .HasColumnType("int");

                    b.Property<int>("Initiator")
                        .HasColumnType("int");

                    b.Property<int>("PodioAzureItemLinkId")
                        .HasColumnType("int");

                    b.Property<int>("PodioRevision")
                        .HasColumnType("int");

                    b.Property<DateTime>("SyncDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PodioAzureItemLinkId");

                    b.ToTable("SyncEvents");
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.TypeMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AzureType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PodioAppId")
                        .HasColumnType("int");

                    b.Property<string>("PodioType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PodioAppId");

                    b.ToTable("TypeMappings");
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.CategoryMapping", b =>
                {
                    b.HasOne("PodioSyncer.Data.Models.FieldMapping", "FieldMapping")
                        .WithMany("CategoryMappings")
                        .HasForeignKey("FieldMappingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.FieldMapping", b =>
                {
                    b.HasOne("PodioSyncer.Data.Models.TypeMapping", "TypeMapping")
                        .WithMany("FieldMappings")
                        .HasForeignKey("TypeMappingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.PodioAzureItemLink", b =>
                {
                    b.HasOne("PodioSyncer.Data.Models.PodioApp", "PodioApp")
                        .WithMany("PodioAzureItemLinks")
                        .HasForeignKey("PodioAppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.SyncEvent", b =>
                {
                    b.HasOne("PodioSyncer.Data.Models.PodioAzureItemLink", "PodioAzureItemLink")
                        .WithMany("SyncEvents")
                        .HasForeignKey("PodioAzureItemLinkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PodioSyncer.Data.Models.TypeMapping", b =>
                {
                    b.HasOne("PodioSyncer.Data.Models.PodioApp", "PodioApp")
                        .WithMany("TypeMappings")
                        .HasForeignKey("PodioAppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
