using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Profession {
    public string title;
    protected List<Trait> potentialTraits = new List<Trait>();
    protected List<Action> uniqueActions = new List<Action>();

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

    public List<Trait> ChooseProfessionTraits()
    {
        List<Trait> chosenTraits = new List<Trait>();

        chosenTraits.Add(potentialTraits[Random.Range(0, potentialTraits.Count)]);

        return chosenTraits;
    }

    public List<Action> ChooseUniqueActions()
    {
        List<Action> chosenActions = new List<Action>();
        chosenActions.AddRange(uniqueActions); //I forse potentially having an wider pool of potential actions that not every person of this profession
                                                // can preform, but only someone of this profession might be able to (ex: not every shaman can cast holy shield,
                                                // but only a shaman would be able to)
        return chosenActions;
    }

}

[System.Serializable]
public class MageProfession : Profession {
    public MageProfession()
    {
        title = "Mage";
        potentialTraits = new List<Trait> { new Trait("Chosen By Mana", "mp0", "This person's spells cost 10% less mana to cast."),
                                            new Trait("Bibliophile", "mp1", "This person is more likely to be in found in the library than anywhere else."),
                                            new Trait("Introverted", "mp2", "This person prefers other activities over chatting with others")};

        uniqueActions = new List<Action> { new ActionSocial("Discuss the Nature of Magic", baseDesire: .2f, 
                                                            familyModifier: 1, friendlyModifier: 1, enemyModifier: 1, 
                                                            traitModifiers: new List<ConditionModifier> { new ConditionModifier("mp2", modifier: 2) }) };
    }
}


[System.Serializable]
public class ShamanProfession : Profession {
    public ShamanProfession()
    {
        title = "Shaman";
        potentialTraits = new List<Trait> {
            new Trait("One with Nature", "sp0", "This person would rather be in places of nature than not."),
            new Trait("Team Orriented", "sp1", "This person sticks with the team, if they have one.")
        };

        uniqueActions = new List<Action> { new ActionSocial("Discuss the Beauty of Nature", baseDesire: .2f,
                                                            familyModifier: 1, friendlyModifier: 1.5f, enemyModifier: 1,
                                                            traitModifiers: new List<ConditionModifier> { new ConditionModifier("sp0", modifier: 2) }) };
    }
}


[System.Serializable]
public class KnightProfession : Profession {
    public KnightProfession()
    {
        title = "Knight";
        potentialTraits = new List<Trait> {
            new Trait("Chatty", "kp0", "This person loves to talk with the people."),
            new Trait("Tough", "kp1", "This person takes 10% less damage in fights."),
            new Trait("Monster Hunter", "kp2", "This person abhors monsters and would see all of them dead.")
        };

        uniqueActions = new List<Action> { new ActionSocial("Boast of Heroism", baseDesire: .2f,
                                                            familyModifier: 1, friendlyModifier: 1.5f, enemyModifier: 2,
                                                            traitModifiers: new List<ConditionModifier> { new ConditionModifier("kp0", modifier: 2) }) };
    }
}

[System.Serializable]
public class RogueProfession : Profession {
    public RogueProfession()
    {
        title = "Rogue";
        potentialTraits = new List<Trait> {
            new Trait("Inn Lover", "rp0", "This person prefers to spend their time in inns, particularly inns with drinks."),
            new Trait("Lone Wolf", "rp1", "This person doesn't work well with others and often splits off from their party if they are part of one."),
            new Trait("Shifty", "rp2", "This person is kind of shady looking, others don't trust them and would rather not associate with them.")
        };

        uniqueActions = new List<Action> { new ActionSocial("Pickpocket", baseDesire: .2f,
                                                            familyModifier: -2, friendlyModifier: -1.5f, enemyModifier: 3,
                                                            traitModifiers: new List<ConditionModifier> { new ConditionModifier("rp2", modifier: 3) }) };
    }
}

public class VillagerProfession: Profession {
    public VillagerProfession()
    {
        title = "Villager";
        potentialTraits = new List<Trait> { new Trait("Villager", "vp0", "Not everyone is destined for greatness, or even movement. This is exactly one of those people.") };
    }
}

public class ChildProfession: Profession {
    public ChildProfession()
    {
        title = "Child";
        potentialTraits = new List<Trait> { new Trait("Child", "cp0", "Some say being a kid is a full time job. They would be right.") };
    }
}

[System.Serializable]
public class Trait {
    public string name;
    public string id;
    public string description;

    public Trait(string Name, string ID, string Desc)
    {
        name = Name;
        id = ID;
        description = Desc;
    }
}