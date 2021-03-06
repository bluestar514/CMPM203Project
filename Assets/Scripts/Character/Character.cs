﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//acts as a collector for all data types related to a charcter 
[System.Serializable]
public class Character
{
    public string name;
    public PhysicalCharactristics pc;
    public List<Relation> relations;
    public Profession profession;
    public List<Memory> memoryLog;
    public List<Trait> traitList;
    public List<Action> uniqueActions;

    public Tile currentLocation;

    public Character()
    {
        relations = new List<Relation>();
        pc = new PhysicalCharactristics();
        profession = null;
        currentLocation = null;
        memoryLog = new List<Memory>();
        traitList = new List<Trait>();
        uniqueActions = new List<Action>();
    }

    

    public void AddRelation(Character otherCharacter, string relationship)
    {
        relations.Add(new Relation(otherCharacter, relationship, relationship=="mother"|| relationship=="father"?true:false));
    }
    public void AddRelation(Character otherCharacter, string relationship, int thisFamId) 
    {
        relations.Add(new Relation(otherCharacter, relationship));
    }

    public void MoveTo(Tile location, bool isParty)
    {
        if(currentLocation != null) currentLocation.RemoveCharacterFromTile(this, isParty);
        location.AddCharacterToTile(this, isParty);
        currentLocation = location;
    }

    public void printAcharecter() //this will return it later instead of print it or write it to json 
    {
        
        PhysicalCharactristics p = new PhysicalCharactristics();
        Debug.Log(name + "\n has eye: " + p.eyeColor + "" +
            "\n has hair color: " + p.hairColor +
            "\n height:  " + p.height +
            "\n and weight" + p.weight +
            "\n does this charcter have a scar?" + p.hasMark);
        //will return it here and link it 
        // need to store this somehow --- and then check if anyone in the list has the sam ehair color? refine relationship 

    }

    public Character FindRelation(string relationship)
    {
        foreach(Relation relation in relations) {
            if(relation.relationship == relationship) {
                return relation.otherCharacter;
            }
        }

        return null;
    }

    public Relation FindRelationBetween(Character otherCharacter)
    {
        foreach (Relation relation in relations) {
            if (relation.otherCharacter == otherCharacter) {
                return relation;
            }
        }

        return null;
    }

    public void AddMemory(string mainEntry, List<ActionTargetDesire> potentialConsiderations)
    {
        memoryLog.Add(new Memory(mainEntry, potentialConsiderations));
    }

    List<ActionTargetDesire> GetFullListOfActions(Tile location, bool mayMove = true)
    {
        List<Action> locationAndUniqueActions = new List<Action>();
        locationAndUniqueActions.AddRange(location.availableActions);
        locationAndUniqueActions.AddRange(uniqueActions);

        List<ActionTargetDesire> potentialActions = new List<ActionTargetDesire>();
        foreach (Action action in locationAndUniqueActions) {
            string target = "";
            float desire = action.Desire(this, location, target) * Random.Range(.85f, 1.15f);

            ActionSocial actionSocial = action as ActionSocial;
            if (actionSocial != null) {
                foreach (Character character in location.GetAllVisitors()) {
                    target = character.name;
                    desire = actionSocial.Desire(this, location, target) * Random.Range(.85f, 1.15f); //add a little variation to base desire
                    potentialActions.Add(new ActionTargetDesire(actionSocial, target, desire));
                }
                continue;
            }
            ActionMovement actionMovement = action as ActionMovement;
            if (mayMove && actionMovement != null) {
                foreach (Tile loc in location.connectedTiles) {
                    target = loc.tileID.ToString();
                    desire = actionMovement.Desire(this, location, target) * Random.Range(.85f, 1.15f);

                    potentialActions.Add(new ActionTargetDesire(actionMovement, loc.tileID.ToString(), desire));
                }
                continue;
            }

            potentialActions.Add(new ActionTargetDesire(action, "", desire));
        }

        return potentialActions;
    }

    

    public ActionTargetDesire PickBestActionAt(Tile location, bool mayMove = true)
    {
        List<ActionTargetDesire> potentialActions = GetFullListOfActions(location, mayMove);

        return PickBestActionFrom(potentialActions);

    }

    public ActionTargetDesire PickBestActionFrom(List<ActionTargetDesire> potentialActions)
    {
        ActionTargetDesire bestAction = new ActionTargetDesire(new ActionMovement("loitter", 0), currentLocation.tileID.ToString(), 0); //must pick an action that they can do (in other words, is greater than 0)
        foreach (ActionTargetDesire choice in potentialActions) {
            if (bestAction.desire < choice.desire) bestAction = choice;
        }

        return bestAction;

    }

    public void TakeActionAt(Tile location)
    {
        List<ActionTargetDesire> allActions = GetFullListOfActions(location);

        ActionTargetDesire bestAction = PickBestActionFrom(allActions);

        bestAction.action.Enact(this, location, bestAction.target, allActions);

    }   

    public void TakeAction()
    {
        TakeActionAt(currentLocation);
    }

    
}

[System.Serializable]
public class ActionTargetDesire {
    public string name="null Action";
    public Action action;
    public string target;
    public float desire;

    public ActionTargetDesire(Action action, string target, float desire)
    {
        if(action != null) name = action.name + " "+ target + ": "+desire;
        this.action = action;
        this.target = target;
        this.desire = desire;
    }
}

