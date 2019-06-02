using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Town : MonoBehaviour
{

    //public int numberOfNpcs = 10;
    //public List<Charecter> charecters;

    Character test;
    List<Character> allCharecters = new List<Character>();
    List<Character> staticNPCs;
    List<Character> partyMembers;
    int sizeOfTiles= 10;
    public Map map;     //public List<Character> residents;
    public List<Family> famalies; 
    void Start()
    {
        TownGenerator townGenerator = new TownGenerator();
        famalies = townGenerator.GenerateFamalies(5, 2);//(100,25);//off by 1 or -1 
        townGenerator.AddRandomRelations(famalies);
        townGenerator.AssignProfessions(famalies);

        allCharecters = combineAllCharecters(famalies);

        map = new Map(15);
        //map.PopullateAllTiles(allCharecters);

        //Debugging Section:
        partyMembers = new List<Character>(allCharecters);
        map.InstantiatePremadeMap();
        map.QuickFillPopulation(partyMembers);
       

         //for debugging only -- works 
        //foreach (Tile t in map.tiles){
        //    Debug.Log("HI this map has room: "+t.roomType
        //    +" the ID is : "+ t.tileID + " : ");
        //    foreach(Character c in t.populatedNPCs)
        //    {
        //        Debug.Log(t.tileID+"# has npcs:"+c.name);
        //        //foreach(Tile x in t.connectedTiles)
        //        //{
        //        //    Debug.Log(x.tileID + "# is conntected to " +x.roomType);


        //        //}
        //    }
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //for (int hour = 5; hour < 20; hour++) {
                foreach (Character character in partyMembers) {
                    character.TakeAction();

                }
            //}
        }
    }

    //pre step so we can randomly assign them - better than looping later on, maybe moove family ID to each charecteR? 
    //and save it after creating the families 
    List<Character>  combineAllCharecters(List<Family> _families)
    {
        List<Character> all = new List<Character>();
        foreach (Family f in _families)
        {
            foreach (Character c in f.members)
            {
                all.Add(c);
            }
        }
        return all;
    }

}





//public string returnOneDiscription()
//{
//    return "";
//}
//public void PopulateTile(int tileID, List<Character> characters, List<Character> party)
//{
//    populatedNPCs = characters;
//    populatedPatry = party;
//}

//public void populateNPCTile(List<Character> npc)
//{

//} 


    //todo fix this mess when you can focus :) 
//public List<Tile> populateAllTiles(List<Character> NPCcharacters, List<Character> party)
//{ 
//        //save npccharecters in a list manipulate it and then remove it instead of by ref
//       //tiles is supplied initially via constructor --- or can be sent here and then sent back 
//       point: // and repeat this loop 
//        foreach( Tile tile in tiles)
//        {
//            if ( tile.tileID == 0)// at the first tile then populate all party memebrs here 
//            {
//                tile.populatedPatry = party; //full party 
//            }
//            //might change from call by refrence --- 
//        int x = Random.Range(0, NPCcharacters.Count);
//        tile.populatedNPCs.Add(NPCcharacters[x]);
//        NPCcharacters.Remove(NPCcharacters[x]);

//    }

//    if (NPCcharacters.Count >= 1) // if my npc char list is not empty ( need to populate more then jump back to the bening ) 

//    {
//        goto point;
//    }

//    return tiles;
//}