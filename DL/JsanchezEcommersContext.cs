using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class JsanchezEcommersContext : DbContext
{
    public JsanchezEcommersContext()
    {
    }

    public JsanchezEcommersContext(DbContextOptions<JsanchezEcommersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Descuento> Descuentos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.; Database= JSanchezEcommers; User ID=sa; TrustServerCertificate=True; Password=pass@word1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Idcategoria).HasName("PK__CATEGORI__ADC0E719E9B464C9");

            entity.ToTable("CATEGORIA");

            entity.Property(e => e.Idcategoria).HasColumnName("IDCATEGORIA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Descuento>(entity =>
        {
            entity.HasKey(e => e.Iddescuento).HasName("PK__DESCUENT__26F3ABA5AC8ED647");

            entity.ToTable("DESCUENTO");

            entity.Property(e => e.Iddescuento).HasColumnName("IDDESCUENTO");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("VALOR");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Idproducto).HasName("PK__PRODUCTO__7D8DC0F1F026214F");

            entity.ToTable("PRODUCTO");

            entity.Property(e => e.Idproducto).HasColumnName("IDPRODUCTO");
            entity.Property(e => e.Codigobarra)
                .IsUnicode(false)
                .HasColumnName("CODIGOBARRA");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Idcategoria).HasColumnName("IDCATEGORIA");
            entity.Property(e => e.Iddescuento).HasColumnName("IDDESCUENTO");
            entity.Property(e => e.Imagen)
                .IsUnicode(false)
                .HasColumnName("IMAGEN");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("PRECIO");
            entity.Property(e => e.Precioventa)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("PRECIOVENTA");
            entity.Property(e => e.Stock).HasColumnName("STOCK");

            entity.HasOne(d => d.IdcategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Idcategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoriaProducto");

            entity.HasOne(d => d.IddescuentoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Iddescuento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DescuentoProducto");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
