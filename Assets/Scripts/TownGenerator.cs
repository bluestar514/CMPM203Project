using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator 
{
    //this method instead adds a layer of families on top of charecters to seperate them ( same as resedints, kept both incase of error ) 
    public List<Family> GenerateFamalies(int maxNumberOfResidents, int numberOfFamilies, int maxFamilySize=8)
    {
        List<Family> families = new List<Family>();
        int familyIDCounter = 0; /// was going to use numgenfamalies but it does not seem to update --- weird thought it did  
        int numGeneratedResidents = 0;
        int numGeneratedFamilies = 0;//fixes issue with counter for somereason
        for (numGeneratedFamilies = 0; numGeneratedFamilies < numberOfFamilies; numGeneratedFamilies++)
        { //do this till we have 2 families
            familyIDCounter++;
            int numberOfMembers = Random.Range(1, Mathf.Min(maxFamilySize, maxNumberOfResidents - numGeneratedResidents));//between, 1 and ( 8 and 0 initially  )- small family 
            numGeneratedResidents += numberOfMembers; //0 -> 8 
            Family aFamily = new Family(GenerateFamily(numberOfMembers), familyIDCounter);
            families.Add(aFamily);
            if (numGeneratedResidents >= maxNumberOfResidents) break; //8 >= 10 ( if we do not exceed number of resefits - otherwise increse family count 
        }

     
        return families;

    }

    public void AddRandomRelations(List<Family> family)
    {
          for (int i = 0; i<=10; i++)
        {
            int selectedFamilyA = Random.Range(0, family.Count);
            int selectedFamilyB = Random.Range(0, family.Count);
            if (family[selectedFamilyA].familyID != family[selectedFamilyB].familyID)
            {// only add a relationship if it is outside the family (  another family ) 
                Character a = family[selectedFamilyA].members[Random.Range(0, family[selectedFamilyA].members.Count)];
                Character b = family[selectedFamilyB].members[Random.Range(0, family[selectedFamilyB].members.Count)];
                addRelationships(a, b, chooseArandomRelation());
            }
        }  }

    string chooseArandomRelation()
    {
        int x = Random.Range(0, 7);
        string relationship;
        switch (x)
        {
            case 0:
                relationship = "best buds";
                break;
            case 1:
                relationship = "admirer";
                break;
            case 2:
                relationship = "mortal enemy";
                break;
            case 3:
                relationship = "mentor";
                break;
            case 4:
                relationship = "acquaintance";
                break;
            case 5:
                relationship = "envious";
                break;
            default:
                relationship = "friend";
                break;

        }
        Debug.Log("added " + relationship);
        return relationship;
    }

    void addRelationships(Character a, Character b, string relationship) //i think at somepoint we need to remove strings and add enums for types 
    {
        a.AddRelation(b, relationship);
    }

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
    {//gives back one family 
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

    public void AssignProfessions(List<Family> families)
    {
        AssignProfessions(FamilyToCharacterList(families));
    }

    public void AssignProfessions(List<Character> residents)
    {
        foreach(Character resident in residents) {
            if(resident.profession == null) AssignProfessionTo(resident);
        }
    }

    public void AssignProfessionTo(Character resident)
    {
        int ageOfAdulthood = 16;
        if (resident.pc.age < ageOfAdulthood) {
            resident.profession = Profession.PickChildProfession();
        } else {
            Profession motherProfession = GetRelationProfession(resident, "mother");
            Profession fatherProfession = GetRelationProfession(resident, "father");
            if (motherProfession != null || fatherProfession != null) {
                resident.profession = Profession.PickProfessionBasedOnParents(motherProfession, fatherProfession);
            } else {
                resident.profession = Profession.PickAdventurerProfession();
            } 
        }
    }

    Profession GetRelationProfession(Character subject, string relationship)
    {
        Character relation = subject.FindRelation(relationship);

        if(relation == null) {
            return null;
        }

        if (relation.profession == null) {
            AssignProfessionTo(relation);
        }

        return relation.profession;
    }

    public List<Character> FamilyToCharacterList(List<Family> families)
    {
        List<Character> residents = new List<Character>();
        foreach(Family family in families) {
            residents.AddRange(family.members);
        }

        return residents;
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


/// was in first method ---   but used temp ver instead ->  //families[numGeneratedFamilies].members = GenerateFamily(numberOfMembers);//works but above is cleaner 
