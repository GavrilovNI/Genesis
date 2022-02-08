using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class GeneAttack : BotGeneCommand
    {
        public override bool IsFinal => true;

        public override int Code => 52;

        public GeneAttack()
        {
        }

        public override void Apply(Bot bot)
        {
            BotDirection direction = bot.Direction;
            Vector2Int lookPosition = bot.Position + direction.ToVector();

            EntityType entityType = bot.Map!.GetEntityType(lookPosition);
            if(entityType == EntityType.Bot)
            {
                bot.GeneAttack((Bot)bot.Map!.GetEntity(lookPosition));
            }
            bot.MoveCommand(1);
        }
    }
}
