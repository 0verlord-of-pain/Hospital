using Hospital.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Storage.Mapping
{
    public sealed class ScheduleMap : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedule");
            builder.HasKey(item => item.ScheduleId);

            builder
                .HasIndex(item => item.ScheduleId)
                .IsUnique();

            builder
                .HasOne(item => item.Doctor)
                .WithMany(item => item.Schedules)
                .HasForeignKey(item => item.DoctorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}