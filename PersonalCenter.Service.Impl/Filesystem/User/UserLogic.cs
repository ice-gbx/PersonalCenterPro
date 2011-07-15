using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersonalCenter.Service.User;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace PersonalCenter.Service.Impl.Filesystem.User
{
    public class UserLogic : IUserLogic
    {
        private string path = Process.GetCurrentProcess().MainModule.FileName.
            Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf("\\")) + @"\pcui.bin";

        public void EditUserInfo(UserView view)
        {
            SerializeUtils.Serialize(path, view);
        }

        public void ChangeSkin(UserView view)
        {
            SerializeUtils.Serialize(path, view);
        }

        public UserView Login(string account, string password)
        {
            UserView user = null;

            try
            {
                var res = SerializeUtils.Deserialize(path) as UserView;
                if (res != null
                    && res.Account.Equals(account, StringComparison.CurrentCultureIgnoreCase)
                    && res.Password == password)
                {
                    user = res;
                }
            }
            catch (SerializationException)
            {
                Init();
            }

            return user;
        }

        public void Init()
        {
            UserView user = new UserView
                            {
                                Id = 1,
                                Account = "Ice",
                                Uname = "彬哥",
                                Password = "ice",
                                Email = "ice@you-may.cn"
                            };

            SerializeUtils.Serialize(path, user);
        }
    }
}
