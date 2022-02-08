using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class RotateHorizontally : BotGeneCommand
    {
        public override void Apply(Bot bot)
        {
            Random random = new Random();
            if (random.NextSingle() < 0.5f)
                bot.Direction = BotDirection.Right;
            else
                bot.Direction = BotDirection.Left;
            bot.MoveCommand(1);
        }

        public override int GetCode()
        {
            return 36;
        }
    }
}
