using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//for now it is not in json - will transfter later once 
//faster for me 


public class Data : MonoBehaviour 
{

    public int numberOfNpcs = 10;
    public List<string> listOfGeneratedNames = new List<string>();
    List<string> partOne = new List<string> { "Mar", "Anna", "Hey", "Ase", "Ro", "Cla", "Zen", "Ni", "Lin", "Re" };
    List<string> partTwo = new List<string> { "ly", "nama", "meer", "nia", "Ro", "ra", "soar", "ght", "tera", "ey", "oul", "ra","beth" };




    // Start is called before the first frame update
    void Start()

    {
        for(int i =0; i <= numberOfNpcs; i++)
        {
            listOfGeneratedNames.Add(
                    partOne[Random.Range(0, partOne.Count)] + partTwo[Random.Range(0, partTwo.Count)]);
        }
        printAcharecter();
        printAcharecter();
        printAcharecter();

    }

    public void printAcharecter() //this will return it later instead of print it or write it to json 
    {
        string thisname = listOfGeneratedNames[Random.Range(0, listOfGeneratedNames.Count - 1)];
        PhysicalCharectristcs p = new PhysicalCharectristcs();
        Debug.Log(thisname + " has eye: " + p.eyeColor + "" +
            " has hair color: " + p.hairColor +
            " height:  " + p.height +
            "and weight" + p.weight +
            "does this charcter have a scar?" + p.hasMark);
        //will return it here and link it 
        // need to store this somehow --- and then check if anyone in the list has the sam ehair color? refine relationship 

    }

    public PhysicalCharectristcs returnPhysicalCharectristcs()
    {
        //return a random phisical chartrictic 
        PhysicalCharectristcs p = new PhysicalCharectristcs();
        return p;
    }

    //public string returnAname()
    //{
    //    //return a random phisical chartrictic 
    //   return  listOfGeneratedNames[Random.Range(0, listOfGeneratedNames.Count - 1)];
      
    //}





}

public class PhysicalCharectristcs :Data
{

    List<string> listOfColors = new List<string> { "Blue", "Black", "Red", "Orange", "Pink" }; //thuis might change to color object
    List<string> listOfeyeColors = new List<string> { "Blue", "Black", "green", "Gold" };
    Classes pr;
    public bool hasMark = false;
    public string hairColor;
    public string eyeColor;
    public float height, weight;

    //might get alot related this way - mayeb another approuch --- 
    public PhysicalCharectristcs()
    {
        hairColor = listOfColors[Random.Range(0, listOfColors.Count)];
        eyeColor = listOfeyeColors[Random.Range(0, listOfeyeColors.Count)];
        height = Random.Range(4.0f, 10.0f);
        weight = Random.Range(50f, 300);
        hasMark = Random.value > 0.5f;


    }

   


}

public class Classes
{
    List<string> titiles = new List<string> { "Rouge", "Mage", "Hunter", "Witch", "Null" };
    //list traits for each one? 
    // link possible traits with classess and if statments in structures ---- -
    struct traits
    {
        string name;
        string ability;
        string affeliation;
    }


}


