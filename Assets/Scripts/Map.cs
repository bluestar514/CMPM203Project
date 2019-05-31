using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void InstantiatePremadeMap()
    {
        List<Tile> premadeMap = new List<Tile> {
            new Tile(0, Tile.roomTypes.TownCenter),
            new Tile(1, Tile.roomTypes.Inn),
            new Tile(2, Tile.roomTypes.Library),
            new Tile(3, Tile.roomTypes.forestPath),
            new Tile(4, Tile.roomTypes.forestPath),
            new Tile(5, Tile.roomTypes.forest),
            new Tile(6, Tile.roomTypes.Cave)
        };

        Connect(premadeMap[0], premadeMap[1]);
        Connect(premadeMap[0], premadeMap[2]);
        Connect(premadeMap[0], premadeMap[3]);
        Connect(premadeMap[3], premadeMap[4]);
        Connect(premadeMap[3], premadeMap[5]);
        Connect(premadeMap[4], premadeMap[5]);
        Connect(premadeMap[5], premadeMap[6]);

        tiles = premadeMap;
        size = tiles.Count;
    }

    public List<Tile> GenerateAMap(int size)
    {

        List<Tile> generatedTiles = new List<Tile>();//create an empty one
        for (int i = 0; i < size; i++) {
            if (i == 0) //first tile thenmake sure it is the firsy one 
            {
                Tile firstTile = new Tile(i, true);
                generatedTiles.Add(firstTile);

            } else {
                Tile aTile = new Tile(i);
                generatedTiles.Add(aTile);

            }

        }
        //uncomment this to connect tiles - bad  for now ----  would have to fillde with it to inlcude sizes with just 12 tiles we barly have enupogh npcs / tile types
        // it looks like it populates npcs in the not connected ones 

        ConnectTiles(generatedTiles);
        return generatedTiles; //holds all types and tile map
    }
    //this is reallyt baddddly done --
    private void ConnectTiles(List<Tile> generatedTiles)
    {
        List<Tile> temporaryTileList = new List<Tile>( generatedTiles );
        for (int i = 0; i < temporaryTileList.Count; i++) {
            int index = UnityEngine.Random.Range(0, temporaryTileList.Count);

            if (generatedTiles[i].roomType == Tile.roomTypes.UpeerLevel || generatedTiles[i].roomType == Tile.roomTypes.LowerLevel) {
                if (temporaryTileList[i].roomType == Tile.roomTypes.Inn || temporaryTileList[i].roomType == Tile.roomTypes.house) {
                    //generatedTiles[i].connectedTiles.Add(temporaryTileList[index]); // add upper or lower here 
                    Connect(generatedTiles[i], temporaryTileList[index]);
                }
            } else if (generatedTiles[i].roomType == Tile.roomTypes.TownCenter && temporaryTileList[i].roomType == Tile.roomTypes.forestPath) {
                generatedTiles[i].connectedTiles.Add(temporaryTileList[index]); // add upper or lower here 
                Connect(generatedTiles[i], temporaryTileList[index]);
            } else if (generatedTiles[i].roomType != Tile.roomTypes.Inn || generatedTiles[i].roomType != Tile.roomTypes.house || generatedTiles[i].roomType != Tile.roomTypes.TownCenter)
                Connect(generatedTiles[i], temporaryTileList[index]); // add upper or lower here
            else Connect(generatedTiles[i], temporaryTileList[index]);

            temporaryTileList.Remove(temporaryTileList[index]);

        }
    }

    void Connect(Tile a, Tile b)
    {
        a.connectedTiles.Add(b);
        b.connectedTiles.Add(a);
    }
    //public List<Tile> GenerateMap()
    //{
    //    List<Tile> directory = new List<Tile> { new Tile(0, true)};

    //    for(int i = 1; i < size; i++) {

    //    }

    //}

    internal void PopullateAllTiles(List<Character> _cs)
    {

        List<Character> cs = _cs;
point:
        for (int i = 0; i < tiles.Count; i++) {
            int index = UnityEngine.Random.Range(0, cs.Count);

            if (tiles[i].tileID == 0)// at the first tile then populate all party memebrs here 
            {
                //add --- once i know the party tiles[i].populatedPatry = party; //full party add here 

            }
            Character c = cs[index];
            cs.Remove(cs[index]);
            PopulateSpecficTile(i, c);

            if (i == tiles.Count - 1 && cs.Count >= 1) //if we have populated all tiles and we still have npcs then go back and add more npcs 
            {
                goto point;
            }
            if (cs.Count <= 1) {
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

    public void QuickFillPopulation(List<Character> pop)
    {
        foreach(Character character in pop) {
            character.MoveTo(tiles[Random.Range(0, tiles.Count)], true);
        }
    }
}


[System.Serializable]
public class Tile {
    public string name;
    //public List<Tile> subConnections = new List<Tile>();
    public int tileID = 0; // index 
    //string TitleType = "";
    string discription = "";
    public enum roomTypes {
        forest, forestPath, house, Cave, TownCenter,
        Watchtower, Library, LowerLevel, UpeerLevel, Inn
    }
    public roomTypes roomType;

    public List<string> discriptionsPerRoom = new List<string>();
    public List<Character> populatedNPCs = new List<Character>();
    public List<Character> populatedPatry = new List<Character>();
    public List<Tile> connectedTiles = new List<Tile>();

    public List<Action> availableActions = new List<Action>();

    //maybe better make a dictionary of roomtype and string (discription) 
    public Tile(int _tileId)
    {
        
        setroomInformation();
        name = _tileId + "(" + roomType.ToString() + ")";
        this.tileID = _tileId;
        AddSocialActions();
        AddMovementActions();
    }
    public Tile(int _tileId, bool flag)
    {
        this.tileID = _tileId;
        
        MakeTownCenter();
        name = _tileId + "(" + roomType.ToString() + ")";
        AddSocialActions();
        AddMovementActions();
    }

    public Tile(int _tileId, roomTypes roomType)
    {
        this.tileID = _tileId;
        MakeRoomOfType(roomType);
        name = _tileId + "(" + roomType.ToString() + ")";
        AddSocialActions();
        AddMovementActions();
    }


    public void setroomInformation()
    {
        {//i feel like we used way too many random.ranges and types defined this way, maybe change this tructure? 
            //summer tile will be fun :) 
            int x = UnityEngine.Random.Range(0, 10);
            switch (x) {
                case 0:
                    MakeHouse();
                    break;
                case 1:
                    MakeCave();
                    break;

                case 2:
                    MakeWatchTower();
                    break;
                case 3:
                    MakeLibrary();
                    break;
                case 4:
                    MakeLowerLevel();
                    break;
                case 5:
                    MakeUpperLevel();
                    break;
                case 6:
                    MakeInn();
                    break;
                case 7:
                    MakeForest();
                    break;
                default:
                    MakeForestPath();
                    break;
            }
        }
    }

    public void MakeRoomOfType(roomTypes roomType)
    {
        switch (roomType) {
            case roomTypes.TownCenter:
                MakeTownCenter();
                break;
            case roomTypes.house:
                MakeHouse();
                break;
            case roomTypes.Cave:
                MakeCave();
                break;

            case roomTypes.Watchtower:
                MakeWatchTower();
                break;
            case roomTypes.Library:
                MakeLibrary();
                break;
            case roomTypes.LowerLevel:
                MakeLowerLevel();
                break;
            case roomTypes.UpeerLevel:
                MakeUpperLevel();
                break;
            case roomTypes.Inn:
                MakeInn();
                break;
            case roomTypes.forest:
                MakeForest();
                break;
            default:
                MakeForestPath();
                break;
        }
        
    }

    private void MakeTownCenter()
    {
        this.roomType = roomTypes.TownCenter;
        this.discriptionsPerRoom = new List<string>{
                    " at the town house",
                     " The group is at the town center",
                     "What a great town"   };
    }

    private void MakeHouse()
    {
        this.roomType = roomTypes.house;
        this.discriptionsPerRoom = new List<string>{
                    " at the town house",
                     " The group is at the town center",
                     "What a great town"   };
    }

    private void MakeCave()
    {
        this.roomType = roomTypes.Cave;
        this.discriptionsPerRoom = new List<string>{
                    " a gloomy cave",
                     " dark cave"
                     };
    }

    private void MakeWatchTower()
    {
        this.roomType = roomTypes.Watchtower;
        this.discriptionsPerRoom = new List<string>{
                    " so high in the sky",
                     " The group is at the town center",
                     "What a great town"   };
    }

    private void MakeLibrary()
    {
        this.roomType = roomTypes.Library;//or maybe char qoutes?
        this.discriptionsPerRoom = new List<string>{
                    "so many books",
                     "book, books and more books in this room"
                };
    }

    private void MakeLowerLevel()
    {
        this.roomType = roomTypes.LowerLevel;
        this.discriptionsPerRoom = new List<string>{
                    " down",
                     "so humid " };
    }

    private void MakeUpperLevel()
    {
        this.roomType = roomTypes.UpeerLevel;
        this.discriptionsPerRoom = new List<string>{
                    " the view from up here looks ... ummm"

                };
    }

    private void MakeInn()
    {
        this.roomType = roomTypes.Inn;
        this.discriptionsPerRoom = new List<string>{
                    " so crowded",
                     " The group is at the local Inn",
                     "What a great INN"

                };
    }

    private void MakeForest()
    {
        this.roomType = roomTypes.forest;
        this.discriptionsPerRoom = new List<string>{
                    " at the town house",
                     " The group is at the town center",
                     "What a great town" };
    }

    private void MakeForestPath()
    {
        this.roomType = roomTypes.forestPath;
        this.discriptionsPerRoom = new List<string>{
                    "so many treeeees",
                     "bugs bugs and more bugs in this path",
                     "get me outta here" };
    }

    private void AddSocialActions()
    {
        availableActions.Add(new ActionSocial("chat", traitModifiers: new List<TraitModifier> { new TraitModifier("kp0", 3), new TraitModifier("rp1", -3) }));
        availableActions.Add(new ActionSocial("hug", familyModifier: 3));
    }
    private void AddMovementActions()
    {
        availableActions.Add(new ActionMovement("move"));
    }

    public void AddCharacterToTile(Character character, bool isParty)
    {
        if (isParty) {
            populatedPatry.Add(character);
        } else {
            populatedNPCs.Add(character); 
        }
    }

    public void RemoveCharacterFromTile(Character character, bool isParty)
    {
        if (isParty) {
            populatedPatry.Remove(character);
        } else {
            populatedNPCs.Remove(character);
        }
    }

    public List<Character> GetAllVisitors()
    {
        List<Character> visitors = new List<Character>(populatedNPCs);
        visitors.AddRange(populatedPatry);
        return visitors;
    }
}//end of class 
