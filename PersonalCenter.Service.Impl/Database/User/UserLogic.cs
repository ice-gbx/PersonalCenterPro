using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersonalCenter.Service.User;
using PersonalCenter.Entity;

namespace PersonalCenter.Service.Impl.Database.User
{
    public class UserLogic : IUserLogic
    {
        private PersonalCenterEntities Context;

        public UserLogic()
        {
            Context = new PersonalCenterEntities();
        }

        public UserView Login(string account, string password)
        {
            var user = Context.Users.
                Where(n => n.Account == account && n.Password == password).
                Select(n => new UserView 
                { 
                    Id = n.Id,
                    Account = n.Account,
                    Email = n.Email,
                    Password = n.Password,
                    Uname = n.Uname,
                    Skin = n.Skin
                }).FirstOrDefault();

            return user;
        }


        public void EditUserInfo(UserView view)
        {
            var user = Context.Users.Where(n => n.Id == view.Id).Select(n => n).FirstOrDefault();
            user.Account = view.Account;
            user.Uname = view.Uname;
            user.Email = view.Email;
            user.Password = view.Password;
            Context.SaveChanges();
        }


        public void ChangeSkin(UserView view)
        {
            var user = Context.Users.Where(n => n.Id == view.Id).Select(n => n).FirstOrDefault();
            user.Skin = view.Skin;
            Context.SaveChanges();
        }
    }
}
