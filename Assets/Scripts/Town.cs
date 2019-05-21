using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Town : MonoBehaviour
{

    //public int numberOfNpcs = 10;
    //public List<Charecter> charecters;

    Character test;
    //public List<Character> residents;
    public List<Family> famalies; 
     void Start()
    {
        TownGenerator townGenerator = new TownGenerator();
        famalies = townGenerator.GenerateFamalies(20, 4);//off by 1 or -1 
        townGenerator.AddRandomRelations(famalies);
        //residents = townGenerator.GenerateResidents(10, 2);//copying this method for family structure for now commenting here 
        string json = JsonUtility.ToJson(famalies[0].members[0].name);
        Debug.Log(json);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
