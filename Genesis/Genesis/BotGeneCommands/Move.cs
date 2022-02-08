using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class Move : RelationCommand
    {
        public Move(bool relative) : base(relative)
        {
        }

        public override void Apply(Bot bot, EntityType entityType, Vector2Int lookPosition)
        {
            bot.TryMove(lookPosition);
        }

        public override int GetCode()
        {
            return Relative ? 26 : 27;
        }
    }
}
