using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action //social
{
    public string name = "social";
    public float baseDesire = .2f;
    public float familyModifier = 1.5f;
    public float friendlyModifier = 2f;
    public float enemyModifier = -.75f;

    public virtual void Enact(Character actor, Tile location, string target)
    {
        FormMemory(actor, location, target);
    }

    public virtual float Desire(Character actor, Tile location, string target)
    {
        float currentDesire = baseDesire;

        Character targetCharacter = FindInRoom(target, location);

        if (targetCharacter == null) return 0; //Action is not possible

        if (Relation.AreFamily(actor, targetCharacter)) currentDesire += (currentDesire * familyModifier);
        if (Relation.AreFriendly(actor, targetCharacter)) currentDesire += (currentDesire * friendlyModifier);
        if (Relation.AreEnemy(actor, targetCharacter)) currentDesire += (currentDesire * enemyModifier);

        return Mathf.Max(currentDesire, 0);

    }



    public virtual void FormMemory(Character actor, Tile location, string target)
    {
        actor.AddMemory(name + " with " + target);
    }


    Character FindInRoom(string target, Tile location)
    {
        List<Character> combinedVisitors = new List<Character>();
        combinedVisitors.AddRange(location.populatedNPCs);
        combinedVisitors.AddRange(location.populatedPatry);
        

        foreach (Character character in combinedVisitors) {
            if (character.name == target) {
                return character;
            }
        }
        return null;
    }
}
