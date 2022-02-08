using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class Mutate : BotGeneCommand
    {
        public const int MUTATION_COUNT = 2;

        public override bool IsFinal => true;

        public override void Apply(Bot bot)
        {
            for (int i = 0; i < MUTATION_COUNT; i++)
                bot.Mutate();

            bot.MoveCommand(1);
        }

        public override int GetCode()
        {
            return 48;
        }
    }
}
