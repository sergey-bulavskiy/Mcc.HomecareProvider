﻿// <auto-generated />
using System;
using Mcc.HomecareProvider.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Mcc.HomecareProvider.Persistence.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    [Migration("20210527041337_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CurrentBindingId")
                        .HasColumnType("uuid");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CurrentBindingId")
                        .IsUnique();

                    b.HasIndex("SerialNumber")
                        .IsUnique();

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.DeviceBinding", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("AssignedToPatientAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PatientId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("PatientId");

                    b.ToTable("DeviceBindings");
                });

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CurrentBindingId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CurrentBindingId")
                        .IsUnique();

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.StatisticalDay", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("StatisticalDays");
                });

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.Device", b =>
                {
                    b.HasOne("Mcc.HomecareProvider.Domain.DeviceBinding", "CurrentBinding")
                        .WithOne()
                        .HasForeignKey("Mcc.HomecareProvider.Domain.Device", "CurrentBindingId");

                    b.Navigation("CurrentBinding");
                });

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.DeviceBinding", b =>
                {
                    b.HasOne("Mcc.HomecareProvider.Domain.Device", null)
                        .WithMany("DeviceBindings")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mcc.HomecareProvider.Domain.Patient", "Patient")
                        .WithMany("DeviceBindings")
                        .HasForeignKey("PatientId");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.Patient", b =>
                {
                    b.HasOne("Mcc.HomecareProvider.Domain.DeviceBinding", "CurrentBinding")
                        .WithOne()
                        .HasForeignKey("Mcc.HomecareProvider.Domain.Patient", "CurrentBindingId");

                    b.Navigation("CurrentBinding");
                });

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.Device", b =>
                {
                    b.Navigation("DeviceBindings");
                });

            modelBuilder.Entity("Mcc.HomecareProvider.Domain.Patient", b =>
                {
                    b.Navigation("DeviceBindings");
                });
#pragma warning restore 612, 618
        }
    }
}