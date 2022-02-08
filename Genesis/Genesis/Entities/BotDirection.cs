using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Entities
{
    public enum BotDirection : int
    {
        UpLeft = 0,
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
    }

    public static class BotDirectionExtensions
    {
        // 0 1 2
        // 7   3
        // 6 5 4
        private static Vector2Int[] _directionToVector = new Vector2Int[]
        {
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
        };

        public static Vector2Int ToVector(this BotDirection direction)
        {
            return _directionToVector[(int)direction];
        }

        public static BotDirection Add(this BotDirection direction, BotDirection value)
        {
            int directionInt = (int)direction + (int)value;
            directionInt %= GetDirectionsCount();

            return (BotDirection)directionInt;
        }

        public static int GetDirectionsCount()
        {
            return Enum.GetNames(typeof(BotDirection)).Length;
        }

        public static BotDirection DirectionFromCode(int code)
        {
            code %= GetDirectionsCount();
            if (code < 0)
                code += GetDirectionsCount();
            return (BotDirection)code;
        }
    }
}
