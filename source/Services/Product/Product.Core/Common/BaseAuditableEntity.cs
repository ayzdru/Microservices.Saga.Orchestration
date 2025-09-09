using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Product.Core.Common
{
    public abstract class BaseAuditableEntity
    {
        public User? CreatedByUser { get; protected set; }
        public Guid? CreatedByUserId { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public User? UpdatedByUser { get; protected set; }
        public Guid? UpdatedByUserId { get; protected set; }
        public DateTime? UpdatedDate { get; protected set; }
        public uint RowVersion { get; protected set; }
        public User? DeletedByUser { get; protected set; }
        public Guid? DeletedByUserId { get; protected set; }
        public DateTime? DeletedDate { get; protected set; }
        public bool IsDeleted { get; protected set; } = false;
        public bool IsActive { get; protected set; } = true;
        
    
        private readonly List<BaseNotification> _notifications = new();

        [NotMapped]
        public IReadOnlyCollection<BaseNotification> Notifications => _notifications.AsReadOnly();

        public void AddNotification(BaseNotification baseNotification)
        {
            _notifications.Add(baseNotification);
        }

        public void RemoveNotification(BaseNotification baseNotification)
        {
            _notifications.Remove(baseNotification);
        }

        public void ClearNotifications()
        {
            _notifications.Clear();
        }
        public void SetCreated(Guid? createdByUserId)
        {
            CreatedByUserId = createdByUserId;
            CreatedDate = DateTime.UtcNow;
        }
        public void SetUpdated(Guid? updatedByUserId)
        {
            UpdatedByUserId = updatedByUserId;
            UpdatedDate = DateTime.UtcNow;
        }
        public void SetDeleted(Guid? deletedByUserId)
        {
            DeletedByUserId = deletedByUserId;
            DeletedDate = DateTime.UtcNow;
            IsDeleted = true;
        }
        public void SetActive(bool isActive, Guid? updatedByUserId)
        {
            if (IsActive != isActive)
            {
                IsActive = isActive;
            }
        }
    }
}
