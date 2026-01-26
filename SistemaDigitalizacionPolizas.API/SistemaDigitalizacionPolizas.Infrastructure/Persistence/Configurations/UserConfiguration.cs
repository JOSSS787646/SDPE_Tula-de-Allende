using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(x => x.IdUser);

        builder.Property(x => x.IdUser)
               .HasColumnName("idUsuario");

        builder.Property(x => x.Email)
               .HasColumnName("email")
               .HasMaxLength(60)
               .IsRequired();

        builder.Property(x => x.Password)
               .HasColumnName("password")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(x => x.Asset)
               .HasColumnName("activo")
               .IsRequired();

        builder.Property(x => x.LastAccess)
               .HasColumnName("ultimoAcceso");

        builder.Property(x => x.CreationDate)
               .HasColumnName("createdAt")
               .IsRequired();

        builder.Property(x => x.UpdateDate)
               .HasColumnName("updateAt");

        builder.Property(x => x.IdAdministrativeUnit)
               .HasColumnName("idUnidadAdministrativa")
               .IsRequired();

        // IMPORTANTE: Aquí está el problema - la columna se llama "idRoles" (plural)
        builder.Property(x => x.IdRole)
               .HasColumnName("idRoles")
               .IsRequired();

        // Relación con AdministrativeUnit
        builder.HasOne(x => x.AdministrativeUnit)
               .WithMany(a => a.Users)
               .HasForeignKey(x => x.IdAdministrativeUnit)
               .OnDelete(DeleteBehavior.Restrict);

        // Relación con Role
        builder.HasOne(x => x.Role)
               .WithMany(r => r.Users)  // Añade esto
               .HasForeignKey(x => x.IdRole)
               .OnDelete(DeleteBehavior.Restrict);
    }
}