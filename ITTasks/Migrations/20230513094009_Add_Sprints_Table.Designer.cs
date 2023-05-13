﻿// <auto-generated />
using System;
using ITTasks.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ITTasks.Migrations
{
    [DbContext(typeof(ITDbContext))]
    [Migration("20230513094009_Add_Sprints_Table")]
    partial class Add_Sprints_Table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ITTasks.DataLayer.Entities.ITTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("HourAmount")
                        .HasColumnType("real");

                    b.Property<Guid>("ITTaskTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SprintId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ITTaskTypeId");

                    b.HasIndex("SprintId");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ITTasks.DataLayer.Entities.ITTaskType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TasksType");
                });

            modelBuilder.Entity("ITTasks.DataLayer.Entities.Sprint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Sprints");
                });

            modelBuilder.Entity("ITTasks.DataLayer.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ITTasks.DataLayer.Entities.ITTask", b =>
                {
                    b.HasOne("ITTasks.DataLayer.Entities.ITTaskType", "ITTaskType")
                        .WithMany("Tasks")
                        .HasForeignKey("ITTaskTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ITTasks.DataLayer.Entities.Sprint", "Sprint")
                        .WithMany("Tasks")
                        .HasForeignKey("SprintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ITTasks.DataLayer.Entities.User", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ITTaskType");

                    b.Navigation("Sprint");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ITTasks.DataLayer.Entities.ITTaskType", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ITTasks.DataLayer.Entities.Sprint", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ITTasks.DataLayer.Entities.User", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
