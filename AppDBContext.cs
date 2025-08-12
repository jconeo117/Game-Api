using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }


        public DbSet<MUser> Users { get; set; }
        public DbSet<MCharacter> Characters { get; set; }
        public DbSet<MInventory> Inventory { get; set; }
        public DbSet<MItems> Items { get; set; }
        public DbSet<ItemShop> ItemsShop { get; set; }
        public DbSet<MDungeon> Dungeons { get; set; }
        public DbSet<MDungeonRun> DungeonRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MUser>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            });

            modelBuilder.Entity<MCharacter>(entity =>
            {
                entity.HasOne(c => c.User)
                .WithOne(u => u.Character)
                .HasForeignKey<MCharacter>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MInventory>(entity =>
            {
                entity.HasOne(I => I.Player)
                .WithOne(c => c.Inventory)
                .HasForeignKey<MInventory>(I => I.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MItems>(entity =>
            {
                entity.HasOne(It => It.Inventory)
                .WithMany(I => I.Items)
                .HasForeignKey(it => it.InventaryId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ItemShop>(entity =>
            {
                entity.HasOne(Is => Is.Item)
                .WithMany(I => I.ShoppingListing)
                .HasForeignKey(Is => Is.ItemId);

                entity.HasOne(Is => Is.Character)
                .WithMany(ow => ow.ItemShop)
                .HasForeignKey(Is => Is.CharacterId);
            });

            modelBuilder.Entity<MDungeonRun>(Entity =>
            {
                Entity.HasOne(dr => dr.Dungeon)
                .WithMany(d => d.Runs)
                .HasForeignKey(dr => dr.DungeonId)
                .OnDelete(DeleteBehavior.Restrict);

                Entity.HasOne(dr => dr.Character)
                .WithMany(c => c.DungeonRuns)
                .HasForeignKey (dr => dr.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
