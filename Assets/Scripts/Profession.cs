using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Profession {
    public string title;
    protected List<Trait> potentialTraits = new List<Trait>();
    public List<Trait> chosenTraits = new List<Trait>();

    public static Profession PickAdventurerProfession()
    {
        Profession profession;
     
        switch(Random.Range(0, 8)) {
            case 0:
                profession = new MageProfession();
                break;
            case 1:
                profession = new ShamanProfession();
                break;
            case 2:
                profession = new KnightProfession();
                break;
            case 3:
                profession = new RogueProfession();
                break;
            default:
                profession = new VillagerProfession();
                break;
        }
        
        profession.ChooseProfessionTraits();

        return profession;
    }

    public static Profession PickVillagerProfession()
    {
        return new VillagerProfession();
    }

    public static Profession PickChildProfession()
    {
        return new ChildProfession();
    }

    public static Profession PickProfessionBasedOnParents(Profession professionA, Profession professionB)
    {
        List<Profession> potentialProfessions = new List<Profession> { new MageProfession(), new ShamanProfession(), new KnightProfession(), new RogueProfession(), new VillagerProfession() };
        int hereditaryWeight = potentialProfessions.Count;
        if (professionA != null) {
            potentialProfessions.AddRange(Enumerable.Repeat(professionA, hereditaryWeight));
        }
        if (professionB != null) {
            potentialProfessions.AddRange(Enumerable.Repeat(professionB, hereditaryWeight));
        }

        return potentialProfessions[Random.Range(0, potentialProfessions.Count)];
    }

    public void ChooseProfessionTraits()
    {
        chosenTraits.Add(potentialTraits[Random.Range(0, potentialTraits.Count)]);
    }

}

[System.Serializable]
public class MageProfession : Profession {
    public MageProfession()
    {
        title = "Mage";
        potentialTraits = new List<Trait> { new Trait("Chosen By Mana", "This person's spells cost 10% less mana to cast."),
                                            new Trait("Bibliophile", "This person is more likely to be in found in the library than anywhere else."),
                                            new Trait("Introverted", "This person prefers other activities over chatting with others")};
    }
}


[System.Serializable]
public class ShamanProfession : Profession {
    public ShamanProfession()
    {
        title = "Shaman";
        potentialTraits = new List<Trait> {
            new Trait("One with Nature", "This person would rather be in places of nature than not."),
            new Trait("Team Orriented", "This person sticks with the team, if they have one.")
        };
    }
}


[System.Serializable]
public class KnightProfession : Profession {
    public KnightProfession()
    {
        title = "Knight";
        potentialTraits = new List<Trait> {
            new Trait("Chatty", "This person loves to talk with the people."),
            new Trait("Tough", "This person takes 10% less damage in fights."),
            new Trait("Monster Hunter", "This person abhors monsters and would see all of them dead.")
        };
    }
}

[System.Serializable]
public class RogueProfession : Profession {
    public RogueProfession()
    {
        title = "Rogue";
        potentialTraits = new List<Trait> {
            new Trait("Inn Lover", "This person prefers to spend their time in inns, particularly inns with drinks."),
            new Trait("Lone Wolf", "This person doesn't work well with others and often splits off from their party if they are part of one."),
            new Trait("Shifty", "This person is kind of shady looking, others don't trust them and would rather not associate with them.")
        };
    }
}

public class VillagerProfession: Profession {
    public VillagerProfession()
    {
        title = "Villager";
        potentialTraits = new List<Trait> { new Trait("Villager", "Not everyone is destined for greatness, or even movement. This is one of those people.") };
    }
}

public class ChildProfession: Profession {
    public ChildProfession()
    {
        title = "Child";
        potentialTraits = new List<Trait> { new Trait("Child", "Some say being a kid is a full time job. They would be right.") };
    }
}

[System.Serializable]
public class Trait {
    public string name;
    public string description;

    public Trait(string Name, string Desc)
    {
        name = Name;
        description = Desc;
    }
}