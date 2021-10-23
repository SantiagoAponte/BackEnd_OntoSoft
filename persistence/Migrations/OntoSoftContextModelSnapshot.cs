﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using persistence;

namespace persistence.Migrations
{
    [DbContext(typeof(OntoSoftContext))]
    partial class OntoSoftContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domine.Appoinments", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Appoinments");
                });

            modelBuilder.Entity("Domine.BackgroundMedical", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("backgroundMedicals");
                });

            modelBuilder.Entity("Domine.BackgroundOral", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("backgroundOrals");
                });

            modelBuilder.Entity("Domine.ClinicHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("backgroundMedical")
                        .HasColumnType("bit");

                    b.Property<bool>("backgroundOral")
                        .HasColumnType("bit");

                    b.Property<DateTime>("dateRegister")
                        .HasColumnType("datetime2");

                    b.Property<string>("nameCompanion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneCompanion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("clinicHistories");
                });

            modelBuilder.Entity("Domine.FaceTooth", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("faceTooth");
                });

            modelBuilder.Entity("Domine.Galleries", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Contain")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ObjectReference")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("dateCreate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Galleries");
                });

            modelBuilder.Entity("Domine.Odontogram", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("clinicHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("date_register")
                        .HasColumnType("datetime2");

                    b.Property<string>("observation")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("clinicHistoryId");

                    b.ToTable("Odontogram");
                });

            modelBuilder.Entity("Domine.OralRadiography", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("clinicHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("dateRegister")
                        .HasColumnType("datetime2");

                    b.Property<string>("observation")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("clinicHistoryId");

                    b.ToTable("oralRadiography");
                });

            modelBuilder.Entity("Domine.PatientEvolution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("clinicHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("dateCreate")
                        .HasColumnType("datetime2");

                    b.Property<string>("observation")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("clinicHistoryId");

                    b.ToTable("patientEvolution");
                });

            modelBuilder.Entity("Domine.TreamentPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("clinicHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("observation")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("clinicHistoryId");

                    b.ToTable("treamentPlan");
                });

            modelBuilder.Entity("Domine.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("addresContact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bloodType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("centerEmergency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("city")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("contactEmergency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dateBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("document")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("eps")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("height")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneEmergency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rh")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("typeDocumentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("weight")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("typeDocumentId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Domine.UserAppoinments", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AppoinmentsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("id");

                    b.HasIndex("AppoinmentsId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAppoinments");
                });

            modelBuilder.Entity("Domine.backgroundMedicalClinicHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BackgroundMedicalsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("clinicHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BackgroundMedicalsId");

                    b.HasIndex("clinicHistoryId");

                    b.ToTable("backgroundMedicalClinicHistories");
                });

            modelBuilder.Entity("Domine.backgroundOralClinicHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BackgroundOralsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("clinicHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BackgroundOralsId");

                    b.HasIndex("clinicHistoryId");

                    b.ToTable("backgroundOralClinicHistories");
                });

            modelBuilder.Entity("Domine.tooth", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("num")
                        .HasColumnType("int");

                    b.Property<string>("ubicacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tooth");
                });

            modelBuilder.Entity("Domine.typeDocument", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("typeDocument");
                });

            modelBuilder.Entity("Domine.typeProcess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("typeProcess");
                });

            modelBuilder.Entity("Domine.typeProcessTooth", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OdontogramId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ToothId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("faceToothId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("typeProcessId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OdontogramId");

                    b.HasIndex("ToothId");

                    b.HasIndex("faceToothId");

                    b.HasIndex("typeProcessId");

                    b.ToTable("typeProcessTooth");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Domine.ClinicHistory", b =>
                {
                    b.HasOne("Domine.User", "user")
                        .WithOne("clinicHistory")
                        .HasForeignKey("Domine.ClinicHistory", "UserId");
                });

            modelBuilder.Entity("Domine.Odontogram", b =>
                {
                    b.HasOne("Domine.User", "User")
                        .WithMany("odontogram")
                        .HasForeignKey("UserId");

                    b.HasOne("Domine.ClinicHistory", "clinicHistory")
                        .WithMany("Odontogram")
                        .HasForeignKey("clinicHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domine.OralRadiography", b =>
                {
                    b.HasOne("Domine.ClinicHistory", "clinicHistory")
                        .WithMany("oralRadiographyList")
                        .HasForeignKey("clinicHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domine.PatientEvolution", b =>
                {
                    b.HasOne("Domine.ClinicHistory", "clinicHistory")
                        .WithMany("patientEvolutionList")
                        .HasForeignKey("clinicHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domine.TreamentPlan", b =>
                {
                    b.HasOne("Domine.ClinicHistory", "clinicHistory")
                        .WithMany("treamentPlanList")
                        .HasForeignKey("clinicHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domine.User", b =>
                {
                    b.HasOne("Domine.typeDocument", "typeDocument")
                        .WithMany("userLista")
                        .HasForeignKey("typeDocumentId");
                });

            modelBuilder.Entity("Domine.UserAppoinments", b =>
                {
                    b.HasOne("Domine.Appoinments", "Appoinments")
                        .WithMany("userLink")
                        .HasForeignKey("AppoinmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domine.User", "User")
                        .WithMany("appoinmentsLink")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Domine.backgroundMedicalClinicHistory", b =>
                {
                    b.HasOne("Domine.BackgroundMedical", "BackgroundMedicals")
                        .WithMany("clinicHistoryLink")
                        .HasForeignKey("BackgroundMedicalsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domine.ClinicHistory", "clinicHistory")
                        .WithMany("BackgroundMedicalsLink")
                        .HasForeignKey("clinicHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domine.backgroundOralClinicHistory", b =>
                {
                    b.HasOne("Domine.BackgroundOral", "BackgroundOrals")
                        .WithMany("clinicHistoryLink")
                        .HasForeignKey("BackgroundOralsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domine.ClinicHistory", "clinicHistory")
                        .WithMany("BackgroundOralsLink")
                        .HasForeignKey("clinicHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domine.typeProcessTooth", b =>
                {
                    b.HasOne("Domine.Odontogram", "odontogram")
                        .WithMany("toothTypeProcessLink")
                        .HasForeignKey("OdontogramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domine.tooth", "Tooth")
                        .WithMany("typeProcessLink")
                        .HasForeignKey("ToothId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domine.FaceTooth", "faceTooth")
                        .WithMany("faceToothsLink")
                        .HasForeignKey("faceToothId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domine.typeProcess", "typeProcess")
                        .WithMany("toothLink")
                        .HasForeignKey("typeProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Domine.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Domine.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domine.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Domine.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
