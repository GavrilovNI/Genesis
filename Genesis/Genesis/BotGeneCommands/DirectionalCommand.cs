using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public abstract class DirectionalCommand : BotGeneCommand
    {
        public bool Relative { get; private set; }

        public DirectionalCommand(bool relative)
        {
            Relative = relative;
        }

        private BotDirection GetDirectionFromCode(int code, Bot bot)
        {
            BotDirection direction = BotDirectionExtensions.DirectionFromCode(code);
            if (Relative)
                direction = direction.Add(bot.Direction);
            return direction;
        }

        public sealed override void Apply(Bot bot)
        {
            BotGeneCommand nextCommand = bot.GetCommand(bot.CurrentCommand + 1);
            BotDirection direction = GetDirectionFromCode(nextCommand.Code, bot);
            Apply(bot, direction);
        }

        public abstract void Apply(Bot bot, BotDirection direction);
    }
}
