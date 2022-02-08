using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.BotGeneCommands
{
    public class CheckHeight : BotGeneCommand
    {
        public override bool IsFinal => false;
        public override int Code => 37;

        public override void Apply(Bot bot)
        {
            float compareWith = 1f * bot.GetCommand(bot.CurrentCommand + 1).Code * bot.Map!.Size.Y / bot.GeneSize;

            Vector2Int botPosition = bot.Position;

            int commandMoveDelta;
            if(botPosition.Y < compareWith)
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 2).Code;
            }
            else
            {
                commandMoveDelta = bot.GetCommand(bot.CurrentCommand + 3).Code;
            }
            bot.MoveCommand(commandMoveDelta);
        }
    }
}
