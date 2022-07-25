using System;
using System.Linq;
using Hospital.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Storage.Extensions
{
    public static class DbContextExtensions
    {
        public static void UpdateSystemDates(this DbContext context)
        {
            var modifiedEntityEntries = context.ChangeTracker
                .Entries()
                .Where(item => item.State == EntityState.Modified);


            foreach (var entityEntry in modifiedEntityEntries)
                if (entityEntry.Entity is IUpdatedOnUtc updated)
                    updated.UpdatedOnUtc = DateTime.UtcNow;

            var addedEntityEntries = context.ChangeTracker
                .Entries()
                .Where(item => item.State == EntityState.Added);


            foreach (var entityEntry in addedEntityEntries)
            {
                if (entityEntry.Entity is IUpdatedOnUtc updated) updated.UpdatedOnUtc = DateTime.UtcNow;

                if (entityEntry.Entity is ICreatedOnUtc created) created.CreatedOnUtc = DateTime.UtcNow;
            }
        }
    }
}