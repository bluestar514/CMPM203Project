using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        famalies = townGenerator.GenerateFamalies(20, 4);//exceeds family count tho
        townGenerator.AddRandomRelations(famalies);
        //residents = townGenerator.GenerateResidents(10, 2);//copying this method for family structure for now commenting here 

    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
