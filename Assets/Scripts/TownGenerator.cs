using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator 
{

    public List<Character> GenerateResidents(int numberOfResidents, int numberOfFamilies)
    {
        List<Character> residents = new List<Character>();
        int numGeneratedResidents = 0;
        for(int numGeneratedFamilies = 0; numGeneratedFamilies < numberOfFamilies; numberOfFamilies++) {
            int numberOfMembers = Random.Range(1, Mathf.Min(8, numberOfResidents-numGeneratedResidents));
            numGeneratedResidents += numberOfMembers;
            residents.AddRange(GenerateFamily(numberOfMembers));
            if (numGeneratedResidents >= numberOfResidents) break;
        }


        return residents;
        
    }

    public List<Character> GenerateFamily(int numMembers)
    {
        List<Character> members = new List<Character>();

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
        for(int i = 2; i<= numMembers; i++) {
            members.Add(GenerateChild(mother, father));

        }

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

    public Character GenerateChild(Character mother, Character father)
    {
        Character character = new Character();
        character.data.GenerateName();
        character.name = character.data.name;
        character.pc.GenerateFromExisting(mother.pc, father.pc);

        character.AddRelation(mother, "mother");
        character.AddRelation(father, "father");

        mother.AddRelation(character, "child");
        father.AddRelation(character, "child");

        return character;
    }
}

