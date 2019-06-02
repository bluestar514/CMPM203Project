using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSocial : Action {
    public float familyModifier;
    public float friendlyModifier;
    public float enemyModifier;

    public ActionSocial(string name, float baseDesire = .2f, List<ConditionModifier> traitModifiers = null, 
                        float familyModifier = 1.5f, float friendlyModifier = 2f, float enemyModifier = -.75f) :
            base(name, baseDesire, traitModifiers)
    {
        this.name = name;
        this.baseDesire = baseDesire;
        this.familyModifier = familyModifier;
        this.friendlyModifier = friendlyModifier;
        this.enemyModifier = enemyModifier;
    }

    public override float Desire(Character actor, Tile location, string target)
    {
        float currentDesire = base.Desire(actor, location, target);

        Character targetCharacter = FindInRoom(target, location);

        if (targetCharacter == null || targetCharacter == actor) return 0; //Action is not possible

        if (Relation.AreFamily(actor, targetCharacter)) currentDesire = ModifyDesire(currentDesire, familyModifier);
        if (Relation.AreFriendly(actor, targetCharacter)) currentDesire = ModifyDesire(currentDesire, friendlyModifier);
        if (Relation.AreEnemy(actor, targetCharacter)) currentDesire = ModifyDesire(currentDesire, enemyModifier);

        foreach (ConditionModifier cm in traitModifiers) {
            if (cm.EvaluateWithCharacter(actor, targetCharacter)) currentDesire = ModifyDesire(currentDesire, cm.modifier);
        }

        return currentDesire;

    }

    public override void FormMemory(Character actor, Tile location, string target, List<ActionTargetDesire> potentialConsiderations)
    {
        actor.AddMemory(name + " with " + target, potentialConsiderations);
    }

}