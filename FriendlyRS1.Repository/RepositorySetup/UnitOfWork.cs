using DataLayer.EntityModels;
using DataLayer.Repositories;
using FriendlyRS1.Repository.Repositories;
using FriendlyRS1.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendlyRS1.Repository.RepostorySetup
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        public HobbyRepository hobby { get; set; }
        public HobbyCategoryRepository hobbyCategory { get; set; }
        public ApplicationUserHobbyRepository UserHobby { get; set; }

        public UserRepository User { get; set; }
        private GenderRepository GenderRepository;
        public GenderRepository Gender
        {
            get
            {
                if (this.GenderRepository == null)
                {
                    GenderRepository = new GenderRepository(context);
                }
                return GenderRepository;
            }
        }
        private CountryRepository CountryRepository;
        public CountryRepository Country
        {
            get
            {
                if (CountryRepository == null)
                {
                    CountryRepository = new CountryRepository(context);
                }
                return CountryRepository;
            }
        }
        private FriendshipStatusRepository FriendshipStatusRepository;
        public FriendshipStatusRepository FriendshipStatus
        {
            get
            {
                if (FriendshipStatusRepository == null)
                {
                    FriendshipStatusRepository = new FriendshipStatusRepository(context);
                }
                return FriendshipStatusRepository;
            }
        }

        // friendship
        private FriendshipRepository FriendshipRepository;
        public FriendshipRepository Friendship
        {
            get
            {
                if (this.FriendshipRepository == null)
                {
                    this.FriendshipRepository = new FriendshipRepository(context);
                }
                return FriendshipRepository;
            }
        }
        // city
        private CityRepository CityRepository;

        public CityRepository City
        {
            get
            {
                if (this.CityRepository == null)
                {
                    this.CityRepository = new CityRepository(context);
                }
                return CityRepository;
            }
        }

        private PostRepository PostRepository;
        public PostRepository Post
        {
            get
            {
                if (this.PostRepository == null)
                {
                    this.PostRepository = new PostRepository(context);
                }
                return PostRepository;
            }
        }

        private BellNotificationRepository BellNotificationRepository;
        public BellNotificationRepository BellNotification
        {
            get
            {
                //return this.BellNotificationRepository ?? new BellNotificationRepository(context);
                if (this.BellNotificationRepository == null)
                {
                    this.BellNotificationRepository = new BellNotificationRepository(context);
                }
                return BellNotificationRepository;
            }
        }

        private SkillRepository SkillRepository;
        public SkillRepository Skill
        {
            get
            {
                if (SkillRepository == null)
                    SkillRepository = new SkillRepository(context);
                return SkillRepository;
            }
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            hobby = new HobbyRepository(context);
            hobbyCategory = new HobbyCategoryRepository(context);
            User = new UserRepository(context);
            UserHobby = new ApplicationUserHobbyRepository(context);
        }

        public int Complete()
        {
            return context.SaveChanges();
        }
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            context.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
