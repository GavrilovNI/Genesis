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
        public Dictionary<Season, float> SeasonToSunDepth = new Dictionary<Season, float>()
        {
            { Season.Summer, 0.7f },
            { Season.Fall, 0.63f },
            { Season.Spring, 0.63f },
            { Season.Winter, 0.57f },
        };
        public Dictionary<Season, float> SeasonToSunEnergyMupltiplier = new Dictionary<Season, float>()
        {
            { Season.Summer, 35 },
            { Season.Fall, 33 },
            { Season.Spring, 33 },
            { Season.Winter, 31 },
        };

        private BidirectionalDictrionary<Vector2Int, Entity> _entities;
        private BidirectionalDictrionary<Vector2Int, Bot> _bots;

        public Vector2Int Size { get; private set; }
        public Season Season { get; private set; }

        private int _iteration;

        public Random Random { get; private set; }

        public Map(Vector2Int size, Random random)
        {
            if (size.X <= 0 || size.Y <= 0)
                throw new ArgumentException(nameof(size) + " must be more than (0; 0).");

            _entities = new BidirectionalDictrionary<Vector2Int, Entity>();
            _bots = new BidirectionalDictrionary<Vector2Int, Bot>();

            Size = size;
            Season = Season.Summer;
            _iteration = 0;
            Random = random;
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
            if(entity is Bot)
                _bots.Add(position, (Bot)entity);
        }

        public void RemoveEnt(Vector2Int position)
        {
            position = NormalizePosition(position);
            if (_entities.ContainsKey(position))
            {
                if(_entities[position] is Bot)
                {
                    _bots.Remove(position);
                }
                _entities.Remove(position);
            }
            else
                throw new InvalidOperationException("No entity found in " + nameof(position) + ".");
        }
        public void RemoveEnt(Entity entity)
        {
            if (_entities.Reverse.ContainsKey(entity))
            {
                _entities.Reverse.Remove(entity);
                if(entity is Bot)
                    _bots.Reverse.Remove((Bot)entity);
            }
            else
                throw new InvalidOperationException("Entity not found.");
        }

        public Dictionary<Vector2Int, Entity>.Enumerator GetEnumerator()
        {
            return new BidirectionalDictrionary<Vector2Int, Entity>(_entities).GetEnumerator();
        }

        public void DoIteration()
        {
            var entities = new BidirectionalDictrionary<Vector2Int, Bot>(_bots);
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
            if(entity is Bot)
            {
                Bot bot = (Bot)entity;
                _bots.Reverse.Remove(bot);
                _bots.Add(position, bot);
            }
        }

        public Vector2Int NormalizePosition(Vector2Int position)
        {
            if(position.X < 0 || position.X >= Size.X)
            {
                position.X %= Size.X;
                if(position.X < 0)
                    position.X += Size.X;
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

            float maxDepth = SeasonToSunDepth[Season] * Size.Y;

            if (position.Y > maxDepth)
                return 0;

            float sunEnergyMupltiplier = SeasonToSunEnergyMupltiplier[Season];
            int result = (int)MathF.Ceiling((1f - position.Y / maxDepth) * sunEnergyMupltiplier);

            return result;
        }

        public int GetMinerals(Vector2Int position)
        {
            position = NormalizePosition(position);
            if (position.Y > Size.Y)
                return 0;

            int count = 3;
            int one = Size.Y / 2 / count;

            int minDepth = Size.Y / 2;

            if (position.Y <= minDepth)
                return 0;

            int result = (int)(( MathF.Ceiling(1f * (position.Y - minDepth) / (Size.Y - minDepth) * count)));

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
