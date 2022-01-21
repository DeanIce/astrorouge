using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class PersistentUpgrades : MonoBehaviour
{
    public class Data
    {
        //needed variables here (public)
        public int num;
    }
    
    XmlSerializer serializer = new XmlSerializer(typeof(Data));
    private static PersistentUpgrades instance;
    //put any needed variables here (public)
    public int num;
    
    // Start is called before the first frame update
    void Start()
    {
        //put initial states here
        num = 0;
        if (instance == null)
        {
            instance = this;
            instance.Load();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //synchronize local variables with those in game
        SampleManager manager = FindObjectOfType<SampleManager>();
        num = manager.num;
    }

    public void Save()
    {
        Data data = new Data();
        //set variables here- set equal to the Persistent upgrades version
        data.num = num;
        
        string destination = Application.persistentDataPath + "/save.dat";

        using (var file = File.Open(destination, FileMode.Create))
        {
            serializer.Serialize(file, data);
        }
    }

    public void Load()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        Data data;

        if (!File.Exists(destination))
        {
            //error handling
            return;
        }

        using (var file = File.Open(destination, FileMode.Open))
        {
            data = (Data)serializer.Deserialize(file);
        }

        //synchronize with persistent upgrades version
        num = data.num;
    }
}