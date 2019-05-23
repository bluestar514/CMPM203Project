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
    int sizeOfTiles= 10;
    public Map map;     //public List<Character> residents;
    public List<Family> famalies; 
     void Start()
    {
        TownGenerator townGenerator = new TownGenerator();
        famalies = townGenerator.GenerateFamalies(100,25);//off by 1 or -1 
        townGenerator.AddRandomRelations(famalies);
        townGenerator.AssignProfessions(famalies);
        //residents = townGenerator.GenerateResidents(10, 2);//copying this method for family structure for now commenting here 
        string json = JsonUtility.ToJson(famalies[0]);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
        allCharecters = combineAllCharecters(famalies);

        map = new Map(15);
        map.PopullateAllTiles(allCharecters);
       

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

//in progress ---- 
[System.Serializable]
public class Map //kinda like a wrapper 
{
    public List<Tile> tiles = new List<Tile>();
    int size = 0;


    public Map(int _size) 
    {
        this.size = _size;
        this.tiles = GenerateAMap(_size); //this was its own method - instead of looping twice doing evrything in one go?
    }

    public List<Tile> GenerateAMap(int size)
    {
    
        List<Tile> generatedTiles = new List<Tile>();//create an empty one
        for(int i = 0; i< size; i++)
        {
            if (i == 0) //first tile thenmake sure it is the firsy one 
            {
                Tile firstTile = new Tile(i, true);
                generatedTiles.Add(firstTile);

            }
            else
            {
            Tile aTile = new Tile(i);
            generatedTiles.Add(aTile);

            }

        }
        //uncomment this to connect tiles - bad  for now ----  would have to fillde with it to inlcude sizes with just 12 tiles we barly have enupogh npcs / tile types
       // it looks like it populates npcs in the not connected ones 

         //ConnectTiles(generatedTiles);
        return generatedTiles; //holds all types and tile map
    }
    //this is reallyt baddddly done --
    private void ConnectTiles(List<Tile> generatedTiles)
    {
        List<Tile> temporaryTileList = generatedTiles;
        for (int i = 0; i < temporaryTileList.Count; i++)
        {
            int index = UnityEngine.Random.Range(0, temporaryTileList.Count);
            
            generatedTiles[i].connectedTiles.Add(temporaryTileList[index]);

             if (generatedTiles[i].roomType == Tile.roomTypes.UpeerLevel || generatedTiles[i].roomType == Tile.roomTypes.LowerLevel)

                {
                if (temporaryTileList[i].roomType == Tile.roomTypes.Inn || temporaryTileList[i].roomType == Tile.roomTypes.house)
                          {
                    generatedTiles[i].connectedTiles.Add(temporaryTileList[index]); // add upper or lower here 

                        }
                    }

            if (generatedTiles[i].roomType == Tile.roomTypes.TownCenter && temporaryTileList[i].roomType == Tile.roomTypes.forestPath)
            {
                generatedTiles[i].connectedTiles.Add(temporaryTileList[index]); // add upper or lower here 

            }
            if (generatedTiles[i].roomType!= Tile.roomTypes.Inn || generatedTiles[i].roomType != Tile.roomTypes.house || generatedTiles[i].roomType != Tile.roomTypes.TownCenter)
                generatedTiles[i].connectedTiles.Add(temporaryTileList[index]); // add upper or lower here 

            temporaryTileList.Remove(temporaryTileList[index]);

        }


    }

    internal void PopullateAllTiles(List<Character> _cs)
    {

        List<Character> cs = _cs;
    point:
        for (int i =0; i < tiles.Count; i++)
        {
            int index = UnityEngine.Random.Range(0, cs.Count);

            if (tiles[i].tileID == 0)// at the first tile then populate all party memebrs here 
            {
                //add --- once i know the party tiles[i].populatedPatry = party; //full party add here 
               
            }
            Character c = cs[index];
            cs.Remove(cs[index]);
            PopulateSpecficTile(i, c);

            if(i==tiles.Count -1 && cs.Count >= 1) //if we have populated all tiles and we still have npcs then go back and add more npcs 
            {
                goto point;
            }
            if (cs.Count <= 1)
            {
                //if we run our of charecters - exit the loop 
                break;
            }
        }
    }

    //if party memeber just send in party memeber on this method -  party members in a tile 
    public void PopulateSpecficTile(int tileID, Character character)
    {
        tiles[tileID].populatedNPCs.Add(character);
    }
     
}
[System.Serializable]

public class Tile
{

    //public List<Tile> subConnections = new List<Tile>();
    public int tileID = 0; // index 
    //string TitleType = "";
    string discription = "";
    public enum roomTypes  {
        forest, forestPath, house, Cave, TownCenter,
         Watchtower, Library, LowerLevel, UpeerLevel, Inn 
}
    public roomTypes roomType;

    public List<string> discriptionsPerRoom = new List<string>();
    public List<Character> populatedNPCs = new List<Character>();
    public List<Character> populatedPatry = new List<Character>();
    public List<Tile> connectedTiles = new List<Tile>();
    //maybe better make a dictionary of roomtype and string (discription) 
    public Tile(int _tileId)
    {
        setroomInformation();
        this.tileID = _tileId;
         }
    public Tile(int _tileId,bool flag)
    {
        this.tileID = _tileId;
        this.roomType = roomTypes.TownCenter;
        this.discriptionsPerRoom = new List<string>{
                    " at the town house",
                     " The group is at the town center",
                     "What a great town"   };
     
    }
    //moved this to map 
    //List<Tile> returnConnectingTiles(List<Tile> AllTile) { 
    //{


    //}

    //public roomTypes setTiletype()
    public void setroomInformation() {
        {//i feel like we used way too many random.ranges and types defined this way, maybe change this tructure? 
            //summer tile will be fun :) 
            int x =UnityEngine.Random.Range(0, 10);
            switch (x)
            {
                case 0:
                    this.roomType = roomTypes.house;
                    this.discriptionsPerRoom = new List<string>{
                    " at the town house",
                     " The group is at the town center",
                     "What a great town"   };
                    break;
                case 1:
                    this.roomType = roomTypes.Cave;
                    this.discriptionsPerRoom = new List<string>{
                    " a gloomy cave",
                     " dark cave" 
                     };
                    break;

                case 2:
                    this.roomType = roomTypes.Watchtower;
                    this.discriptionsPerRoom = new List<string>{
                    " so high in the sky",
                     " The group is at the town center",
                     "What a great town"   };
                    break;
                case 3:
                    this.roomType = roomTypes.Library;//or maybe char qoutes?
                    this.discriptionsPerRoom = new List<string>{
                    "so many books",
                     "book, books and more books in this room"
                };
                    break;
                case 4:
                    this.roomType = roomTypes.LowerLevel;
                    this.discriptionsPerRoom = new List<string>{
                    " down",
                     "so humid " };
                    break;
                case 5:
                    this.roomType = roomTypes.UpeerLevel;
                    this.discriptionsPerRoom = new List<string>{
                    " the view from up here looks ... ummm"

                };
                    break;
                case 6:
                    this.roomType = roomTypes.Inn;
                    this.discriptionsPerRoom = new List<string>{
                    " so crowded",
                     " The group is at the local Inn",
                     "What a great INN"

                };
                    break;
                case 7:
                    this.roomType = roomTypes.forest;
                    this.discriptionsPerRoom = new List<string>{
                    " at the town house",
                     " The group is at the town center",
                     "What a great town" };
                    break;
                default:
                    this.roomType = roomTypes.forestPath;
                    this.discriptionsPerRoom = new List<string>{
                    "so many treeeees",
                     "bugs bugs and more bugs in this path",
                     "get me outta here" };
                    break;
            }
        }
    }


}//end of class 



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