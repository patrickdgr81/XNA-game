using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
namespace Main
{
    class Cave
    {

        static String[] caveLines = new String[30];
        Dictionary<int, int[]> doors = new Dictionary<int, int[]>();
        public void ReadMap(int mapNumber)
        {
            // create reader & open file
            TextReader caveReader = new StreamReader("map" + mapNumber + ".txt");
            // read a line of text
            for (int i = 0; i < 30; i++)
            {
                caveLines[i] = caveReader.ReadLine();
            }

            // close the stream
            caveReader.Close();
        }

        public Dictionary<int, int[]> GetRooms()
        {
            for (int x = 1; x <= 30; x++)
            {
                String[] roomStrings = new String[3];
                int[] rooms = new int[3];
                roomStrings = caveLines[x - 1].Split(',');
                for (int i = 0; i <= 2; i++)
                {
                    rooms[i] = int.Parse(roomStrings[i]);
                }
                doors.Add(x, rooms);
            }
            return doors;

        }



    }
}
