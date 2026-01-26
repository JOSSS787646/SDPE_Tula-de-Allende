using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PermissionRoleConfiguration : IEntityTypeConfiguration<PermissionRole>
{
    public void Configure(EntityTypeBuilder<PermissionRole> builder)
    {
        builder.ToTable("RolesPermiso");

        builder.HasKey(x => x.IdPermissionRole);

        builder.Property(x => x.IdPermissionRole)
               .HasColumnName("idRolPermiso");

        // IMPORTANTE: La columna en la BD se llama "idRoles" (plural)
        builder.Property(x => x.IdRole)
               .HasColumnName("idRoles")  // Asegúrate que esto coincida
               .IsRequired();

        builder.Property(x => x.IdPermission)
               .HasColumnName("idPermiso")
               .IsRequired();

        builder.HasOne(x => x.Role)
               .WithMany(r => r.PermissionRoles)
               .HasForeignKey(x => x.IdRole)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Permission)
               .WithMany(p => p.PermissionRoles)
               .HasForeignKey(x => x.IdPermission)
               .OnDelete(DeleteBehavior.Restrict);
    }
}