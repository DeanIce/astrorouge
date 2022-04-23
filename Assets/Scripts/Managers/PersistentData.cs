using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Managers
{
    public static class PersistentData
    {
        //make this a coroutine if synchronization issues occur
        public static void Save(Data data)
        {
            var serializer = new XmlSerializer(typeof(Data));
            var destination = Application.persistentDataPath + "/save.dat";

            using (var file = File.Open(destination, FileMode.Create))
            {
                serializer.Serialize(file, data);
            }
        }

        //make this a coroutine if synchronization issues occur
        public static Data Load()
        {
            Data data;
            var serializer = new XmlSerializer(typeof(Data));
            var destination = Application.persistentDataPath + "/save.dat";
            if (!File.Exists(destination)) return new Data();

            using (var file = File.Open(destination, FileMode.Open))
            {
                data = (Data) serializer.Deserialize(file);
            }

            return data;
        }

        public static void Save<T>(T data, string name)
        {
            var serializer = new XmlSerializer(typeof(T));
            var destination = Application.persistentDataPath + "/" + name + ".dat";

            using (var file = File.Open(destination, FileMode.Create))
            {
                serializer.Serialize(file, data);
            }
        }

        public static T Load<T>(string name) where T : new()
        {
            T data;
            var serializer = new XmlSerializer(typeof(T));
            var destination = Application.persistentDataPath + "/" + name + ".dat";
            if (!File.Exists(destination)) return new T();

            using (var file = File.Open(destination, FileMode.Open))
            {
                data = (T) serializer.Deserialize(file);
            }

            return data;
        }
    }
}