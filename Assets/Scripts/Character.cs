﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//acts as a collector for all data types related to a charcter 
[System.Serializable]
public class Character
{
    public string name;
    public PhysicalCharactristics pc;
    public Data data; // at this point read it from json 
    public List<Relation> relations;

    public Character()
    {
        relations = new List<Relation>();
        pc = new PhysicalCharactristics();
        data = new Data();
        //Debug.Log(pc.eyeColor);

    }
    // Start is called before the first frame update
    void Start()
    {
        //Data = FindObjectOfType<Data>();
        //pc = Data.returnPhysicalCharectristcs();

        //pc = new PhysicalCharectristcs();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRelation(Character otherCharacter, string relationship)
    {
        relations.Add(new Relation(otherCharacter, relationship));
    }

    public void printAcharecter() //this will return it later instead of print it or write it to json 
    {
        
        PhysicalCharactristics p = new PhysicalCharactristics();
        Debug.Log(data.name + "\n has eye: " + p.eyeColor + "" +
            "\n has hair color: " + p.hairColor +
            "\n height:  " + p.height +
            "\n and weight" + p.weight +
            "\n does this charcter have a scar?" + p.hasMark);
        //will return it here and link it 
        // need to store this somehow --- and then check if anyone in the list has the sam ehair color? refine relationship 

    }

    //public Charecter() {
    //    if (pc != null)
    //    {
    //        pc = Data.returnPhysicalCharectristcs();
    //    }
    //    name = Data.returnAname();
    //}
}

[System.Serializable]
public class Relation {
    [System.NonSerialized]
    public Character otherCharacter;
    public string name;
    public string relationship;

    public Relation(Character otherCharacter, string relationship)
    {
        this.otherCharacter = otherCharacter;
        name = otherCharacter.data.name;
        this.relationship = relationship;
    }
}