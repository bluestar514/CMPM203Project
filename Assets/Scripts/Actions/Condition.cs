using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionModifier
{
    public List<string> traitModifiers;
    public List<string> targetModifiers;
    public float modifier;

    public ConditionModifier(string trait="", string targetTrait="", float modifier=1)
    {
        if (trait != "") traitModifiers = new List<string> { trait };
        else traitModifiers = new List<string>();

        if (targetTrait != "") targetModifiers = new List<string> { targetTrait };
        else targetModifiers = new List<string>();

        this.modifier = modifier;
    }

    public ConditionModifier(List<string> traitModifiers=null, List<string> targetModifiers=null, float modifier=1)
    {
        if (traitModifiers != null) this.traitModifiers = traitModifiers;
        else this.traitModifiers = new List<string>();

        if (targetModifiers != null) this.targetModifiers = targetModifiers;
        else this.targetModifiers = new List<string>();


        this.modifier = modifier;
    }

    public bool EvaluateWithCharacter(Character actor, Character target)
    {
        foreach (string traitId in traitModifiers) {
            if (!actor.traitList.Exists(x => x.id == traitId)) return false;
        }

        foreach(string traitId in targetModifiers) {
            if (!target.traitList.Exists(x => x.id == traitId)) return false;
        }

        return true;
    }

    public bool EvaluateWithLocation(Character actor, Tile location)
    {
        foreach (string traitId in traitModifiers) {
            if (!actor.traitList.Exists(x => x.id == traitId)) return false;
        }

        foreach (string traitId in targetModifiers) {
            if (!location.traitList.Exists(x => x.id == traitId)) return false;
        }

        return true;
    }
}
