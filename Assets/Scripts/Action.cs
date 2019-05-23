﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action //social
{
    public string name;
    public float baseDesire;

    public Action(string name, float baseDesire=.1f)
    {
        this.name = name;
        this.baseDesire = baseDesire;
    }

    public virtual void Enact(Character actor, Tile location, string target)
    {
        FormMemory(actor, location, target);
    }

    public virtual float Desire(Character actor, Tile location, string target)
    {
        float currentDesire = baseDesire;

        return Mathf.Max(currentDesire, 0);

    }

    public virtual void FormMemory(Character actor, Tile location, string target)
    {
        actor.AddMemory(name);
    }


    protected Character FindInRoom(string target, Tile location)
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


public class ActionSocial : Action {
    public float familyModifier;
    public float friendlyModifier;
    public float enemyModifier;

    public ActionSocial(string name, float baseDesire=.2f, float familyModifier=1.5f, float friendlyModifier=2f, float enemyModifier=-.75f): base(name, baseDesire)
    {
        this.name = name;
        this.baseDesire = baseDesire;
        this.familyModifier = familyModifier;
        this.friendlyModifier = friendlyModifier;
        this.enemyModifier = enemyModifier;
    }


    public override float Desire(Character actor, Tile location, string target)
    {
        float currentDesire = baseDesire;

        Character targetCharacter = FindInRoom(target, location);

        if (targetCharacter == null) return 0; //Action is not possible

        if (Relation.AreFamily(actor, targetCharacter)) currentDesire += (currentDesire * familyModifier);
        if (Relation.AreFriendly(actor, targetCharacter)) currentDesire += (currentDesire * friendlyModifier);
        if (Relation.AreEnemy(actor, targetCharacter)) currentDesire += (currentDesire * enemyModifier);

        return Mathf.Max(currentDesire, 0);

    }

    public override void FormMemory(Character actor, Tile location, string target)
    {
        actor.AddMemory(name + " with " + target);
    }

}

public class ActionMovement: Action {

    public ActionMovement(string name, float baseDesire=.15f):base(name, baseDesire)
    {
        this.name = name;
        this.baseDesire = baseDesire;
    }

    public override void Enact(Character actor, Tile location, string target)
    {
        base.Enact(actor, location, target);

        //something that moves a character to target location
    }

    public override float Desire(Character actor, Tile location, string target)
    {
        //something that checks if the locations are connected and the path is traversable
        //something that sees how much they might want to do that based on their traits

        return 0; //replace later
    }
    public override void FormMemory(Character actor, Tile location, string target)
    {
        actor.AddMemory("Moved to " + target);
    }
}