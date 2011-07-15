using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace PersonalCenter.Service.User
{
    public interface IUserLogic
    {
        UserView Login(string account, string password);

        void EditUserInfo(UserView view);

        void ChangeSkin(UserView view);
    }
}
