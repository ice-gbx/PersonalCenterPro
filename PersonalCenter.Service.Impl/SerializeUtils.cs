using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace PersonalCenter.Service.Impl
{
    public class SerializeUtils
    {
        public static int Serialize(string path, object obj)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, obj);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public static object Deserialize(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(fs);
                }
                else
                    throw new SerializationException();
            }
        }
    }
}
