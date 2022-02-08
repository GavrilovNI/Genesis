using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genesis.Utils;

namespace Genesis.BotGeneCommands
{

    public abstract class BotGeneCommand
    {
        public abstract void Apply(Bot bot);
        public abstract int GetCode();

        public abstract bool IsFinal { get; }

        public static BotGeneCommand GenerateCommand(int code)
        {
            switch(code)
            {
                case 23:
                    return new Rotate(true);
                case 24:
                    return new Rotate(false);
                case 25:
                    return new Photosynthesis();
                case 26:
                    return new Move(true);
                case 27:
                    return new Move(false);
                case 28:
                    return new Eat(true);
                case 29:
                    return new Eat(false);
                case 30:
                    return new Look(true);
                case 31:
                    return new Look(false);
                case 32:
                    return new Share(true);
                case 33:
                    return new Share(false);
                case 34:
                    return new GiveQuater(true);
                case 35:
                    return new GiveQuater(false);
                case 36:
                    return new RotateHorizontally();
                case 37:
                    return new CheckHeight();
                case 38:
                    return new CheckEnergy();
                case 39:
                    return new CheckMinerals();


                case 41:
                    return new Separate();

                case 43:
                    return new CheckIfHasEmptyPositionAround();
                case 44:
                    return new CheckIfCanDoPhotosynthesis();
                case 45:
                    return new CheckIfCanGetMinerals();

                case 47:
                    return new ConvertMinerals();
                case 48:
                    return new Mutate();

                default:
                    return new CommandMover(code);
            }
        }
    }
}
