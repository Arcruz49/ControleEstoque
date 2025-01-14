using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Models;

public partial class controleEstoqueDBContext : DbContext
{
    public controleEstoqueDBContext()
    {
    }

    public controleEstoqueDBContext(DbContextOptions<controleEstoqueDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<cadCliente> cadCliente { get; set; }

    public virtual DbSet<cadFornecedor> cadFornecedor { get; set; }

    public virtual DbSet<cadProduto> cadProduto { get; set; }

    public virtual DbSet<cadVenda_produto> cadVenda_produto { get; set; }

    public virtual DbSet<cadVendum> cadVenda { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=controleEstoqueDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<cadCliente>(entity =>
        {
            entity.HasKey(e => e.cdCliente).HasName("PK__cadClien__64864EC6D6438B7E");

            entity.ToTable("cadCliente");

            entity.Property(e => e.bairro).HasMaxLength(256);
            entity.Property(e => e.cidade).HasMaxLength(256);
            entity.Property(e => e.complemento).HasMaxLength(256);
            entity.Property(e => e.nmCliente).HasMaxLength(256);
            entity.Property(e => e.numero).HasMaxLength(256);
            entity.Property(e => e.numeroCelular).HasMaxLength(256);
            entity.Property(e => e.rua).HasMaxLength(256);
        });

        modelBuilder.Entity<cadFornecedor>(entity =>
        {
            entity.HasKey(e => e.cdFornecedor).HasName("PK__cadForne__CE234A905CB5DF87");

            entity.ToTable("cadFornecedor");

            entity.Property(e => e.dtCriacao).HasColumnType("datetime");
            entity.Property(e => e.nmFornecedor).IsUnicode(false);
        });

        modelBuilder.Entity<cadProduto>(entity =>
        {
            entity.HasKey(e => e.cdProduto).HasName("PK__cadProdu__8897B7732E40DFE5");

            entity.ToTable("cadProduto");

            entity.Property(e => e.dtCriacao).HasColumnType("datetime");
            entity.Property(e => e.tamanho)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.valorCompra).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.valorVenda).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.cdFornecedorNavigation).WithMany(p => p.cadProdutos)
                .HasForeignKey(d => d.cdFornecedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cadProduto_cadFornecedor");
        });

        modelBuilder.Entity<cadVenda_produto>(entity =>
        {
            entity.HasKey(e => e.cdVendaProduto).HasName("PK__cadVenda__CB73D0B56C79A717");

            entity.ToTable("cadVenda_produto");

            entity.HasOne(d => d.cdProdutoNavigation).WithMany(p => p.cadVenda_produtos)
                .HasForeignKey(d => d.cdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cadVenda_produto_cadProduto");

            entity.HasOne(d => d.cdVendaNavigation).WithMany(p => p.cadVenda_produtos)
                .HasForeignKey(d => d.cdVenda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cadVenda_produto_cadVenda");
        });

        modelBuilder.Entity<cadVendum>(entity =>
        {
            entity.HasKey(e => e.cdVenda).HasName("PK__cadVenda__CD0A7B10E7F13A06");

            entity.Property(e => e.dtVenda).HasColumnType("datetime");
            entity.Property(e => e.valorLucro).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.valorVenda).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.cdClienteNavigation).WithMany(p => p.cadVenda)
                .HasForeignKey(d => d.cdCliente)
                .HasConstraintName("FK_cadVenda_cadCliente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
