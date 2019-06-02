using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMovement : Action {

    public ActionMovement(string name, float baseDesire = .15f, List<ConditionModifier> traitModifiers = null) : 
        base(name, baseDesire, traitModifiers)
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
        List<Tile> toVisitQueue = new List<Tile> { startLocation };
        HashSet<int> visited = new HashSet<int> { startLocation.tileID };

        Dictionary<Tile, Parent> parentLevelInfo = new Dictionary<Tile, Parent>();
        parentLevelInfo.Add(startLocation, new Parent(null, 1));

        while (toVisitQueue.Count > 0) {
            Tile nextLocation = toVisitQueue[0];
            toVisitQueue.RemoveAt(0);

            Parent parInfo = parentLevelInfo[nextLocation];

            float addedDesire = 0;

            foreach (ConditionModifier cm in traitModifiers) {
                if (cm.EvaluateWithLocation(actor, nextLocation)) addedDesire = ModifyDesire(addedDesire, cm.modifier);
            }

            addedDesire += actor.PickBestActionAt(nextLocation, false).desire;

            currentDesire += (addedDesire / (Mathf.Pow(parInfo.level, 2) * parInfo.siblings));

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