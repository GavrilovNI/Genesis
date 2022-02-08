using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class Rotate : DirectionalCommand
    {
        public override bool IsFinal => false;

        public Rotate(bool relative) : base(relative)
        {
            
        }

        public override void Apply(Bot bot, BotDirection direction)
        {
            bot.Direction = direction;
            bot.MoveCommand(2);
        }

        public override int GetCode()
        {
            return Relative ? 23 : 24;
        }
    }
}
