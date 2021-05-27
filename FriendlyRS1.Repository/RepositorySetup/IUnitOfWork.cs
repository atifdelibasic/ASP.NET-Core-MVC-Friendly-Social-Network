using DataLayer.Repositories;
using FriendlyRS1.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendlyRS1.Repository.RepostorySetup
{
    public interface IUnitOfWork
    {
        public HobbyRepository hobby { get; set; }
        public HobbyCategoryRepository hobbyCategory { get; set; }
        public UserRepository User { get; set; }
        public GenderRepository Gender { get; }
        public CountryRepository Country { get; }
        public FriendshipRepository Friendship { get; }
        public CityRepository City { get; }
        public FriendshipStatusRepository FriendshipStatus { get; }
        public ApplicationUserHobbyRepository UserHobby { get; set; }
        public PostRepository Post { get; }
        public BellNotificationRepository BellNotification { get; }
        public SkillRepository Skill { get; }

        int Complete();
    }
}
