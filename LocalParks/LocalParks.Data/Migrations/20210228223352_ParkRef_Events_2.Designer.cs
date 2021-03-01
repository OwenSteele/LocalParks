﻿// <auto-generated />
using System;
using LocalParks.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LocalParks.Data.Migrations
{
    [DbContext(typeof(ParkContext))]
    [Migration("20210228223352_ParkRef_Events_2")]
    partial class ParkRef_Events_2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "6.0.0-preview.1.21102.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LocalParks.Core.LocalParksUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("MemberSince")
                        .HasColumnType("datetime2");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PostcodeZone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("LocalParks.Core.Park", b =>
                {
                    b.Property<int>("ParkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ClosingTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpeningTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostcodeZone")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SizeInMetresSquared")
                        .HasColumnType("int");

                    b.HasKey("ParkId");

                    b.HasIndex("PostcodeZone");

                    b.ToTable("Parks");

                    b.HasData(
                        new
                        {
                            ParkId = 1,
                            ClosingTime = new DateTime(2021, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 40.34857m,
                            Longitude = 0.384756m,
                            Name = "Leafy Meadows",
                            OpeningTime = new DateTime(2021, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified),
                            PostcodeZone = "LP4",
                            SizeInMetresSquared = 200000
                        },
                        new
                        {
                            ParkId = 2,
                            ClosingTime = new DateTime(2021, 1, 1, 22, 0, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 38.8765m,
                            Longitude = 0.18798m,
                            Name = "Middlesbrook Park",
                            OpeningTime = new DateTime(2021, 1, 1, 6, 0, 0, 0, DateTimeKind.Unspecified),
                            PostcodeZone = "PF15",
                            SizeInMetresSquared = 85000
                        },
                        new
                        {
                            ParkId = 3,
                            ClosingTime = new DateTime(2021, 1, 1, 23, 0, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 35.1034m,
                            Longitude = 2.4678m,
                            Name = "Mirror Lake Park",
                            OpeningTime = new DateTime(2021, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            PostcodeZone = "PF10",
                            SizeInMetresSquared = 2000
                        },
                        new
                        {
                            ParkId = 7,
                            ClosingTime = new DateTime(2021, 1, 1, 16, 30, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 41.0301m,
                            Longitude = -2.9088m,
                            Name = "Shadow Grounds",
                            OpeningTime = new DateTime(2021, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified),
                            PostcodeZone = "LP2",
                            SizeInMetresSquared = 26500
                        },
                        new
                        {
                            ParkId = 4,
                            ClosingTime = new DateTime(2021, 1, 1, 16, 30, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 41.6545m,
                            Longitude = -1.564m,
                            Name = "Shadow Grounds",
                            OpeningTime = new DateTime(2021, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified),
                            PostcodeZone = "LP4",
                            SizeInMetresSquared = 26500
                        },
                        new
                        {
                            ParkId = 5,
                            ClosingTime = new DateTime(2021, 1, 1, 18, 30, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 38.999m,
                            Longitude = -5.938m,
                            Name = "Sapphire Gardens",
                            OpeningTime = new DateTime(2021, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            PostcodeZone = "PF13",
                            SizeInMetresSquared = 6000
                        },
                        new
                        {
                            ParkId = 6,
                            ClosingTime = new DateTime(2021, 1, 1, 20, 30, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 39.7145m,
                            Longitude = -4.123m,
                            Name = "Fletcher Plaza",
                            OpeningTime = new DateTime(2021, 1, 1, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            PostcodeZone = "PF13",
                            SizeInMetresSquared = 70000
                        });
                });

            modelBuilder.Entity("LocalParks.Core.ParkEvent", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganiserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganiserFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganiserLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganiserPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParkRef")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("EventId");

                    b.HasIndex("ParkRef");

                    b.HasIndex("UserId");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            EventId = 1,
                            Date = new DateTime(2021, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "A fair for everyone, for hobbyists and professionals to come together and meet.",
                            Name = "Arts and Crafts Fair",
                            OrganiserEmail = "JoanRoberts@owensteele.co.uk",
                            OrganiserFirstName = "Joan",
                            OrganiserLastName = "Roberts",
                            OrganiserPhoneNumber = "07123456789",
                            ParkRef = 1
                        },
                        new
                        {
                            EventId = 2,
                            Date = new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Come and race other from the village, egg and spoons provided!",
                            Name = "Egg and Spoon Race",
                            OrganiserEmail = "DanJackson@owensteele.co.uk",
                            OrganiserFirstName = "Dan",
                            OrganiserLastName = "Jackson",
                            OrganiserPhoneNumber = "0700345876",
                            ParkRef = 2
                        },
                        new
                        {
                            EventId = 3,
                            Date = new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Come and adopt a new feline or canine friend, all looking for good homes.",
                            Name = "Pet adoption Clinic",
                            OrganiserEmail = "MarkDavis@owensteele.co.uk",
                            OrganiserFirstName = "Mark",
                            OrganiserLastName = "Davis",
                            OrganiserPhoneNumber = "01196596691",
                            ParkRef = 2
                        });
                });

            modelBuilder.Entity("LocalParks.Core.Postcode", b =>
                {
                    b.Property<string>("Zone")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Neighbourhood")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Population")
                        .HasColumnType("bigint");

                    b.HasKey("Zone");

                    b.ToTable("Postcodes");

                    b.HasData(
                        new
                        {
                            Zone = "LP1",
                            Neighbourhood = "Ringhurst",
                            Population = 274987L
                        },
                        new
                        {
                            Zone = "LP2",
                            Neighbourhood = "MapleDown",
                            Population = 16570L
                        },
                        new
                        {
                            Zone = "LP3",
                            Neighbourhood = "Alterdon",
                            Population = 98105L
                        },
                        new
                        {
                            Zone = "LP4",
                            Neighbourhood = "Rakeley",
                            Population = 7654L
                        },
                        new
                        {
                            Zone = "PF10",
                            Neighbourhood = "Chapterborough",
                            Population = 100311L
                        },
                        new
                        {
                            Zone = "PF11",
                            Neighbourhood = "MarketTown",
                            Population = 3948L
                        },
                        new
                        {
                            Zone = "PF12",
                            Neighbourhood = "Snowbush",
                            Population = 10334L
                        },
                        new
                        {
                            Zone = "PF13",
                            Neighbourhood = "Romsby",
                            Population = 12808L
                        },
                        new
                        {
                            Zone = "PF14",
                            Neighbourhood = "Accreton",
                            Population = 6510L
                        },
                        new
                        {
                            Zone = "PF15",
                            Neighbourhood = "Caelfall",
                            Population = 968L
                        });
                });

            modelBuilder.Entity("LocalParks.Core.SportsClub", b =>
                {
                    b.Property<int>("ClubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Members")
                        .HasColumnType("int");

                    b.Property<decimal>("MembershipFee")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParkId")
                        .HasColumnType("int");

                    b.Property<string>("President")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sport")
                        .HasColumnType("int");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClubId");

                    b.HasIndex("ParkId");

                    b.ToTable("SportsClubs");

                    b.HasData(
                        new
                        {
                            ClubId = 1,
                            Email = "CPCPolo@owensteele.co.uk",
                            Members = 103,
                            MembershipFee = 20000m,
                            Name = "Curling Polo Club",
                            ParkId = 1,
                            President = "Matthrew Albright",
                            Sport = 0,
                            Website = "CPCPolo-OS.co.uk"
                        },
                        new
                        {
                            ClubId = 2,
                            Email = "Contact.Meadow-Rugby@owensteele.co.uk",
                            Members = 20,
                            MembershipFee = 150m,
                            Name = "Meadow RFC",
                            ParkId = 1,
                            President = "Jess Hampston",
                            Sport = 2,
                            Website = "Meadow-Rugby-OS.co.uk"
                        },
                        new
                        {
                            ClubId = 3,
                            Email = "Admin.ChesterlyCricket@owensteele.co.uk",
                            Members = 67,
                            MembershipFee = 330m,
                            Name = "Chesterly Cricket Club",
                            ParkId = 2,
                            President = "Linda Paul",
                            Sport = 8,
                            Website = "ChesterlyCricketClub-OS.co.uk"
                        });
                });

            modelBuilder.Entity("LocalParks.Core.Supervisor", b =>
                {
                    b.Property<int>("SupervisorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmergencyNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Office")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParkRef")
                        .HasColumnType("int");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("StartingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("SupervisorId");

                    b.HasIndex("ParkRef")
                        .IsUnique();

                    b.ToTable("Supervisors");

                    b.HasData(
                        new
                        {
                            SupervisorId = 1,
                            EmergencyNumber = "07654321000",
                            FirstName = "Joe",
                            LastName = "Bloggs",
                            Office = "123 Chestnut Drive",
                            ParkRef = 1,
                            Salary = 35000m,
                            StartingDate = new DateTime(1980, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            SupervisorId = 2,
                            EmergencyNumber = "07685940321",
                            FirstName = "Anne",
                            LastName = "Smith",
                            Office = "Parkside Cottage, Park Avenue",
                            ParkRef = 2,
                            Salary = 32000m,
                            StartingDate = new DateTime(1960, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            SupervisorId = 3,
                            EmergencyNumber = "0784368711",
                            FirstName = "Robert",
                            LastName = "Bob",
                            Office = "Bramble side, Supervisor Street",
                            ParkRef = 3,
                            Salary = 35500m,
                            StartingDate = new DateTime(1986, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            SupervisorId = 4,
                            EmergencyNumber = "0762985702",
                            FirstName = "Yohan",
                            LastName = "Blake",
                            Office = "1 Park Lane",
                            ParkRef = 4,
                            Salary = 29000m,
                            StartingDate = new DateTime(2019, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            SupervisorId = 5,
                            EmergencyNumber = "0723096876",
                            FirstName = "Michael",
                            LastName = "Mitchell",
                            Office = "154 Wednesday drive",
                            ParkRef = 5,
                            Salary = 34000m,
                            StartingDate = new DateTime(2016, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            SupervisorId = 6,
                            EmergencyNumber = "0787876876",
                            FirstName = "Daniel",
                            LastName = "Cotting",
                            Office = "Little Cottage, Ramping Lane",
                            ParkRef = 6,
                            Salary = 26350m,
                            StartingDate = new DateTime(2011, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            SupervisorId = 7,
                            EmergencyNumber = "0789871231",
                            FirstName = "Abraham",
                            LastName = "Yoluni",
                            Office = "Ramping Cottage, Little Lane",
                            ParkRef = 7,
                            Salary = 22850m,
                            StartingDate = new DateTime(2008, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
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

            modelBuilder.Entity("LocalParks.Core.Park", b =>
                {
                    b.HasOne("LocalParks.Core.Postcode", "Postcode")
                        .WithMany("Parks")
                        .HasForeignKey("PostcodeZone");

                    b.Navigation("Postcode");
                });

            modelBuilder.Entity("LocalParks.Core.ParkEvent", b =>
                {
                    b.HasOne("LocalParks.Core.Park", "Park")
                        .WithMany("Events")
                        .HasForeignKey("ParkRef")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LocalParks.Core.LocalParksUser", "User")
                        .WithMany("OrganisedEvents")
                        .HasForeignKey("UserId");

                    b.Navigation("Park");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LocalParks.Core.SportsClub", b =>
                {
                    b.HasOne("LocalParks.Core.Park", "Park")
                        .WithMany("SportClubs")
                        .HasForeignKey("ParkId");

                    b.Navigation("Park");
                });

            modelBuilder.Entity("LocalParks.Core.Supervisor", b =>
                {
                    b.HasOne("LocalParks.Core.Park", "Park")
                        .WithOne("Supervisor")
                        .HasForeignKey("LocalParks.Core.Supervisor", "ParkRef")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Park");
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
                    b.HasOne("LocalParks.Core.LocalParksUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LocalParks.Core.LocalParksUser", null)
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

                    b.HasOne("LocalParks.Core.LocalParksUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LocalParks.Core.LocalParksUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LocalParks.Core.LocalParksUser", b =>
                {
                    b.Navigation("OrganisedEvents");
                });

            modelBuilder.Entity("LocalParks.Core.Park", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("SportClubs");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("LocalParks.Core.Postcode", b =>
                {
                    b.Navigation("Parks");
                });
#pragma warning restore 612, 618
        }
    }
}
