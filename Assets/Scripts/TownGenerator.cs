using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator 
{

    public List<Character> GenerateResidents(int numberOfResidents, int numberOfFamilies)
    {//10,2
        List<Character> residents = new List<Character>();
        int numGeneratedResidents = 0;//counter
        int numGeneratedFamilies = 0;//fixes issue with counter for somereason

        for ( numGeneratedFamilies = 0; numGeneratedFamilies < numberOfFamilies; numberOfFamilies++) {//do this till we have 2 families
            int numberOfMembers = Random.Range(1, Mathf.Min(8, numberOfResidents-numGeneratedResidents));//between, 1 and ( 8 and 0 initially  )- small family 
            numGeneratedResidents += numberOfMembers; //0 -> 8
            residents.AddRange(GenerateFamily(numberOfMembers));//add range dulpicayes elements --
            if (numGeneratedResidents >= numberOfResidents) break; //8 >= 10 ( if we do not exceed number of resefits - otherwise increse family count 
        }


        return residents;
        
    }

    public List<Character> GenerateFamily(int numMembers)
    {
        List<Character> members = new List<Character>();
        List<Character> children = new List<Character>(); ;

        //Generate Mother
        Character mother = GenerateRandomCharacter();
        members.Add(mother);

        if (numMembers == 1) return members;
        
        //Generate Father
        Character father = GenerateRandomCharacter();
        father.AddRelation(mother, "wife");
        mother.AddRelation(father, "husband");

        members.Add(father);

        if (numMembers == 2) return members;

        //Generate Children
        for(int i = 2; i<= numMembers; i++) { //inside the family loop

            Character temp = GenerateChild(mother, father, i - 1);
            children.Add(temp);
            //members.Add(temp);
            //members.Add(GenerateChild(mother, father, i-1));
        }
        for(int i = 0; i<= children.Count;i++) //loop between children of this family and ad d oyherkids as sibilings
        {
            if(i+1< children.Count)
            children[i].AddRelation(children[i+1], "sibling");
        }

        members.AddRange(children);
        return members;
    }

    public Character GenerateRandomCharacter()
    {
        Character character = new Character();
        character.data.GenerateName();
        character.name = character.data.name;
        character.pc.GenerateRandomPhysicalCharacteristics();

        return character;
    }

    public Character GenerateChild(Character mother, Character father,int order)
    {
        Character character = new Character();
        character.data.GenerateName();
        character.name = character.data.name;
        character.pc.GenerateFromExisting(mother.pc, father.pc,order);

        character.AddRelation(mother, "mother");
        character.AddRelation(father, "father");

        mother.AddRelation(character, "child"+order.ToString());
        father.AddRelation(character, "child" + order.ToString());

        return character;
    }
}

////returns members of a family 
//foreach(Character member in members)
//{
//    foreach( Relation r in member.relations)
//    {
//        if(r.relationship == "father" || r.relationship == "mother")
//        {
//            Debug.Log(r.name + " is a child, other relations include: " + r.relationship);
//        }
//        //Debug.Log(member.name + ": has relationships" + r.relationship);
//    }
//}



//// gmae breaking bug  trying to modify relationships prior to sending it to family 
///     for(int i =0; i < members.Count
/// 
//; i++)
    //{
    //    for(int j =0; j<members[i].relations.Count; j++)
    //    {
    //        if (members[i].relations[j].relationship!= "wife" || members[i].relations[j].relationship != "husband")//am i a child and nota parent 
    //        {
    //            members[i].AddRelation(members[i], "sibiling");
    //        }
    //    }


    //}