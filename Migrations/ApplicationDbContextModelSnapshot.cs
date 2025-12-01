using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MiSitioWeb.Data;

#nullable disable

namespace MiSitioWeb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("MiSitioWeb.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER")
                    .HasColumnName("id");

                b.Property<bool>("Activo")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER")
                    .HasDefaultValue(true)
                    .HasColumnName("activo");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasColumnName("email");

                b.Property<DateTime>("FechaCreacion")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("fecha_creacion");

                b.Property<string>("PasswordHash")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasColumnName("password_hash");

                b.Property<string>("Username")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasColumnName("username");

                b.HasKey("Id")
                    .HasName("id");

                b.HasIndex("Email")
                    .IsUnique()
                    .HasDatabaseName("IX_usuarios_email");

                b.HasIndex("Username")
                    .IsUnique()
                    .HasDatabaseName("IX_usuarios_username");

                b.ToTable("usuarios");
            });
        }
    }
}
