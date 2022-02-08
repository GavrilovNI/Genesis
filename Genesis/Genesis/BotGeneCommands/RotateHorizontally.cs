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
        public override bool IsFinal => false;
        public override int Code => 36;

        public override void Apply(Bot bot)
        {
            Random random = bot.Map!.Random;
            if (random.NextSingle() < 0.5f)
                bot.Direction = BotDirection.Right;
            else
                bot.Direction = BotDirection.Left;
            bot.MoveCommand(1);
        }
    }
}
