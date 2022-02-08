using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis
{
    public struct Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if(other is Vector2Int)
            {
                return this == (Vector2Int)other;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public float SqrMagnitude()
        {
            return X * X + Y * Y;
        }

        public float Magnitude()
        {
            return MathF.Sqrt(SqrMagnitude());
        }

        public override string ToString()
        {
            return nameof(Vector2Int) + "(" + X + "; "+ Y + ")";
        }


        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2Int operator -(Vector2Int a)
        {
            return new Vector2Int(-a.X, -a.Y);
        }

        public static Vector2Int operator *(Vector2Int a, int b)
        {
            return new Vector2Int(a.X * b, a.Y * b);
        }

        public static Vector2Int operator /(Vector2Int a, int b)
        {
            if(b == 0)
                throw new DivideByZeroException();

            return new Vector2Int(a.X / b, a.Y / b);
        }

        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return a.X != b.X || a.Y != b.Y;
        }
    }
}
