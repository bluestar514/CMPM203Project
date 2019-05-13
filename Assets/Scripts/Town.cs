using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{

    //public int numberOfNpcs = 10;
    //public List<Charecter> charecters;
    //// Start is called before the first frame update
    /// 
    /// 
    Character test;
    public List<Character> residents;
     void Start()
    {
        TownGenerator townGenerator = new TownGenerator();
        residents = townGenerator.GenerateResidents(10, 2);


        //test = new Character();
        //if (test.pc.hasMark)
        //{
        //    Debug.Log("the char has a scar");
        //}

        //for (int i = 0; i <= numberOfNpcs; i++)
        //{
        //    charecters.Add(new Charecter());
        //}
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
