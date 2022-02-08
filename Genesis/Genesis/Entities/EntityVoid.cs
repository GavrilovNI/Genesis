using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Entities
{
    public sealed class EntityVoid : Entity
    {
        public EntityVoid(Map map) : base(map)
        {
        }

        public override EntityType Type => EntityType.Void;

        public override void Kill()
        {
            
        }
    }
}
