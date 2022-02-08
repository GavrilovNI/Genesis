using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Entities
{
    public class Organic : Entity
    {
        public Organic(Map map) : base(map)
        {
        }

        public override EntityType Type => EntityType.Organic;

        public int GetEnergy()
        {
            return 100;
        }
    }
}
