using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Models;

public partial class ControleEstoqueDbContext : DbContext
{
    public ControleEstoqueDbContext()
    {
    }

    public ControleEstoqueDbContext(DbContextOptions<ControleEstoqueDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CadFornecedor> CadFornecedor { get; set; }

    public virtual DbSet<CadProduto> CadProduto { get; set; }

    public virtual DbSet<CadVendaProduto> CadVendaProduto { get; set; }

    public virtual DbSet<CadVenda> CadVenda { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=controleEstoqueDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CadFornecedor>(entity =>
        {
            entity.HasKey(e => e.CdFornecedor).HasName("PK__cadForne__CE234A90F913AF44");

            entity.ToTable("cadFornecedor");

            entity.Property(e => e.CdFornecedor).HasColumnName("cdFornecedor");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.DtCriacao)
                .HasColumnType("datetime")
                .HasColumnName("dtCriacao");
            entity.Property(e => e.NmFornecedor)
                .IsUnicode(false)
                .HasColumnName("nmFornecedor");
        });

        modelBuilder.Entity<CadProduto>(entity =>
        {
            entity.HasKey(e => e.CdProduto).HasName("PK__cadProdu__8897B7734FD124D5");

            entity.ToTable("cadProduto");

            entity.Property(e => e.CdProduto).HasColumnName("cdProduto");
            entity.Property(e => e.CdFornecedor).HasColumnName("cdFornecedor");
            entity.Property(e => e.DsProduto).HasColumnName("dsProduto");
            entity.Property(e => e.DtCriacao)
                .HasColumnType("datetime")
                .HasColumnName("dtCriacao");
            entity.Property(e => e.NmProduto).HasColumnName("nmProduto");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");
            entity.Property(e => e.Tamanho)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tamanho");
            entity.Property(e => e.ValorCompra)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("valorCompra");
            entity.Property(e => e.ValorVenda)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("valorVenda");

            entity.HasOne(d => d.CdFornecedorNavigation).WithMany(p => p.CadProdutos)
                .HasForeignKey(d => d.CdFornecedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cadProduto_cadFornecedor");
        });

        modelBuilder.Entity<CadVendaProduto>(entity =>
        {
            entity.HasKey(e => e.CdVendaProduto).HasName("PK__cadVenda__CB73D0B5A689E3CD");

            entity.ToTable("cadVenda_produto");

            entity.Property(e => e.CdVendaProduto).HasColumnName("cdVendaProduto");
            entity.Property(e => e.CdProduto).HasColumnName("cdProduto");
            entity.Property(e => e.CdVenda).HasColumnName("cdVenda");

            entity.HasOne(d => d.CdProdutoNavigation).WithMany(p => p.CadVendaProdutos)
                .HasForeignKey(d => d.CdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cadVenda_produto_cadProduto");

            entity.HasOne(d => d.CdVendaNavigation).WithMany(p => p.CadVendaProdutos)
                .HasForeignKey(d => d.CdVenda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cadVenda_produto_cadVenda");
        });

        modelBuilder.Entity<CadVenda>(entity =>
        {
            entity.HasKey(e => e.CdVenda).HasName("PK__cadVenda__CD0A7B10A757AFCA");

            entity.ToTable("cadVenda");

            entity.Property(e => e.CdVenda).HasColumnName("cdVenda");
            entity.Property(e => e.DtVenda)
                .HasColumnType("datetime")
                .HasColumnName("dtVenda");
            entity.Property(e => e.NmComprador)
                .HasMaxLength(256)
                .HasColumnName("nmComprador");
            entity.Property(e => e.ValorLucro)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("valorLucro");
            entity.Property(e => e.ValorVenda)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("valorVenda");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
