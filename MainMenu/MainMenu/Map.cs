using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Main
{
    class Map
    {
        Random r = new Random();
        public int startingRoom;
        public static int Wumpus;
        public static int Pit1;
        public static int Pit2;
        public static int sBat1;
        public static int sBat2;
        public static int Player;
        Dictionary<int, int[]> allDoors;
        public int wumpus
        {
            get { return Wumpus; }
            set { Wumpus = value; }
        }
        public int pit1
        {
            get { return Pit1; }
        }
        public int pit2
        {
            get { return Pit2; }
        }
        public int sbat1
        {
            get { return sBat1; }
            set { sBat1 = value; }
        }
        public int sbat2
        {
            get { return sBat2; }
            set { sBat2 = value; }
        }
        public int player
        {
            get { return Player; }
            set { Player = value; }
        }
        Dictionary<int, int[]> doors = new Dictionary<int, int[]>();
        public int[] Rooms = new int[30];

        public Map()
        {
            for (int x = 0; x < 30; x++)
            {
                Rooms[x] = x + 1;
            }
            Random r = new Random();
            int n = Rooms.Length;
            while (n > 1)
            {
                int k = r.Next(0, n);
                n--;
                int temp = Rooms[n];
                Rooms[n] = Rooms[k];
                Rooms[k] = temp;
            }
            setHazards();
            doors = LoadDoors();
        }
        public void Read()
        {
            StreamReader sr = new StreamReader("the path");
            //for you to figure out

        }
        public void setHazards()
        {
            Pit1 = Rooms[0];
            Pit2 = Rooms[1];
            sBat1 = Rooms[2];
            sBat2 = Rooms[3];
            Wumpus = Rooms[4];
            player = Rooms[5];
            startingRoom = Rooms[5];
        }
        public string[] createSecrets()
        {
            string[] secret = new string[6];
            secret[0] = "...There is a bat in room #";
            int whereIsBat = r.Next(2);
            if (whereIsBat == 0)
            {
                secret[0] += sbat1;
            }
            else if (whereIsBat == 1)
            {
                secret[0] += sbat2;
            }
            secret[1] = "...There is a pit in room #";
            int whichpit = r.Next(2);
            if (whichpit == 0)
            {
                secret[1] += pit1;
            }
            else if (whichpit == 1)
            {
                secret[1] += pit2;
            }
            secret[3] = "The Wumpus is in room #" + wumpus;
            secret[4] = "Player currently in room #" + player;
            secret[5] = "There are four tidal extremes in one day.";
            return secret;
        }

        public string getsecret()
        {
            string error = "ERROR";
            int index = r.Next(6);
            if (index == 0)
            {
                return createSecrets()[0];
            }
            else if (index == 1)
            {
                return createSecrets()[1];
            }
            else if (index == 2)
            {
                return createSecrets()[5];
            }
            else if (index == 3)
            {
                return createSecrets()[3];
            }
            else if (index == 4)
            {
                return createSecrets()[4];
            }
            else if (index == 5)
            {
                return createSecrets()[5];
            }
            return error;
        }
        public Dictionary<int, int[]> LoadDoors()
        {
            allDoors = new Dictionary<int, int[]>();
            allDoors.Add(1, new int[] { 30, 26, 25, 2, 7, 6 });
            allDoors.Add(2, new int[] { 1, 26, 3, 9, 8, 7 });
            allDoors.Add(3, new int[] { 26, 27, 28, 4, 9, 2 });
            allDoors.Add(4, new int[] { 3, 28, 5, 11, 10, 9 });
            allDoors.Add(5, new int[] { 28, 29, 30, 6, 11, 4 });
            allDoors.Add(6, new int[] { 5, 30, 1, 7, 11, 12 });
            allDoors.Add(7, new int[] { 6, 1, 2, 8, 13, 12 });
            allDoors.Add(8, new int[] { 7, 2, 9, 15, 14, 13 });
            allDoors.Add(9, new int[] { 2, 3, 4, 10, 15, 8 });
            allDoors.Add(10, new int[] { 9, 4, 11, 17, 16, 15 });
            allDoors.Add(11, new int[] { 4, 5, 6, 12, 17, 10 });
            allDoors.Add(12, new int[] { 11, 6, 7, 13, 18, 17 });
            allDoors.Add(13, new int[] { 12, 7, 8, 14, 19, 18 });
            allDoors.Add(14, new int[] { 13, 8, 15, 21, 20, 19 });
            allDoors.Add(15, new int[] { 8, 9, 10, 16, 21, 14 });
            allDoors.Add(16, new int[] { 15, 10, 17, 21, 22, 23 });
            allDoors.Add(17, new int[] { 10, 11, 12, 16, 23, 18 });
            allDoors.Add(18, new int[] { 17, 12, 13, 19, 24, 23 });
            allDoors.Add(19, new int[] { 18, 13, 14, 20, 25, 24 });
            allDoors.Add(20, new int[] { 19, 14, 21, 27, 26, 25 });
            allDoors.Add(21, new int[] { 14, 15, 16, 22, 27, 20 });
            allDoors.Add(22, new int[] { 21, 16, 23, 29, 28, 27 });
            allDoors.Add(23, new int[] { 16, 17, 18, 24, 29, 22 });
            allDoors.Add(24, new int[] { 23, 18, 19, 25, 30, 29 });
            allDoors.Add(25, new int[] { 24, 19, 20, 26, 1, 30 });
            allDoors.Add(26, new int[] { 25, 20, 27, 1, 2, 3 });
            allDoors.Add(27, new int[] { 20, 21, 22, 26, 28, 3 });
            allDoors.Add(28, new int[] { 27, 22, 29, 5, 4, 3 });
            allDoors.Add(29, new int[] { 22, 23, 24, 30, 5, 28 });
            allDoors.Add(30, new int[] { 29, 24, 25, 1, 5, 6 });
            return allDoors;
        }

    }
}
