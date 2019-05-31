using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Memory 
{
    public string mainEntry;
    public List<ActionTargetDesire> potentialConsiderations;

    public Memory(string mainEntry, List<ActionTargetDesire> potentialConsiderations)
    {
        this.mainEntry = mainEntry;
        this.potentialConsiderations = potentialConsiderations;
    }

    public Memory(string mainEntry)
    {
        this.mainEntry = mainEntry;
        potentialConsiderations = new List<ActionTargetDesire>();
    }
}
