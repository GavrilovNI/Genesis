using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class GiveQuater : RelationCommand
    {
        public override bool IsFinal => false;

        public GiveQuater(bool relative) : base(relative)
        {
        }

        public override void Apply(Bot bot, EntityType entityType, Vector2Int lookPosition)
        {
            if (entityType == EntityType.Bot)
            {
                Bot other = (Bot)bot.Map!.GetEntity(lookPosition);
                bot.GiveQuater(other);
            }
        }

        public override int GetCode()
        {
            return Relative ? 34 : 35;
        }
    }
}
