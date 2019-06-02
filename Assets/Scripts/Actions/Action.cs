using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action
{
    public string name;
    public float baseDesire;
    public List<ConditionModifier> traitModifiers;
    //public List<TraitModifier> targetModifiers;


    public Action(string name, float baseDesire = .1f, List<ConditionModifier> traitModifiers = null)
    {
        this.name = name;
        this.baseDesire = baseDesire;

        if (traitModifiers != null) this.traitModifiers = traitModifiers;
        else this.traitModifiers = new List<ConditionModifier>();

    }

    public virtual void Enact(Character actor, Tile location, string target, List<ActionTargetDesire> potentialConsiderations)
    {
        FormMemory(actor, location, target, potentialConsiderations);
    }

    public virtual float Desire(Character actor, Tile location, string target)
    {
        float currentDesire = baseDesire;

        return currentDesire;

    }

    public virtual void FormMemory(Character actor, Tile location, string target, List<ActionTargetDesire> potentialConsiderations)
    {
        actor.AddMemory(name, potentialConsiderations);
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

    protected Tile FindConnection(string target, Tile location)
    {
        foreach(Tile loc in location.connectedTiles) {
            if (target == loc.tileID.ToString()) return loc;
        }
        return null;
    }

    protected float ModifyDesire(float currentDesire, float modifier)
    {
        return currentDesire + (baseDesire * modifier);
    }
}

public class TraitModifier {
    public string id;
    public float modifier;

    public TraitModifier(string id, float modifier)
    {
        this.id = id;
        this.modifier = modifier;
    }
}



