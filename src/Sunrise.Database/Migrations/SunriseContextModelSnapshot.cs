﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sunrise.Database;

#nullable disable

namespace Sunrise.Database.Migrations
{
    [DbContext(typeof(SunriseContext))]
    partial class SunriseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PostTag", b =>
                {
                    b.Property<Guid>("PostsPostId")
                        .HasColumnType("uuid");

                    b.Property<int>("TagsTagId")
                        .HasColumnType("integer");

                    b.HasKey("PostsPostId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("Sunrise.Types.Account", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("CheckedUser")
                        .HasColumnType("boolean");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PrivilegeLevel")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("VerifyUser")
                        .HasColumnType("boolean");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Sunrise.Types.File", b =>
                {
                    b.Property<Guid>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<int>("PostType")
                        .HasColumnType("integer");

                    b.Property<string>("Sha256")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("fullPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isSampleExsist")
                        .HasColumnType("boolean");

                    b.Property<string>("previewPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("samplePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FileId");

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Sunrise.Types.Post", b =>
                {
                    b.Property<Guid>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PostCreatorAccountId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.HasKey("PostId");

                    b.HasIndex("PostCreatorAccountId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Sunrise.Types.Session", b =>
                {
                    b.Property<string>("SessionId")
                        .HasColumnType("text");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<List<IPAddress>>("IPAddresses")
                        .IsRequired()
                        .HasColumnType("inet[]");

                    b.HasKey("SessionId");

                    b.HasIndex("AccountId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Sunrise.Types.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TagId"));

                    b.Property<int>("PostCount")
                        .HasColumnType("integer");

                    b.Property<string>("TagDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TagText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.HasOne("Sunrise.Types.Post", null)
                        .WithMany()
                        .HasForeignKey("PostsPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sunrise.Types.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sunrise.Types.File", b =>
                {
                    b.HasOne("Sunrise.Types.Post", "LinkedPost")
                        .WithOne("LinkedFile")
                        .HasForeignKey("Sunrise.Types.File", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LinkedPost");
                });

            modelBuilder.Entity("Sunrise.Types.Post", b =>
                {
                    b.HasOne("Sunrise.Types.Account", "PostCreator")
                        .WithMany("Posts")
                        .HasForeignKey("PostCreatorAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostCreator");
                });

            modelBuilder.Entity("Sunrise.Types.Session", b =>
                {
                    b.HasOne("Sunrise.Types.Account", "Account")
                        .WithMany("Sessions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Sunrise.Types.Account", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Sunrise.Types.Post", b =>
                {
                    b.Navigation("LinkedFile")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
