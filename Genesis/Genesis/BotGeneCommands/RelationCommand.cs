using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public abstract class RelationCommand : DirectionalCommand
    {
        public RelationCommand(bool relative) : base(relative)
        {
        }

        public sealed override void Apply(Bot bot, BotDirection direction)
        {
            Vector2Int lookPosition = bot.Position + direction.ToVector();

            EntityType entityType = bot.Map!.GetEntityType(lookPosition);
            Apply(bot, entityType, lookPosition);

            bot.MoveCommand(bot.GetCommand(bot.CurrentCommand + (int)entityType + 2).Code);
        }

        public abstract void Apply(Bot bot, EntityType entityType, Vector2Int lookPosition);
    }
}
