using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Genesis.Entities;
using Genesis.Utils;

namespace Genesis
{
    public enum Season
    {
        Summer,
        Fall,
        Spring,
        Winter
    }

    public class Map
    {
        private BidirectionalDictrionary<Vector2Int, Entity> _entities;

        public Vector2Int Size { get; private set; }
        public Season Season { get; private set; }

        private int _iteration;

        public Map(Vector2Int size)
        {
            if (size.X <= 0 || size.Y <= 0)
                throw new ArgumentException(nameof(size) + " must be more than (0; 0).");

            _entities = new BidirectionalDictrionary<Vector2Int, Entity>();

            Size = size;
            Season = Season.Summer;
            _iteration = 0;
        }

        public Entity GetEntity(Vector2Int position)
        {
            position = NormalizePosition(position);
            if (_entities.TryGetValue(position, out Entity? entity))
                return entity!;
            return new EntityVoid(this);
        }

        public void AddEntity(Vector2Int position, Entity entity)
        {
            position = NormalizePosition(position);
            _entities.Add(position, entity);
        }

        public void RemoveEnt(Vector2Int position)
        {
            position = NormalizePosition(position);
            if (_entities.ContainsKey(position))
                _entities.Remove(position);
            else
                throw new InvalidOperationException("No entity found in " + nameof(position) + ".");
        }
        public void RemoveEnt(Entity entity)
        {
            if (_entities.Reverse.ContainsKey(entity))
                _entities.Reverse.Remove(entity);
            else
                throw new InvalidOperationException("Entity not found.");
        }

        public Dictionary<Vector2Int, Entity>.Enumerator GetEnumerator()
        {
            return new BidirectionalDictrionary<Vector2Int, Entity>(_entities).GetEnumerator();
        }

        public void DoIteration()
        {
            var entities = new BidirectionalDictrionary<Vector2Int, Entity>(_entities);
            int i = 0;
            foreach (var entity in entities)
            {
                Bot? bot = entity.Value as Bot;
                if (bot != null)
                    bot.DoIteration();
                i++;
            }
            _iteration++;
            if(_iteration%1000 ==0)
            {
                Season = (Season)(((int)Season + 1) % Enum.GetNames(typeof(Season)).Length);
            }
        }


        public Vector2Int? GetEntityPos(Entity entity)
        {
            if (_entities.Reverse.TryGetValue(entity, out Vector2Int position))
                return position;
            return null;
        }

        public void SetEntityPos(Entity entity, Vector2Int position)
        {
            position = NormalizePosition(position);
            if (_entities.TryGetValue(position, out Entity? oldEntity) && (oldEntity != null && oldEntity != entity))
                throw new ArgumentException(nameof(position) + " taken by another entity.");

            _entities.Reverse.Remove(entity);
            _entities.Add(position, entity);
        }

        public Vector2Int NormalizePosition(Vector2Int position)
        {
            if(position.X < 0 || position.X >= Size.X)
            {
                position.X %= Size.X;
                if(position.X < 0)
                    position.X += Size.X;
            }

            if (position.Y < 0 || position.Y >= Size.Y)
            {
                position.Y %= Size.Y;
                if (position.Y < 0)
                    position.Y += Size.Y;
            }

            return position;
        }

        public EntityType GetEntityType(Vector2Int position)
        {
            if (position.Y < 0 || position.Y >= Size.Y)
                return EntityType.Wall;
            position = NormalizePosition(position);

            if(_entities.ContainsKey(position))
            {
                _entities.TryGetValue(position, out Entity? entity);
                return entity!.Type;
                //return _entities[position].Type;
            }
            return EntityType.Void;
        }

        public int GetSunEnergy(Vector2Int position)
        {
            position = NormalizePosition(position);
            if (position.Y < 0)
                return 0;

            int one = (int)(0.0625 * Size.Y);
            int count;
            switch(Season)
            {
                case Season.Winter:
                    count = 9;
                    break;
                case Season.Spring:
                case Season.Fall:
                    count = 10;
                    break;
                case Season.Summer:
                    count = 11;
                    break;
                default:
                    throw new NotImplementedException();
            }

            int maxDepth = one * count;

            if (position.Y > maxDepth)
                return 0;

            int result = (int)((1f - 1f * position.Y / maxDepth) * count);

            return result;
        }

        public int GetMinerals(Vector2Int position)
        {
            position = NormalizePosition(position);
            if (position.Y > Size.Y)
                return 0;

            int count = 3;
            int one = Size.Y / 2 / count;

            int minDepth = one * count;

            if (position.Y < minDepth)
                return 0;

            int result = (int)(((position.Y - minDepth) / (Size.Y - minDepth)) * count);

            return result;
        }

        public Vector2Int? GetEmptyPositionAroundPosition(Vector2Int position)
        {
            position = NormalizePosition(position);
            List<Vector2Int> emptyPoses = new List<Vector2Int>();
            for (int y = position.Y - 1; y <= position.Y + 1; y++)
            {
                for (int x = position.X - 1; x <= position.X + 1; x++)
                {
                    if (x == position.X && y == position.Y)
                        continue;

                    Vector2Int currentPos = NormalizePosition(new Vector2Int(x, y));
                    if (GetEntityType(currentPos) == EntityType.Void)
                        emptyPoses.Add(currentPos);
                }
            }

            if (emptyPoses.Count > 0)
            {
                return emptyPoses[new Random().Next(emptyPoses.Count)];
            }
            return null;
        }
    }
}
