﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using devRoot.Server;

#nullable disable

namespace devRoot.Server.Migrations
{
    [DbContext(typeof(devRootContext))]
    partial class devRootContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("QuestTagJoin", b =>
                {
                    b.Property<int>("QuestsId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("QuestsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("QuestTagJoin");
                });

            modelBuilder.Entity("devRoot.Server.Models.ExampleCode", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<int>("Language")
                        .HasColumnType("integer");

                    b.Property<int?>("QuestId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("QuestId");

                    b.ToTable("ExampleCodes");
                });

            modelBuilder.Entity("devRoot.Server.Models.Fillout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CompletionTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeOnly>("FilloutTime")
                        .HasColumnType("time without time zone");

                    b.Property<int>("QuestId")
                        .HasColumnType("integer");

                    b.Property<string>("SubmittedCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SubmittedLanguage")
                        .HasColumnType("integer");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Fillouts");
                });

            modelBuilder.Entity("devRoot.Server.Models.Quest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int[]>("AvailableLanguages")
                        .HasColumnType("integer[]");

                    b.Property<string>("Console")
                        .HasColumnType("text");

                    b.Property<DateOnly>("Created")
                        .HasColumnType("date");

                    b.Property<int>("Difficulty")
                        .HasColumnType("integer");

                    b.Property<string>("PseudoCode")
                        .HasColumnType("text");

                    b.Property<string>("TaskDescription")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("devRoot.Server.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int[]>("Types")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("UserUid")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("devRoot.Server.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("devRoot.Server.Models.Vote", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int>("For")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Uid")
                        .HasColumnType("text");

                    b.Property<int>("VoteId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("QuestTagJoin", b =>
                {
                    b.HasOne("devRoot.Server.Models.Quest", null)
                        .WithMany()
                        .HasForeignKey("QuestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("devRoot.Server.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("devRoot.Server.Models.ExampleCode", b =>
                {
                    b.HasOne("devRoot.Server.Models.Quest", null)
                        .WithMany("ExampleCodes")
                        .HasForeignKey("QuestId");
                });

            modelBuilder.Entity("devRoot.Server.Models.Quest", b =>
                {
                    b.Navigation("ExampleCodes");
                });
#pragma warning restore 612, 618
        }
    }
}
