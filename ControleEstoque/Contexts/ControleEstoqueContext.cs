using System;
using System.Collections.Generic;
using ControleEstoque.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Contexts;

public partial class ControleEstoqueContext : DbContext
{
    public ControleEstoqueContext()
    {
    }

    public ControleEstoqueContext(DbContextOptions<ControleEstoqueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<cadCliente> cadClientes { get; set; }

    public virtual DbSet<cadConfiguracao> cadConfiguracaos { get; set; }

    public virtual DbSet<cadFornecedor> cadFornecedors { get; set; }

    public virtual DbSet<cadProduto> cadProdutos { get; set; }

    public virtual DbSet<cadVenda_produto> cadVenda_produtos { get; set; }

    public virtual DbSet<cadVendum> cadVenda { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=controleEstoqueDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<cadCliente>(entity =>
        {
            entity.HasKey(e => e.cdCliente).HasName("PK__cadClien__64864EC629BB460D");

            entity.ToTable("cadCliente");

            entity.Property(e => e.bairro).HasMaxLength(256);
            entity.Property(e => e.cidade).HasMaxLength(256);
            entity.Property(e => e.complemento).HasMaxLength(256);
            entity.Property(e => e.cpf).HasMaxLength(256);
            entity.Property(e => e.nmCliente).HasMaxLength(256);
            entity.Property(e => e.numero).HasMaxLength(256);
            entity.Property(e => e.numeroCelular).HasMaxLength(256);
            entity.Property(e => e.rua).HasMaxLength(256);
        });

        modelBuilder.Entity<cadConfiguracao>(entity =>
        {
            entity.HasKey(e => e.cdConfiguracao).HasName("PK__cadConfi__2235037D1F4F4419");

            entity.ToTable("cadConfiguracao");

            entity.Property(e => e.corFundoSistema)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.nomeMenu)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        modelBuilder.Entity<cadFornecedor>(entity =>
        {
            entity.HasKey(e => e.cdFornecedor).HasName("PK__cadForne__CE234A906BD715C6");

            entity.ToTable("cadFornecedor");

            entity.Property(e => e.dtCriacao).HasColumnType("datetime");
            entity.Property(e => e.nmFornecedor).IsUnicode(false);
        });

        modelBuilder.Entity<cadProduto>(entity =>
        {
            entity.HasKey(e => e.cdProduto).HasName("PK__cadProdu__8897B7737865BAC9");

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
            entity.HasKey(e => e.cdVendaProduto).HasName("PK__cadVenda__CB73D0B5E67F5D7F");

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
            entity.HasKey(e => e.cdVenda).HasName("PK__cadVenda__CD0A7B10EBF94768");

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
