using Microsoft.EntityFrameworkCore;

namespace PetAdoption.Api.Data;

public partial class PetContext : DbContext
{
    public PetContext()
    {
    }

    public PetContext(DbContextOptions<PetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<OwnedPet> OwnedPets { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAdoption> UserAdoptions { get; set; }

    public virtual DbSet<UserFavorite> UserFavorites { get; set; }

    public virtual DbSet<UserFriend> UserFriends { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.SentOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_ReceiverUser");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_SenderUser");
        });

        modelBuilder.Entity<OwnedPet>(entity =>
        {
            entity.HasOne(d => d.Pet).WithMany(p => p.OwnedPets)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OwnedPets_Pets");

            entity.HasOne(d => d.User).WithMany(p => p.OwnedPets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_OwnedPets_Users");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.Property(e => e.Breed).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Image).HasMaxLength(180);
            entity.Property(e => e.Name).HasMaxLength(25);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(10);
            entity.Property(e => e.ProfilePicture).HasMaxLength(180);
            entity.Property(e => e.Address).HasMaxLength(180);
            entity.Property(e => e.Phone).HasMaxLength(180);
        });

        modelBuilder.Entity<UserAdoption>(entity =>
        {
            entity.HasIndex(e => e.PetId, "IX_UserAdoptions_PetId");

            entity.HasIndex(e => e.UserId, "IX_UserAdoptions_UserId");

            entity.HasOne(d => d.Pet).WithMany(p => p.UserAdoptions).HasForeignKey(d => d.PetId);

            entity.HasOne(d => d.User).WithMany(p => p.UserAdoptions).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserFavorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserFavorites_1");

            entity.HasIndex(e => e.PetId, "IX_UserFavorites_PetId");

            entity.HasOne(d => d.Pet).WithMany(p => p.UserFavorites).HasForeignKey(d => d.PetId);

            entity.HasOne(d => d.User).WithMany(p => p.UserFavorites).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserFriend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserFriend");

            entity.HasOne(d => d.Friend).WithMany(p => p.UserFriendFriends)
                .HasForeignKey(d => d.FriendId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserFriend_Users1");

            entity.HasOne(d => d.User).WithMany(p => p.UserFriendUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserFriend_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
