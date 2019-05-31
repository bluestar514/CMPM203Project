using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action
{
    public string name;
    public float baseDesire;
    public List<TraitModifier> traitModifiers;


    public Action(string name, float baseDesire = .1f, List<TraitModifier> traitModifiers = null)
    {
        this.name = name;
        this.baseDesire = baseDesire;
        if (traitModifiers != null) this.traitModifiers = traitModifiers;
        else this.traitModifiers = new List<TraitModifier>();
    }

    public virtual void Enact(Character actor, Tile location, string target, List<ActionTargetDesire> potentialConsiderations)
    {
        FormMemory(actor, location, target, potentialConsiderations);
    }

    public virtual float Desire(Character actor, Tile location, string target)
    {
        float currentDesire = baseDesire;

        foreach(TraitModifier tm in traitModifiers) {
            if(actor.traitList.Exists( x=> x.id == tm.id )) {
                currentDesire = ModifyDesire(currentDesire, tm.modifier);
            }
        }

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

public class ActionSocial : Action {
    public float familyModifier;
    public float friendlyModifier;
    public float enemyModifier;

    public ActionSocial(string name, float baseDesire=.2f, List<TraitModifier> traitModifiers = null, float familyModifier=1.5f, float friendlyModifier=2f, float enemyModifier=-.75f): base(name, baseDesire, traitModifiers)
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

        return currentDesire;

    }

    public override void FormMemory(Character actor, Tile location, string target, List<ActionTargetDesire> potentialConsiderations)
    {
        actor.AddMemory(name + " with " + target, potentialConsiderations);
    }

}

public class ActionMovement: Action {

    public ActionMovement(string name, float baseDesire=.15f, List<TraitModifier> traitModifiers = null) :base(name, baseDesire, traitModifiers)
    {
        this.name = name;
        this.baseDesire = baseDesire;
    }

    public override void Enact(Character actor, Tile location, string target, List<ActionTargetDesire> potentialConsiderations)
    {
        base.Enact(actor, location, target, potentialConsiderations);

        Tile newLocation = FindConnection(target, location);

        actor.MoveTo(newLocation, true);
    }

    public override float Desire(Character actor, Tile location, string target)
    {
        Tile newLocation = FindConnection(target, location);
        if (newLocation == null) return 0;

        //something that sees how much they might want to do that based on their traits

        float currentDesire = base.Desire(actor, location, target);
        currentDesire = DesireBFS(actor, newLocation, currentDesire);
        
        return currentDesire;
    }

    float DesireBFS(Character actor, Tile startLocation, float currentDesire)
    {
        List<Tile> toVisitQueue = new List<Tile> {startLocation};
        HashSet<int> visited = new HashSet<int> { startLocation.tileID };

        Dictionary<Tile, Parent> parentLevelInfo = new Dictionary<Tile, Parent>();
        parentLevelInfo.Add(startLocation, new Parent(null, 1));

        while(toVisitQueue.Count > 0) {
            Tile nextLocation = toVisitQueue[0];
            toVisitQueue.RemoveAt(0);

            Parent parInfo = parentLevelInfo[nextLocation];

            currentDesire += (actor.PickBestActionAt(nextLocation, false).desire/ (Mathf.Pow(parInfo.level, 2)*parInfo.siblings));

            foreach (Tile neighbor in nextLocation.connectedTiles) {
                if (!visited.Contains(neighbor.tileID)) {
                    toVisitQueue.Add(neighbor);
                    visited.Add(neighbor.tileID);
                    parentLevelInfo.Add(neighbor, new Parent(nextLocation, parInfo.level + 1));
                }
            }
        }

        return currentDesire;
    }
    
    public override void FormMemory(Character actor, Tile location, string target, List<ActionTargetDesire> potentialConsiderations)
    {
        actor.AddMemory("Moved to " + target, potentialConsiderations);
    }



    class Parent {
        public Tile parent;
        public int level;
        public int siblings;

        public Parent(Tile parent, int level)
        {
            this.parent = parent;
            this.level = level;
            if (parent != null) this.siblings = parent.connectedTiles.Count;
            else this.siblings = 1;
        }
    }
}