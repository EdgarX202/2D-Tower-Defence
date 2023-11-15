using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    // Properties
   public int X { get; set; }
   public int Y { get; set; }

   public Point(int x, int y)
   {
       this.X = x;
       this.Y = y;
   }

    public static bool operator == (Point one, Point two)
    {
        return one.X == two.X && one.Y == two.Y;
    }
    public static bool operator !=(Point one, Point two)
    {
        return one.X != two.X || one.Y != two.Y;
    }

    public static Point operator -(Point one, Point two)
    {
        return new Point(one.X - two.X, one.Y - two.Y);
    }
}
