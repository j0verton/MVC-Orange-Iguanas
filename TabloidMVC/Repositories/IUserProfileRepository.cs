using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);

        public List<UserProfile> GetAllUserProfiles();

       void RegisterUser(UserProfile user);

        UserProfile GetById(int id);

    }
}