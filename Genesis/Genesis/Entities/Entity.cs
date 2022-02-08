using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Entities
{
    public abstract class Entity
    {
        public Map? Map { get; private set; }
        public abstract EntityType Type { get; }

        public Vector2Int Position => Map!.GetEntityPos(this)!.Value;

        public bool IsAlive => Map != null;

        public Entity(Map map)
        {
            if(map == null)
                throw new ArgumentNullException(nameof(map));
            Map = map;
        }

        public virtual void Kill()
        {
            if (IsAlive == false)
                return;
            Map!.RemoveEnt(this);
            Map = null;
        }

    }
}
