﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TeamPulse.Performances.Infrastructure.DbContexts;

#nullable disable

namespace TeamPulse.Performances.Infrastructure.Migrations
{
    [DbContext(typeof(WriteDbContext))]
    [Migration("20250605030451_PerformancesInit")]
    partial class PerformancesInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("performances")
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.GroupOfSkills", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("grade_id")
                        .HasColumnType("uuid")
                        .HasColumnName("grade_id");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "TeamPulse.Performances.Domain.Entities.GroupOfSkills.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("group_description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "TeamPulse.Performances.Domain.Entities.GroupOfSkills.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("group_name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_group_of_skills");

                    b.HasIndex("grade_id")
                        .HasDatabaseName("ix_group_of_skills_grade_id");

                    b.ToTable("group_of_skills", "performances");
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.GroupSkill", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<Guid>("SkillId")
                        .HasColumnType("uuid")
                        .HasColumnName("skill_id");

                    b.HasKey("GroupId", "SkillId")
                        .HasName("pk_group_skills");

                    b.HasIndex("SkillId")
                        .HasDatabaseName("ix_group_skills_skill_id");

                    b.ToTable("group_skills", "performances");
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.RecordSkill", b =>
                {
                    b.Property<Guid>("WhoId")
                        .HasColumnType("uuid")
                        .HasColumnName("who_id");

                    b.Property<Guid>("WhatId")
                        .HasColumnType("uuid")
                        .HasColumnName("what_id");

                    b.Property<string>("ManagerGrade")
                        .HasColumnType("text")
                        .HasColumnName("manager_grade");

                    b.Property<string>("SelfGrade")
                        .HasColumnType("text")
                        .HasColumnName("self_grade");

                    b.HasKey("WhoId", "WhatId")
                        .HasName("pk_record_skills");

                    b.ToTable("record_skills", "performances");
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.Skill", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("grade_id")
                        .HasColumnType("uuid")
                        .HasColumnName("grade_id");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "TeamPulse.Performances.Domain.Entities.Skill.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "TeamPulse.Performances.Domain.Entities.Skill.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_skills");

                    b.HasIndex("grade_id")
                        .HasDatabaseName("ix_skills_grade_id");

                    b.ToTable("skills", "performances");
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.SkillGrade.BaseSkillGrade", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("GradesAsString")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("grades");

                    b.Property<string>("grade_type")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)")
                        .HasColumnName("grade_type");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "TeamPulse.Performances.Domain.Entities.SkillGrade.BaseSkillGrade.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("grade_description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "TeamPulse.Performances.Domain.Entities.SkillGrade.BaseSkillGrade.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("grade_name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_skill_grade");

                    b.ToTable("skill_grade", "performances");

                    b.HasDiscriminator<string>("grade_type").HasValue("BaseSkillGrade");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.SkillGrade.NumericSkillGrade", b =>
                {
                    b.HasBaseType("TeamPulse.Performances.Domain.Entities.SkillGrade.BaseSkillGrade");

                    b.ToTable("skill_grade", "performances");

                    b.HasDiscriminator().HasValue("numeric_grade");
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.SkillGrade.SymbolsSkillGrade", b =>
                {
                    b.HasBaseType("TeamPulse.Performances.Domain.Entities.SkillGrade.BaseSkillGrade");

                    b.ToTable("skill_grade", "performances");

                    b.HasDiscriminator().HasValue("symbol_grade");
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.GroupOfSkills", b =>
                {
                    b.HasOne("TeamPulse.Performances.Domain.Entities.SkillGrade.BaseSkillGrade", "SkillGrade")
                        .WithMany()
                        .HasForeignKey("grade_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_group_of_skills_skill_grade_grade_id");

                    b.Navigation("SkillGrade");
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.GroupSkill", b =>
                {
                    b.HasOne("TeamPulse.Performances.Domain.Entities.GroupOfSkills", "GroupOfSkills")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_group_skills_group_of_skills_group_id");

                    b.HasOne("TeamPulse.Performances.Domain.Entities.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_group_skills_skills_skill_id");

                    b.Navigation("GroupOfSkills");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("TeamPulse.Performances.Domain.Entities.Skill", b =>
                {
                    b.HasOne("TeamPulse.Performances.Domain.Entities.SkillGrade.BaseSkillGrade", "SkillGrade")
                        .WithMany()
                        .HasForeignKey("grade_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_skills_skill_grade_grade_id");

                    b.Navigation("SkillGrade");
                });
#pragma warning restore 612, 618
        }
    }
}
