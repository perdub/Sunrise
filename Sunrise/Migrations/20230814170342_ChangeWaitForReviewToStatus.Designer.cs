﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sunrise;

#nullable disable

namespace Sunrise.Migrations
{
    [DbContext(typeof(SunriseContext))]
    [Migration("20230814170342_ChangeWaitForReviewToStatus")]
    partial class ChangeWaitForReviewToStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("PostTag", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("TEXT");

                    b.Property<int>("TagsTagId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PostId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("Sunrise.Types.FileInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("ContentType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Paths")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sha1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Sunrise.Types.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FileId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PostCreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Sunrise.Types.Session", b =>
                {
                    b.Property<string>("SessionId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("CanExpires")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastActivity")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("SessionInactive")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("SessionId");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Sunrise.Types.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullText")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PostCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SearchText")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Sunrise.Types.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("AccountCreationTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("CheckedUser")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("TelegramAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("VerifyUser")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sunrise.Types.Verify", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Verify");
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.HasOne("Sunrise.Types.Post", null)
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sunrise.Types.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sunrise.Types.Post", b =>
                {
                    b.HasOne("Sunrise.Types.FileInfo", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sunrise.Types.User", null)
                        .WithMany("Posts")
                        .HasForeignKey("UserId");

                    b.Navigation("File");
                });

            modelBuilder.Entity("Sunrise.Types.Session", b =>
                {
                    b.HasOne("Sunrise.Types.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sunrise.Types.Verify", b =>
                {
                    b.HasOne("Sunrise.Types.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sunrise.Types.User", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
