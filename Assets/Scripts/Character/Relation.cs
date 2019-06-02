using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        name = otherCharacter.name;
        this.relationship = relationship;
        this.isChild = isitaCHILD;
    }
    public Relation(Character otherCharacter, string relationship)
    {
        this.otherCharacter = otherCharacter;
        name = otherCharacter.name;
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