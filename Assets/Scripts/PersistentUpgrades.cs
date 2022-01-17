using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class PersistentUpgrades : MonoBehaviour
{
    XmlSerializer serializer = new XmlSerializer(typeof(string));
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
        string destination = "";
        string output = "";

        destination = Application.persistentDataPath + "/save.dat";

        //output += <variable>.ToString() + "";

        using (var file = File.Open(destination, FileMode.Create))
        {
            serializer.Serialize(file, output);
        }
    }

    public void Load()
    {
        string destination = "", input;
        int count, start = 0;
        char[] inputChars;

        destination = Application.persistentDataPath + "/save.dat";

        if (!File.Exists(destination))
        {
            //error handling
            return;
        }

        using (var file = File.Open(destination, FileMode.Open))
        {
            input = (string)serializer.Deserialize(file);
        }
        
        inputChars = input.ToCharArray();

        //3 lines needed to get a variable (note first and third lines will always be the same):
        //count = GetNextData(start, inputChars);
        //<variablename> = input.Substring(start, count); convert type if needed via int.Parse, etc
        //start += count + 1;
    }

    private int GetNextData(int start, char[] inputChars)
    {
        int count = 0;

        while (inputChars[start + count] != ' ')
        {
            count++;
        }

        return count;
    }
}