using DataLayer.EntityModels;
using FriendlyRS1.Repository.RepostorySetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendlyRS1.Repository.Repositories
{
    public class BellNotificationRepository : Repository<BellNotification>
    {
        private ApplicationDbContext _db;
        public BellNotificationRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public List<BellNotification> GetNotifications(int id, int take, int skip)
        {
            List<BellNotification> notifications = _db.BellNotification.Where(x => x.NotifierId == id)
                .Select(x => new BellNotification
                {
                    Id = x.Id,
                    Actor = x.Actor,
                    NotificationType = x.NotificationType,
                    DateCreated = x.DateCreated,
                    IsRead = x.IsRead,
                    ActorId = x.ActorId,
                    NotificationTypeId = x.NotificationTypeId,
                    NotifierId = x.NotifierId
                })
                .OrderByDescending(x => x.DateCreated)
                .Skip(skip)
                .Take(take)
                .ToList();

            return notifications;
        }

        public void MarkAsRead()
        {

            IQueryable<BellNotification> bell = _db.BellNotification.Where(x => x.IsRead == false);

            foreach (BellNotification x in bell)
            {
                x.IsRead = true;
                Update(x);
            }

            _db.SaveChanges();
        }

        public int CountUnreadNotifications(int id)
        {
            return _db.BellNotification.Where(x => x.NotifierId == id).Count(x => !x.IsRead);
        }

        public override void Update(BellNotification entityToUpdate)
        {
            base.Update(entityToUpdate);
        }
    }
}
