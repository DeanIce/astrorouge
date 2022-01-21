using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class PersistentUpgrades
{
    //make this a coroutine if synchronization issues occur
    public static void Save(Data data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        string destination = Application.persistentDataPath + "/save.dat";

        using (var file = File.Open(destination, FileMode.Create))
        {
            serializer.Serialize(file, data);
        }
    }

    //make this a coroutine if synchronization issues occur
    public static Data Load()
    {
        Data data;
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        string destination = Application.persistentDataPath + "/save.dat";
        if (!File.Exists(destination))
        {
            return new Data();
        }

        using (var file = File.Open(destination, FileMode.Open))
        {
            data = (Data)serializer.Deserialize(file);
        }

        return data;
    }
}