using CsvHelper;
using CsvHelper.Configuration;
using KnxHelden.SHES.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Windows.Devices.Usb;

namespace KnxHelden.SHES.Data
{
    public class ShesDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Building> Buildings { get; set; }

        public DbSet<ProjectItem> ProjectItems { get; set; }

        public DbSet<BuildingPart> BuildingParts { get; set; }

        public DbSet<Floor> Floors { get; set; }

        public DbSet<Corridor> Corridors { get; set; }

        public DbSet<Cabinet> Cabinets { get; set; }

        public DbSet<Stair> Stairs { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<BinaryInput> BinaryInputs { get; set; }

        public DbSet<BlindActuator> BlindActuators { get; set; }

        public DbSet<BusPowerSupply> BusPowerSupplies { get; set; }

        public DbSet<DimmingActuator> DimmingActuators { get; set; }

        public DbSet<HeatingActuator> HeatingActuators { get; set; }

        public DbSet<SwitchingActuator> SwitchingActuators { get; set; }

        /// <summary>Initializes a new instance of the <see cref="ShesDbContext" /> class.</summary>
        /// <remarks>See <a href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</a>
        /// for more information.</remarks>
        public ShesDbContext()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ShesDbContext" /> class.</summary>
        /// <param name="options">The options for this context.</param>
        /// <remarks>
        /// See <a href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</a> and
        /// <a href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</a> for more information.
        /// </remarks>
        public ShesDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        ///   <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        ///   <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions">DbContextOptions</see> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured">IsConfigured</see> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)">OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)</see>.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.
        /// </param>
        /// <remarks>See <a href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</a>
        /// for more information.</remarks>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Create database directory
            string userDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string shesDatabasePath = Path.Combine(userDocumentPath, "SHES");
            Directory.CreateDirectory(shesDatabasePath);

            optionsBuilder.UseSqlite($"Data Source={Path.Combine(shesDatabasePath, "shes.db")};");
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1">DbSet</see> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.
        /// </param>
        /// <remarks>
        ///   <para>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)">UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)</see>)
        /// then this method will not be run.
        /// </para>
        ///   <para>
        /// See <a href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</a> for more information.
        /// </para>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Projects")
                .HasMany(project => project.Buildings)
                .WithOne(building => building.Project)
                .HasForeignKey(building => building.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Manufacturer>().ToTable("Manufacturers")
                .HasMany(manufacturer => manufacturer.Devices)
                .WithOne(device => device.Manufacturer)
                .HasForeignKey(device => device.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ProjectItem>().ToTable("ProjectItems")
                .HasMany(projectItem => projectItem.Children)
                .WithOne(projectItem => projectItem.Parent)
                .HasForeignKey(projectItem => projectItem.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Building>().ToTable("Buildings");

            modelBuilder.Entity<BuildingPart>().ToTable("BuildingParts");

            modelBuilder.Entity<Cabinet>().ToTable("Cabinets");

            modelBuilder.Entity<Corridor>().ToTable("Corridors");

            modelBuilder.Entity<Device>().ToTable("Devices");

            modelBuilder.Entity<Floor>().ToTable("Floors");

            modelBuilder.Entity<Room>().ToTable("Rooms");

            modelBuilder.Entity<Stair>().ToTable("Stairs");

            modelBuilder.Entity<BinaryInput>().ToTable("BinaryInputs");

            modelBuilder.Entity<BlindActuator>().ToTable("BlindActuators");

            modelBuilder.Entity<BusPowerSupply>().ToTable("BusPowerSupplys");

            modelBuilder.Entity<DimmingActuator>().ToTable("DimmingActuators");

            modelBuilder.Entity<HeatingActuator>().ToTable("HeatingActuators");

            modelBuilder.Entity<SwitchingActuator>().ToTable("SwitchingActuators");

            // Add seed data
            SeedManufacturers(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Prefilled manufacturer table with data
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private void SeedManufacturers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>().HasData(
                new Manufacturer()
                {
                    Id = new Guid("9a644771-78cf-4088-aa18-29575d0d643c"),
                    Name = "MDT"
                },
                new Manufacturer()
                {
                    Id = new Guid("0cf31014-48d7-4d3e-8596-ba88e7a90213"),
                    Name = "Siemens"
                });
        }
    }
}
