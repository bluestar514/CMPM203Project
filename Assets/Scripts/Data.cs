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
   //Classes pr;
    public bool hasMark = false;
    public string hairColor;
    public string eyeColor;
    public float height, weight;
    public int age;

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
        this.age = 100;
    }

    public void GenerateRandomPhysicalCharacteristics()
    {
        
        hairColor = listOfHairColors[Random.Range(0, listOfHairColors.Count)];
        eyeColor = listOfEyeColors[Random.Range(0, listOfEyeColors.Count)];
        height = Random.Range(5.0f, 10.0f);
        weight = Random.Range(50f, 300);
        hasMark = Random.value > 0.5f;
        //parents 
        age = Random.Range(30, 50);//for now 
    }

    public void GenerateFromExisting(PhysicalCharactristics inputA, PhysicalCharactristics inputB,int order)
    {//for child 
        //hair: 
        int hereditaryWeight = listOfHairColors.Count;
        listOfHairColors.AddRange(Enumerable.Repeat(inputA.hairColor, hereditaryWeight));
        listOfHairColors.AddRange(Enumerable.Repeat(inputB.hairColor, hereditaryWeight));

        //eyes:
        hereditaryWeight = listOfEyeColors.Count;
        listOfEyeColors.AddRange(Enumerable.Repeat(inputA.eyeColor, hereditaryWeight));
        listOfEyeColors.AddRange(Enumerable.Repeat(inputB.eyeColor, hereditaryWeight));

        GenerateChildPhysicalCharacteristics( inputA,  inputB,  order);
    }

    public void GenerateChildPhysicalCharacteristics(PhysicalCharactristics inputA, PhysicalCharactristics inputB,int order)
    {
        hairColor = listOfHairColors[Random.Range(0, listOfHairColors.Count)];
        eyeColor = listOfEyeColors[Random.Range(0, listOfEyeColors.Count)];
        weight = Random.Range(50f, 200);
        hasMark = Random.value > 0.25f;
        height = Random.Range(4, Mathf.Min(inputA.height, inputB.height));
        age = Random.Range(7, Mathf.Min(inputA.age, inputB.age));
    //check notes ---- below -- *A can go here if we need specs 


    }

}



[System.Serializable]
public class Profession
{
    //List<string> titles = new List<string> { "Rouge", "Mage", "Hunter", "Witch", "Other" };
    //list traits for each one? 
    // link possible traits with classess and if statments in structures ---- -
    public string title;
    protected List<Trait> potentialTraits = new List<Trait>();
    public List<Trait> chosenTraits = new List<Trait>();

    public static Profession PickProfession() {
        return new MageProfession();
    }

}

[System.Serializable]
public class MageProfession: Profession
{
    public MageProfession()
    {
        title = "Mage";
        potentialTraits = new List<Trait> { new Trait("Chosen By Mana", "This person's spells cost 10% less mana to cast."),
                                            new Trait("Bibliophile", "This person is more likely to be in found in the library than anywhere else.")};
        chosenTraits.Add(potentialTraits[Mathf.RoundToInt(Random.Range(0, potentialTraits.Count))]);
    }
}


[System.Serializable]
public class Trait
{
    public string name;
    public string description;

    public Trait(string Name, string Desc)
    {
        name = Name;
        description = Desc;
    }
}

[System.Serializable]
public class Family
{
    public List<Character> members = new List<Character>();
    public int familyID = 0;
    public Family()
    {

    }
    public Family(List<Character> _familyMembers,int _familyID)
    {
        this.members = _familyMembers;
        this.familyID = _familyID;
    }

}

//------ notes ---*A can go here if we need specs 
//option to tweak if needed be - little buggy but works 
//if(order == 1)//first child
//{
//    height = Mathf.Min(inputA.height, inputB.height)-1.5f;
//    age = Random.Range(18, Mathf.Min(inputA.age, inputB.age)-10); 
//}
//if (order == 2)//first child
//{
//    height = Mathf.Min(inputA.height, inputB.height) - 2f;
//    age = Random.Range(12, Mathf.Min(inputA.age, inputB.age) - 15);
//}
//else
//{
//    height = Random.Range(4, Mathf.Min(inputA.height, inputB.height))-3f;
//    age = Random.Range(7, Mathf.Min(inputA.age, inputB.age) -20 );
//}