using Domain;
using Domain.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class AuditableFootballLeagueDbContext : DbContext
    {
        public DbSet<Audit> Audits { get; set; }

        // That method will be called everytime context.SaveChangesAsync() is being called
        // and the Tracker of entity framework realizes that those entity instances suffered modification by its state
        // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        public async Task<int> SaveChangesAsync(string userName)
        {
            var auditEntries = OnBeforeSaveChanges(userName);
            var saveResult = await base.SaveChangesAsync();
            if(auditEntries != null || auditEntries.Count > 0)
            {
                await OnAfterSaveChanges(auditEntries);
            }

            return saveResult;
        }

        private async Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey()){

                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    } else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                Audits.Add(auditEntry.ToAudit());
            }
            await SaveChangesAsync();
        }

        private List<AuditEntry> OnBeforeSaveChanges(string userName)
        {
            var entries = ChangeTracker.Entries().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified || q.State == EntityState.Deleted);
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in entries)
            {
                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Metadata.GetTableName(),
                    Action = entry.State.ToString()
                };

                var auditedObject = (BaseDomainObject) entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    auditedObject.CreatedAt = DateTime.Now;
                    auditedObject.CreatedBy = userName;
                }

                if (entry.State == EntityState.Modified)
                {
                    auditedObject.UpdatedAt = DateTime.Now;
                    auditedObject.UpdatedBy = userName;
                }

                foreach (var property in entry.Properties)
                {
                    // Is about a entity that is going to be inserted in Db. Without having an Id yet assigned to it
                    // since the Ids, by Default, are IDENTITY(1,1)
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                            }
                            break;

                    }
                }

                auditEntries.Add(auditEntry);
            }

            foreach (var pendingAuditEntry in auditEntries.Where(q => q.HasTemporaryProperties == false))
            {
                Audits.Add(pendingAuditEntry.ToAudit());
            }

            return auditEntries.Where(q => q.HasTemporaryProperties).ToList();
        }
    }
}
