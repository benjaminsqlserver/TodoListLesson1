using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToDoList.Server.Models.ConData;

namespace ToDoList.Server.Data
{
    public partial class ConDataContext : DbContext
    {
        public ConDataContext()
        {
        }

        public ConDataContext(DbContextOptions<ConDataContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ToDoList.Server.Models.ConData.ToDoList>()
              .HasOne(i => i.PriorityLevel)
              .WithMany(i => i.ToDoLists)
              .HasForeignKey(i => i.PriorityID)
              .HasPrincipalKey(i => i.PriorityID);

            builder.Entity<ToDoList.Server.Models.ConData.ToDoList>()
              .HasOne(i => i.Status)
              .WithMany(i => i.ToDoLists)
              .HasForeignKey(i => i.StatusID)
              .HasPrincipalKey(i => i.StatusID);

            builder.Entity<ToDoList.Server.Models.ConData.ToDoList>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"(getdate())");

            builder.Entity<ToDoList.Server.Models.ConData.ToDoList>()
              .Property(p => p.UpdatedAt)
              .HasDefaultValueSql(@"(getdate())");

            builder.Entity<ToDoList.Server.Models.ConData.ToDoList>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime");

            builder.Entity<ToDoList.Server.Models.ConData.ToDoList>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<ToDoList.Server.Models.ConData.PriorityLevel> PriorityLevels { get; set; }

        public DbSet<ToDoList.Server.Models.ConData.Status> Statuses { get; set; }

        public DbSet<ToDoList.Server.Models.ConData.ToDoList> ToDoLists { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}