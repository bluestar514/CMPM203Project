using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



//for now it is not in json - will transfter later once 
//faster for me 

[System.Serializable]
public class Data 
{

    //public int numberOfNpcs = 10;
    //public List<string> listOfGeneratedNames = new List<string>();
    List<string> partOne = new List<string> { "Mar", "Anna", "Hey", "Ase", "Ro", "Cla", "Zen", "Ni", "Lin", "Re" };
    List<string> partTwo = new List<string> { "ly", "nama", "meer", "nia", "Ro", "ra", "soar", "ght", "tera", "ey", "oul", "ra","beth" };

    public string name;

    // Start is called before the first frame update
    void Start()
    {
        //for(int i =0; i <= numberOfNpcs; i++)
        //{
        //    listOfGeneratedNames.Add(
        //            partOne[Random.Range(0, partOne.Count)] + partTwo[Random.Range(0, partTwo.Count)]);
        //}
        //printAcharecter();
        //printAcharecter();
        //printAcharecter();

    }

    public void GenerateName()
    {
        name = partOne[Random.Range(0, partOne.Count)] + partTwo[Random.Range(0, partTwo.Count)];
    }

    public PhysicalCharactristics returnPhysicalCharectristcs()
    {
        //return a random phisical chartrictic 
        PhysicalCharactristics p = new PhysicalCharactristics();
        return p;
    }

    //public string returnAname()
    //{
    //    //return a random phisical chartrictic 
    //   return  listOfGeneratedNames[Random.Range(0, listOfGeneratedNames.Count - 1)];
      
    //}





}


[System.Serializable]
public class PhysicalCharactristics
{

    List<string> listOfHairColors = new List<string> { "Blue", "Black", "Red", "Orange", "Pink" }; //thuis might change to color object
    List<string> listOfEyeColors = new List<string> { "Blue", "Black", "green", "Gold" };
    Classes pr;
    public bool hasMark = false;
    public string hairColor;
    public string eyeColor;
    public float height, weight;

    //might get alot related this way - mayeb another approuch --- 
    public PhysicalCharactristics()
    {
    }

    public PhysicalCharactristics(bool hasMark, string hairColor, string eyeColor, float height, float weight)
    {
        this.hasMark = hasMark;
        this.hairColor = hairColor;
        this.eyeColor = eyeColor;
        this.height = height;
        this.weight = weight;
    }

    public void GenerateRandomPhysicalCharacteristics()
    {
        
        hairColor = listOfHairColors[Random.Range(0, listOfHairColors.Count)];
        eyeColor = listOfEyeColors[Random.Range(0, listOfEyeColors.Count)];
        height = Random.Range(4.0f, 10.0f);
        weight = Random.Range(50f, 300);
        hasMark = Random.value > 0.5f;
    }

    public void GenerateFromExisting(PhysicalCharactristics inputA, PhysicalCharactristics inputB)
    {
        //hair: 
        int hereditaryWeight = listOfHairColors.Count;
        listOfHairColors.AddRange(Enumerable.Repeat(inputA.hairColor, hereditaryWeight));
        listOfHairColors.AddRange(Enumerable.Repeat(inputB.hairColor, hereditaryWeight));

        //eyes:
        hereditaryWeight = listOfEyeColors.Count;
        listOfEyeColors.AddRange(Enumerable.Repeat(inputA.eyeColor, hereditaryWeight));
        listOfEyeColors.AddRange(Enumerable.Repeat(inputB.eyeColor, hereditaryWeight));

        GenerateRandomPhysicalCharacteristics();
    }
}

public class Classes
{
    List<string> titles = new List<string> { "Rouge", "Mage", "Hunter", "Witch", "Null" };
    //list traits for each one? 
    // link possible traits with classess and if statments in structures ---- -
    struct traits
    {
        string name;
        string ability;
        string affeliation;
    }


}