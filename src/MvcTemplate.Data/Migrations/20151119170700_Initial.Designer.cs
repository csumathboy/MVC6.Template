using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using MvcTemplate.Data.Core;

namespace MvcTemplate.Data.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20151119170700_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("IsLocked");

                    b.Property<string>("Passhash")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("RecoveryToken")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime?>("RecoveryTokenExpirationDate");

                    b.Property<string>("RoleId")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.AuditLog", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("AccountId")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Changes")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Log", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("AccountId")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Privilege", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Area")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Controller")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.RolePrivilege", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("PrivilegeId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.HasOne("MvcTemplate.Objects.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("MvcTemplate.Objects.RolePrivilege", b =>
                {
                    b.HasOne("MvcTemplate.Objects.Privilege")
                        .WithMany()
                        .HasForeignKey("PrivilegeId");

                    b.HasOne("MvcTemplate.Objects.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });
        }
    }
}
