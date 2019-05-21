using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Town : MonoBehaviour
{

    //public int numberOfNpcs = 10;
    //public List<Charecter> charecters;

    Character test;
    //public List<Character> residents;
    public List<Family> famalies; 
     void Start()
    {
        TownGenerator townGenerator = new TownGenerator();
        famalies = townGenerator.GenerateFamalies(20, 4);//off by 1 or -1 
        townGenerator.AddRandomRelations(famalies);
        townGenerator.AssignProfessions(famalies);
        //residents = townGenerator.GenerateResidents(10, 2);//copying this method for family structure for now commenting here 
        string json = JsonUtility.ToJson(famalies[0]);
        Debug.Log(json);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

}

//in progress ---- 
public class Map //kinda like a wrapper 
{
    List<Tile> tiles = new List<Tile>();
    public Map() 
    {

    }

    public void generateAMap()
    {

    }

    //for modification on both npc and party members in a tile 
    //todo fix this mess when you can focus :) 
    public void PopulateSpecficTile(int tileID, List<Character> characters, List<Character> party)
    {
        tiles[tileID].populatedNPCs = characters;
        tiles[tileID].populatedPatry = party;
        //add method that populates sublist 
    }
    // for party memebers only 
    public void PopulateSpecficTile(int tileID, List<Character> party)
    {
        tiles[tileID].populatedPatry = party;
    }
    //todo fix this mess when you can focus :) 
    public List<Tile> populateAllTiles(List<Character> NPCcharacters, List<Character> party)
    { 
            //save npccharecters in a list manipulate it and then remove it instead of by ref
           //tiles is supplied initially via constructor --- or can be sent here and then sent back 
           point: // and repeat this loop 
            foreach( Tile tile in tiles)
            {
                if ( tile.tileID == 0)// at the first tile then populate all party memebrs here 
                {
                    tile.populatedPatry = party; //full party 
                }
                //might change from call by refrence --- 
            int x = Random.Range(0, NPCcharacters.Count);
            tile.populatedNPCs.Add(NPCcharacters[x]);
            NPCcharacters.Remove(NPCcharacters[x]);
           
        }

        if (NPCcharacters.Count >= 1) // if my npc char list is not empty ( need to populate more then jump back to the bening ) 

        {
            goto point;
        }

        return tiles;
    }


}

public class Tile
{

    //public List<Tile> subConnections = new List<Tile>();
    public List<Character> populatedNPCs = new List<Character>();
    public List<Character> populatedPatry = new List<Character>();
    public int tileID = 0; // index 
    string TitleType = "";


    public Tile()
    {

    }
    public Tile(List<Tile> connectingTiles,string TitleType)
    {

    }

    public void PopulateTile(int tileID, List<Character> characters, List<Character> party)
    {
        populatedNPCs = characters;
        populatedPatry = party;
    }

    public void populateNPCTile(List<Character> npc)
    {

    }

    //`public Tile returnPopulatedTile()
    //{

    //    Tile x;
    //    return x;
    //}


}
