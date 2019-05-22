using System.Collections;
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
    public Profession profession;
    public List<string> memoryLog;

    public Character()
    {
        relations = new List<Relation>();
        pc = new PhysicalCharactristics();
        data = new Data();
        profession = null;
    }

    

    public void AddRelation(Character otherCharacter, string relationship)
    {
        relations.Add(new Relation(otherCharacter, relationship, relationship=="mother"|| relationship=="father"?true:false));
    }
    public void AddRelation(Character otherCharacter, string relationship, int thisFamId) 
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

    public Character FindRelation(string relationship)
    {
        foreach(Relation relation in relations) {
            if(relation.relationship == relationship) {
                return relation.otherCharacter;
            }
        }

        return null;
    }

    public Relation FindRelationBetween(Character otherCharacter)
    {
        foreach (Relation relation in relations) {
            if (relation.otherCharacter == otherCharacter) {
                return relation;
            }
        }

        return null;
    }

    public void AddMemory(string memory)
    {
        memoryLog.Add(memory);
    }

}

[System.Serializable]
public class Relation {
    [System.NonSerialized]
    public Character otherCharacter;
    public string name;
    public string relationship;
   public bool isChild = false;

    public Relation(Character otherCharacter, string relationship, bool isitaCHILD)
    {
        this.otherCharacter = otherCharacter;
        name = otherCharacter.data.name;
        this.relationship = relationship;
        this.isChild = isitaCHILD;
    }
    public Relation(Character otherCharacter, string relationship)
    {
        this.otherCharacter = otherCharacter;
        name = otherCharacter.data.name;
        this.relationship = relationship;
       
    }

    public static bool AreARelation(Character a, Character b, List<string> relationList)
    {
        
        Relation relation = null;
        foreach (string potential in relationList) {
            relation = a.FindRelationBetween(b);
            if (relation != null) {
                return true;
            }
        }

        return false;

    }

    public static bool AreFamily(Character a, Character b)
    {
        List<string> familialRelations = new List<string> { "mother", "father", "sibling", "husband", "wife" };
        return AreARelation(a, b, familialRelations);

    }

    public static bool AreFriendly(Character a, Character b)
    {
        List<string> friendlyRelations = new List<string> { "best buds", "friend" };
        return AreARelation(a, b, friendlyRelations);
    }

    public static bool AreEnemy(Character a, Character b)
    {
        List<string> enemyRelations = new List<string> { "mortal enemy" };
        return AreARelation(a, b, enemyRelations);
    }
}