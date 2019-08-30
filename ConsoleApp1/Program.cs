using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApp1
{
    class Program
    {
        // Checks if positions are near each other to form a letter
        static bool ComparePositions(Point a, Point b)
        {
            // If the x coordinate is too far, return false
            if ((a.X-b.X)< -1 || (a.X - b.X) > 1)
            {
                return false;
            }
            //Don't need to check Y
            //if ((a.Y - b.Y) < -1 || (a.Y - b.Y) > 1)
            //{
            //    return false;
            //}
            //Otherwise, points are close, return true
            return true;
        }

        static void PrintLetters(List<Point> letter)
        {
            //Sort with X first to get lowest X point
            List<Point> sortedLetter = new List<Point>(letter.OrderBy(point => point.X).ThenBy(point => point.Y));
            int lowestX = letter[0].X;

            //Sort list by y, then x
            sortedLetter = new List<Point>(letter.OrderBy(point => point.Y).ThenBy(point => point.X));

            Point lastPoint = new Point();
            
            foreach(Point p in sortedLetter)
            {
                //Check if first point
                if(lastPoint == new Point())
                {
                    lastPoint = p;
                    Console.Write(String.Concat(Enumerable.Repeat("-", p.X-lowestX)) + "#");
                    
                }
                //Check if on same row
                else if(lastPoint.Y == p.Y)
                {
                    //Check if point is further than 1 from last point
                    //If so, print dashes than point
                    if (p.X - lastPoint.X > 1)
                    {
                        Console.Write(String.Concat(Enumerable.Repeat("-",p.X-lastPoint.X-1))+"#");
                    }
                    //If not, print point
                    else
                    {
                        Console.Write("#");
                    }
                    lastPoint = p;
                //Not on same row, so start new row
                }else{
                    Console.WriteLine();
                    Console.Write(String.Concat(Enumerable.Repeat("-", p.X - lowestX)) + "#");
                    lastPoint = p;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            return;
        }

        static void Main(string[] args)
        {
            List<int[]> Positions = new List<int[]>();
            string input = Console.ReadLine();

            //Get positions and velocities from input
            while (input != "") {

                String[] separator = { "position=<", ",", "> velocity=<", ",", ">" };
                int count = 5;

                String[] numberlist = input.Split(separator, count, StringSplitOptions.RemoveEmptyEntries);

                int px = Convert.ToInt32(numberlist[0]);
                int py = Convert.ToInt32(numberlist[1]);
                int vx = Convert.ToInt32(numberlist[2]);
                int vy = Convert.ToInt32(numberlist[3]);

                int[] position = { px, py, vx, vy };
                Positions.Add(position);

                input = Console.ReadLine();
            }

            bool areLetters = false;
            //Find word in the positions
            while (!areLetters)
            {
                //Change position of coordinates
                Console.WriteLine("######UNSORTED#####");
                int count = 0;
                foreach (int[] i in Positions)
                {
                    Positions[count][0] += i[2];
                    Positions[count][1] += i[3];
                    Console.WriteLine(Positions[count][0].ToString() + ", "+ Positions[count][1].ToString());
                    count++;
                }

                Console.WriteLine("#######SORTED#######");
                //Sort coordinates by x value, then y value
                List<int[]> sortedPositions = new List<int[]>(Positions.OrderBy(position => position[0]).ThenBy(position => position[1]));

                //Group close coordinates
                List<List<Point>> PositionGroups = new List<List<Point>>();
                count = 0;
                foreach (int[] posa in sortedPositions)
                {
                    Point position = new Point(posa[0], posa[1]);

                    //Adds first point in list to group
                    if (PositionGroups.Count == 0)
                    {
                        List<Point> positions = new List<Point>();
                        positions.Add(position);
                        PositionGroups.Add(positions);
                        Console.WriteLine(position.X.ToString() + ", " + position.Y.ToString());
                        continue;
                    }

                    //Check if position near current group positions
                    bool added = false;
                    foreach(Point posb in PositionGroups[count])
                    {
                        if (ComparePositions(position, posb))
                        {
                            PositionGroups[count].Add(position);
                            added = true;
                            break;
                        }
                    }

                    //If position was not near current group, create new one
                    if (!added)
                    {
                        List<Point> positions = new List<Point>();
                        positions.Add(position);
                        PositionGroups.Add(positions);
                        count ++;
                    }

                    Console.WriteLine(position.X.ToString() + ", " + position.Y.ToString());
                }

                //Logic to determine if group is a letter
                areLetters = true;
                foreach(List<Point> positions in PositionGroups)
                {
                    //Check if any points are overlapping
                    List<Point> loopedPoints = new List<Point>();
                    foreach (Point position in positions)
                    {
                        //If point already exists, not letter
                        if (loopedPoints.Contains(position))
                        {
                            areLetters = false;
                            break;
                        }
                        else
                        {
                            loopedPoints.Add(position);
                        }
                        
                    }

                    if (positions.Count< 5 || !areLetters)
                    {
                        areLetters = false;
                        break;
                    }
                }

                if (areLetters)
                {
                    //Want to sort by Y then X, to properly print letters
                    //sortedPositions = new List<int[]>(Positions.OrderBy(position => position[1]).ThenBy(position => position[0]));
                    foreach (List<Point> letter in PositionGroups)
                    {
                        PrintLetters(letter);
                    }
                }
                

                //Console.ReadKey();
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
