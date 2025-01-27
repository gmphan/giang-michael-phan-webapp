using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.BusinessAccessLib
{
    public interface IProfilePictureServ
    {
        public Task<ProfilePicture> GetPictureAsync();
        public Task AddNewPictureAsync(ProfilePicture profilePicture);
        public Task RemovePictureAsync(int id);
    }
}