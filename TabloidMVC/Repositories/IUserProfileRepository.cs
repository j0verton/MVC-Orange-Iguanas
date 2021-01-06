using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        public List<UserProfile> GetAllActiveUserProfiles();
        public List<UserProfile> GetAllInactiveUserProfiles();
        UserProfile GetById(int id);
        public void DeactivateUser(int id);
        public void ReactivateUser(int id);
    }
}