using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;


public class MapGenerator : MonoBehaviour {
    public int[,] map = new int[16, 16]; //2d array describing places of walls and open spaces 
    // Use this for initialization
    public void Start () {
        bool enabled = true; //enabling the final fix to remove loops, for testing

        int[,] deniedspaces = new int[16, 16]; //2d array descirbing all the spaces that cannot be counted (for path checking) as open but are
                                               // j = verital
                                               // i = horizontal

        //assign walls
        System.Random random = new System.Random();
        //full
        // open = 2, unnasigned = 0, block = 1
        for (int j = 0; j <= 15; j++)
            {
                for (int i = 0; i <= 15; i++)
                {
                    // % chance of block spawning

                    int r = 2;
                    int p = random.Next(1, 11);
                    if (p > 5) { r = 1; } //using this it allows me to change the probability of the randomness
                    else if (p < 6) { r = 2; }
                    //do we have any surrounding open* blocks? (above,sides) *(not unassigned, OPEN)
                    int[] openspaces = new int[3] { 0, 0, 0 }; //for recording which spaces are open
                    bool open = false; //assumed to be no
                    //check space to sides and above, but not below as always unassigned
                    try { if (map[j - 1, i] == 2) { open = true; openspaces[0] = 1; } } catch (IndexOutOfRangeException) { }  //above
                    try { if (map[j, i + 1] == 2) { open = true; openspaces[1] = 1; } } catch (IndexOutOfRangeException) { }  //right
                    try { if (map[j, i - 1] == 2) { open = true; openspaces[2] = 1; } } catch (IndexOutOfRangeException) { }  //left

                    if (open == false)
                    {
                        map[j, i] = r; //  50/50 chance as completely singular square
                    }
                    else if (open == true)
                    {   // if the spaces are open, do they have any other open or undecided blocks near them (under, sides)
                        int[] otheropen = new int[3] { 0, 0, 0 }; //for recording which nearby open block have other nearby open spaces
                        if (openspaces[0] == 1) //above
                        {
                            //check space to sides, but not above as not relavent, and below would be checking itself, and none are invalid
                            try { if ((map[j - 1, i + 1] == 0 || map[j - 1, i + 1] == 2) && deniedspaces[j - 1, i + 1] != 1) { otheropen[0] = 1; } } catch (IndexOutOfRangeException) { }
                            try { if ((map[j - 1, i - 1] == 0 || map[j - 1, i - 1] == 2) && deniedspaces[j - 1, i - 1] != 1) { otheropen[0] = 1; } } catch (IndexOutOfRangeException) { }
                        }
                        else if (openspaces[0] == 0) { otheropen[0] = 2; } // if the block is not open it is ignored

                        if (openspaces[1] == 1) //right
                        {
                            //check space to right and below, but not above as not relavent, and left would be checking itself, and none are invalid
                            try { if ((map[j + 1, i + 1] == 0 || map[j + 1, i + 1] == 2) && deniedspaces[j + 1, i + 1] != 1) { otheropen[1] = 1; } } catch (IndexOutOfRangeException) { }
                            try { if ((map[j, i + 2] == 0 || map[j, i + 2] == 2) && deniedspaces[j, i + 2] != 1) { otheropen[1] = 1; } } catch (IndexOutOfRangeException) { }
                        }
                        else if (openspaces[1] == 0) { otheropen[1] = 2; } // if the block is not open it is ignored

                        if (openspaces[2] == 1) //left
                        {
                            //check space to left and below, but not above as not relavent, and right would be checking itself, and none are invalid
                            try { if ((map[j + 1, i - 1] == 0 || map[j + 1, i - 1] == 2) && deniedspaces[j + 1, i - 1] != 1) { otheropen[2] = 1; } } catch (IndexOutOfRangeException) { }
                            try { if ((map[j, i - 2] == 0 || map[j, i - 2] == 2) && deniedspaces[j, i - 2] != 1) { otheropen[2] = 1; } } catch (IndexOutOfRangeException) { }
                        }
                        else if (openspaces[2] == 0) { otheropen[2] = 2; } // if the block is not open it is ignored


                        if (otheropen[0] == 0 || otheropen[1] == 0 || otheropen[2] == 0) { map[j, i] = 2; } //sets self as open, as a nearby open block has no other open blocks
                        else if (otheropen[0] != 0 && otheropen[1] != 0 && otheropen[2] != 0)
                        //yes all the near open block have a possible other route
                        {
                            map[j, i] = r; // 50/50 as this block will not create a dead end
                            if (r == 1)//if original block closed then nearby open blocks will become invalid for next check, i.e. considered closed
                            {
                                try { if (otheropen[0] == 1) { deniedspaces[j - 1, i] = 1; } } catch (IndexOutOfRangeException) { }//top
                                try { if (otheropen[1] == 1) { deniedspaces[j, i + 1] = 1; } } catch (IndexOutOfRangeException) { }//right
                                try { if (otheropen[2] == 1) { deniedspaces[j, i - 1] = 1; } } catch (IndexOutOfRangeException) { }//left
                            }
                        }
                    }

                }
            }






            //AFTER FIX VERTIAL DIVIDE
            int error = 0;
            int NB = 0; //Number of blocks along the 15th (final/bottom) row.
            int[] locationB = new int[16]; //Array to store the locations of the blocks on the bottom row
            bool blockfound = false; //When searching for a block denotes one has not been found yet
            int blocklocation = 0; //Determines where the block was found last.
            int[] Coords = new int[2]; // Coordinates of next block relative to where it was found
            bool endfound = false; //states when the end of a path has been found
            List<int> CoordRecord0 = new List<int>(); //List of coordinates in y, for use when a route with no end has been found
            List<int> CoordRecord1 = new List<int>(); //List of coordinates in x, for use when a route with no end has been found (p.s. I'm sure this would be better as a martix but I'll do it later)

            for (int i = 0; i <= 15; i++)//go along bottom row to find closed blocks
            {
                if (map[15, i] == 1)
                {
                    locationB[NB] = i;
                    NB = NB + 1;
                }
            }
            for (int k = 0; k <= NB - 1; k++)
            {
                endfound = false;
                blockfound = false;
                while (blockfound == false)
                {
                    try { if (map[15, locationB[k] - 1] == 1) { blocklocation = 1; Coords[0] = 15; Coords[1] = locationB[k] - 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //left
                    try { if (map[15 - 1, locationB[k] - 1] == 1) { blocklocation = 2; Coords[0] = 15 - 1; Coords[1] = locationB[k] - 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //above-left
                    try { if (map[15 - 1, locationB[k]] == 1) { blocklocation = 3; Coords[0] = 15 - 1; Coords[1] = locationB[k]; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //above
                    try { if (map[15 - 1, locationB[k] + 1] == 1) { blocklocation = 4; Coords[0] = 15 - 1; Coords[1] = locationB[k] + 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //above-right
                    try { if (map[15, locationB[k] + 1] == 1) { blocklocation = 5; Coords[0] = 15; Coords[1] = locationB[k] + 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //right
                    endfound = true;
                    blockfound = true; //when no blocks are fine that route is good and so can be moved along from
                }
                CoordRecord0.Clear();
                CoordRecord1.Clear();
                while (Coords[0] > 0 && endfound == false) //until the top is reached or end is found
                {
                    blockfound = false;
                    while (blockfound == false)
                    {
                        CoordRecord0.Add(Coords[0]);
                        CoordRecord1.Add(Coords[1]);
                        if (blocklocation != 5 && blocklocation != 6)
                        {
                            try { if (map[Coords[0], Coords[1] - 1] == 1) { blocklocation = 1; Coords[0] = Coords[0]; Coords[1] = Coords[1] - 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //left
                        }
                        if (blocklocation != 6)
                        {
                            try { if (map[Coords[0] - 1, Coords[1] - 1] == 1) { blocklocation = 2; Coords[0] = Coords[0] - 1; Coords[1] = Coords[1] - 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //above-left
                        }
                        try { if (map[Coords[0] - 1, Coords[1]] == 1) { blocklocation = 3; Coords[0] = Coords[0] - 1; Coords[1] = Coords[1]; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //above
                        try { if (map[Coords[0] - 1, Coords[1] + 1] == 1) { blocklocation = 4; Coords[0] = Coords[0] - 1; Coords[1] = Coords[1] + 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //above-right
                        try { if (map[Coords[0], Coords[1] + 1] == 1) { blocklocation = 5; Coords[0] = Coords[0]; Coords[1] = Coords[1] + 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //right
                        try { if (map[Coords[0] + 1, Coords[1] + 1] == 1) { blocklocation = 6; Coords[0] = Coords[0]; Coords[1] = Coords[1] + 1; blockfound = true; break; } } catch (IndexOutOfRangeException) { }  //right low
                        endfound = true;  //when it has passed through all the spaces and none are block the end is found
                        break;

                    }
                }
                if (endfound == false)
                {



                    int r = random.Next(CoordRecord0.Count);

                    if (map[CoordRecord0[r] + 1, CoordRecord1[r]] != 1 && map[CoordRecord0[r] - 1, CoordRecord1[r]] != 1 && map[CoordRecord0[r], CoordRecord1[r] + 1] != 1 && map[CoordRecord0[r], CoordRecord1[r] - 1] != 1)
                    {
                        try { map[CoordRecord0[r], CoordRecord1[r]] = 2; } catch (IndexOutOfRangeException) { k = k + 1; }
                    }
                    else if (error > 20) { error = 0; try { map[CoordRecord0[r], CoordRecord1[r]] = 2; } catch (IndexOutOfRangeException) { k = k + 1; } }
                    else { error = error + 1; }


                    k = k - 1;


                }

                // if the top is reached and no traverable path is found set a random coordinate from the path as open then test again (k = k - 1)
                // if the endfound = true then the path will simply be resolved
            }

            if (enabled == true)
            {
                //AFTER FIX LOOPS
                endfound = false;
                int NOB = 0; //Number of blocks eith open blocks after along the 15th (final/bottom) row.
                int[] locationOB = new int[16]; //Array to store the locations of the open blocks on the bottom row
                List<string> CoordRecordFull = new List<string>(); //List of coordinates in form 'y,x' 
                List<int> DERecord0 = new List<int>(); //List of dead-end coordinates in y, for use when a route with no end has been found
                List<int> DERecord1 = new List<int>(); //List of dead-end coordinates in x, for use when a route with no end has been found (p.s. I'm sure this would be better as a martix but I'll do it later)
                bool deadendchanged = false; //Boolean to check if a block next to a dead end was changed. 
                bool moved = false; //defines if there has been a change since the original position for determing when the first value is a dead end
                int t = 0; //recording when the process is repeated so knowing when to clear the list of dead ends.


                if (map[15, 0] == 2) //if the first block is open add to list of open blocks to be tested, if not then the next open block will be caught by next loop, as it means the block is closed
                {
                    locationOB[NOB] = 0;
                    NOB = NOB + 1;
                }

                for (int i = 0; i <= 15; i++)//go along bottom row to find closed blocks and record open blocks after
                {
                    if (map[15, i] == 1)
                    {
                        try
                        {
                            if (map[15, i + 1] == 2)
                            {
                                locationOB[NOB] = i + 1;
                                NOB = NOB + 1;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }

                for (int j = 0; j <= NOB - 1; j++)
                {
                    endfound = false;
                    moved = false;
                    //list of dead ends, only needs to be cleared is a new value of j
                    if (j > t)
                    {
                        DERecord0.Clear();
                        DERecord1.Clear();
                    }
                    t = j;

                    Coords[0] = 15;
                    Coords[1] = locationOB[j];

                    CoordRecord0.Clear();
                    CoordRecord1.Clear();
                    CoordRecordFull.Clear();
                    while (endfound == false && Coords[0] > 0) //end is reached, end is not found
                    {
                        while (endfound == false && Coords[0] > 0) //end is reached, end is not found 
                        {
                            CoordRecord0.Add(Coords[0]);
                            CoordRecord1.Add(Coords[1]);
                            CoordRecordFull.Add(Coords[0].ToString() + "," + Coords[1].ToString());
                            try //above
                            {
                                if (
                                   (map[Coords[0] - 1, Coords[1]] == 2) && //check if the node is open
                                   (CoordRecordFull.Any(item => item == ((Coords[0] - 1).ToString() + "," + (Coords[1]).ToString()))) == false) //check the node hasnt been passed already
                                { Coords[0] = Coords[0] - 1; Coords[1] = Coords[1]; moved = true; break;
                                }
                            }
                            catch (IndexOutOfRangeException) { }  //above
                            try //right
                            {
                                if (
                                   (map[Coords[0], Coords[1] + 1] == 2) && //check if the node is open
                                   (CoordRecordFull.Any(item => item == ((Coords[0]).ToString() + "," + (Coords[1] + 1).ToString()))) == false) //check the node hasnt been passed already
                                { Coords[0] = Coords[0]; Coords[1] = Coords[1] + 1; moved = true; break;
                                }
                            }
                            catch (IndexOutOfRangeException) { }  //right
                            try //left
                            {
                                if (
                                   (map[Coords[0], Coords[1] - 1] == 2) && //check if the node is open
                                   (CoordRecordFull.Any(item => item == ((Coords[0]).ToString() + "," + (Coords[1] - 1).ToString()))) == false) //check the node hasnt been passed already
                                { Coords[0] = Coords[0]; Coords[1] = Coords[1] - 1; moved = true; break;
                                }
                            }
                            catch (IndexOutOfRangeException) { }  //left
                            try //down
                            {
                                if (
                                   (map[Coords[0] + 1, Coords[1]] == 2) && //check if the node is open
                                   (CoordRecordFull.Any(item => item == ((Coords[0] + 1).ToString() + "," + (Coords[1]).ToString()))) == false) //check the node hasnt been passed already
                                { Coords[0] = Coords[0] + 1; Coords[1] = Coords[1]; moved = true; break;
                                }
                            }
                            catch (IndexOutOfRangeException) { }  //down
                            if (Coords[0] != 0)
                            {
                                endfound = true; //defines that a deadend has been found so will restart 'while'
                                j = j - 1;
                                try { map[Coords[0], Coords[1]] = 7; DERecord0.Add(Coords[0]); DERecord1.Add(Coords[1]); } catch (ArgumentOutOfRangeException) { } //add location of dead end to deadendlocationlist
                            }
                            break;
                        }
                    }

                    //check if either (one of the stsrting blocks i.e. value of OBlocaction[nob] |and| if it's one of the starting blocks, has it moved?

                    if (Coords[0] == 15 && locationOB.Contains(Coords[1]) == true && moved == false) //process for changing one random block near a dead end to open
                    {
                        deadendchanged = false;
                        while (deadendchanged == false)
                        {
                            int r = random.Next(DERecord0.Count);
                            int f = random.Next(1, 4);
                            if (f == 1)
                            {
                                try { map[DERecord0[r] - 1, DERecord1[r]] = 2; deadendchanged = true; } catch (IndexOutOfRangeException) { } //above
                            }
                            else if (f == 2)
                            {
                                try { map[DERecord0[r], DERecord1[r] - 1] = 2; deadendchanged = true; } catch (IndexOutOfRangeException) { } //right
                            }
                            else if (f == 3)
                            {
                                try { map[DERecord0[r], DERecord1[r] + 1] = 2; deadendchanged = true; } catch (IndexOutOfRangeException) { } //left
                            }
                            else if (f == 4)
                            {
                                try { map[DERecord0[r] + 1, DERecord1[r]] = 2; deadendchanged = true; } catch (IndexOutOfRangeException) { } //below
                            }
                        }
                    }

                }

            }


            //DISPLAY FUNCTION


            //testing changes
            //map[3,15] = 5;
            //testing changes

            for (int j = 0; j <= 15; j++)
        {
            Console.WriteLine();
            for (int i = 0; i <= 15; i++)
            {
                //Console.Write(map[j,i]);
                //Console.Write(", ");
            }
        }

        //Console.WriteLine("");
        //Console.WriteLine("");
        //Console.WriteLine("");
        //Console.WriteLine("");
        //Console.WriteLine("");

        for (int j = 0; j <= 15; j++)
        {
            Console.WriteLine();
            for (int i = 0; i <= 15; i++)
            {
                //if (map[j, i] == 7) { Debug.Log("░░"); }
                //if (map[j, i] == 1) { Debug.Log("██"); }
                //if (map[j, i] == 2) { Debug.Log("░░"); }

                //testing changes
                //if (map[j, i] == 5) { Console.Write("aa"); }
                //testing changes
            }
        }
        //Console.WriteLine("");
        //Console.WriteLine("");
        //Console.WriteLine("");
        //Console.WriteLine("");
        //Console.WriteLine("");

        for (int j = 0; j <= 15; j++)
        {
            Console.WriteLine();
            for (int i = 0; i <= 15; i++)
            {
                //if (deniedspaces[j,i] == 1) { Console.Write("██"); }
                //if (deniedspaces[j,i] == 0) { Console.Write("░░"); }

            }
        }
        
        Console.ReadLine();
        
    }
}
