using Hospital.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Storage.Mapping
{
    public sealed class RecordMap : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            builder.ToTable("Record");
            builder.HasKey(item => item.RecordId);

            builder
                .HasIndex(item => item.RecordId)
                .IsUnique();

            builder
                .HasOne(item => item.Doctor)
                .WithMany(item => item.Records)
                .HasForeignKey(item => item.DoctorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(item => item.Patient)
                .WithMany(item => item.Records)
                .HasForeignKey(item => item.PatientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}