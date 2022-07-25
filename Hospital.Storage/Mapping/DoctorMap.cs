using Hospital.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Storage.Mapping
{
    public sealed class DoctorMap : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctor");
            builder.HasKey(item => item.DoctorId);

            builder
                .HasIndex(item => item.DoctorId)
                .IsUnique();

            builder
                .HasOne(item => item.User)
                .WithOne(item => item.Doctor)
                .HasForeignKey<User>(item => item.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}