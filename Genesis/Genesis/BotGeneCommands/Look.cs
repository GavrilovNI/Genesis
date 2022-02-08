using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class Look : RelationCommand
    {
        public override bool IsFinal => false;
        public override int Code => Relative ? 30 : 31;

        public Look(bool relative) : base(relative)
        {
        }

        public override void Apply(Bot bot, EntityType entityType, Vector2Int lookPosition)
        {
            //doing nothing, just watching
        }
    }
}
