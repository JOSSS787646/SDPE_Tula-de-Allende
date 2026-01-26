using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.IdRol);

        builder.Property(x => x.IdRol)
               .HasColumnName("idRoles");

        builder.Property(x => x.RolName)
               .HasColumnName("nombreRol")
               .HasMaxLength(45)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasColumnName("descripcion")
               .HasMaxLength(100);

        // Cambia Asset de string a bool
        builder.Property(x => x.Asset)
               .HasColumnName("activo")
               .IsRequired();

        // Relación con PermissionRole
        builder.HasMany(x => x.PermissionRoles)
               .WithOne(x => x.Role)
               .HasForeignKey(x => x.IdRole)  // Aquí está usando IdRole de PermissionRole
               .OnDelete(DeleteBehavior.Restrict);

        // Relación con Users
        builder.HasMany(x => x.Users)
               .WithOne(u => u.Role)
               .HasForeignKey(u => u.IdRole)
               .OnDelete(DeleteBehavior.Restrict);
    }
}