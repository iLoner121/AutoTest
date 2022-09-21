using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AutoTestEntity {
    public partial class ADOEntity : DbContext {
        public ADOEntity()
            : base("name=ADOEntity") {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Question> Question { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Account>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.Stem)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.Answer)
                .IsUnicode(false);
        }
    }
}
