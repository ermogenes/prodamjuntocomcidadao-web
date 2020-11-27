using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace prodamjuntocomcidadao_web.db
{
    public partial class prodamjuntocomcidadaoContext : DbContext
    {
        public prodamjuntocomcidadaoContext()
        {
        }

        public prodamjuntocomcidadaoContext(DbContextOptions<prodamjuntocomcidadaoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Local> Local { get; set; }
        public virtual DbSet<Mensagem> Mensagem { get; set; }
        public virtual DbSet<Tema> Tema { get; set; }
        public virtual DbSet<Tipo> Tipo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Local>(entity =>
            {
                entity.ToTable("local");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Curtidas).HasColumnName("curtidas");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Mensagem>(entity =>
            {
                entity.ToTable("mensagem");

                entity.HasIndex(e => e.LocalId)
                    .HasName("fk_mensagem_local1_idx");

                entity.HasIndex(e => e.TemaId)
                    .HasName("fk_mensagem_tema1_idx");

                entity.HasIndex(e => e.TipoId)
                    .HasName("fk_mensagem_tipo_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Curtidas).HasColumnName("curtidas");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnName("data")
                    .HasMaxLength(50);

                entity.Property(e => e.LocalId)
                    .HasColumnName("local_id")
                    .HasMaxLength(36);

                entity.Property(e => e.SentimentScore).HasColumnName("sentiment_score");

                entity.Property(e => e.TemaId)
                    .HasColumnName("tema_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Texto)
                    .IsRequired()
                    .HasColumnName("texto")
                    .HasMaxLength(400);

                entity.Property(e => e.TipoId)
                    .HasColumnName("tipo_id")
                    .HasMaxLength(36);

                entity.HasOne(d => d.Local)
                    .WithMany(p => p.Mensagem)
                    .HasForeignKey(d => d.LocalId)
                    .HasConstraintName("fk_mensagem_local1");

                entity.HasOne(d => d.Tema)
                    .WithMany(p => p.Mensagem)
                    .HasForeignKey(d => d.TemaId)
                    .HasConstraintName("fk_mensagem_tema1");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.Mensagem)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("fk_mensagem_tipo");
            });

            modelBuilder.Entity<Tema>(entity =>
            {
                entity.ToTable("tema");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Curtidas).HasColumnName("curtidas");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Tipo>(entity =>
            {
                entity.ToTable("tipo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Curtidas).HasColumnName("curtidas");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
