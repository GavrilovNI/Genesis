using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Genesis.BotGeneCommands;

namespace Genesis.Entities
{
    public class Bot : Entity
    {
        public const int GENE_SIZE = 64;
        public const int FIGHT_BONUS = 100;
        public const int ENERGY_TO_SEPARATE = 150;
        public const int ENERGY_TO_GENE_ATTACK = 10;
        public const int MAX_MINERALS_TO_COVERT = 100;
        public const int MINERAL_TO_ENERGY_MULTIPLIER = 4;
        public const int ENERGY_FOR_ONE_ITERATION = 3;
        public const float MUTATION_CHANCE = 0.5f;
        public const int MAX_COMMANDS_IN_ITERATION = 15;

        private BotGeneCommand[] _gene;

        public BotDirection Direction { get; set; }
        public int CurrentCommand { get; private set; }

        private int _energy;
        private int _minerals;


        public int MaxEnergy => 1000;
        public int MaxMinerals => 1000;

        public int Energy
        {
            get { return _energy; }
            set
            {
                if (value < 0)
                    value = 0;
                if(value > MaxEnergy)
                    value = MaxEnergy;
                _energy = value;
                if (_energy <= 0)
                    Kill();
            }
        }
        public int Minerals
        {
            get { return _minerals; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > MaxMinerals)
                    value = MaxMinerals;
                _minerals = value;
            }
        }

        public int EnergyFromSun { get; private set; }
        public int EnergyFromOrganic { get; private set; }
        public int EnergyFromMinerals { get; private set; }

        public int GeneSize => _gene.Length;

        public override EntityType Type => EntityType.Bot;

        public Bot(Map map, int energy = 10) : base(map)
        {
            if (energy < 0)
                throw new ArgumentOutOfRangeException(nameof(energy));


            _gene = new BotGeneCommand[GENE_SIZE];
            Array.Fill(_gene, new Photosynthesis());
            Direction = BotDirection.Up;
            CurrentCommand = 0;

            Energy = energy;
            Minerals = 0;

            EnergyFromSun = 0;
            EnergyFromOrganic = 0;
            EnergyFromMinerals = 0;
        }

        public Bot(Bot other) : base(other.Map!)
        {
            _gene = new BotGeneCommand[other._gene.Length];
            Array.Copy(other._gene, _gene, _gene.Length);
            Direction = other.Direction;
            CurrentCommand = other.CurrentCommand;
            Energy = other.Energy;
            Minerals = other.Minerals;

            EnergyFromSun = 0;
            EnergyFromOrganic = 0;
            EnergyFromMinerals = 0;
        }

        public void DoIteration()
        {
            for (int i = 0; i < MAX_COMMANDS_IN_ITERATION; i++)
            {
                Energy -= ENERGY_FOR_ONE_ITERATION;
                if (IsAlive == false)
                {
                    return;
                }
                Minerals += Map!.GetMinerals(Position);
                BotGeneCommand command = _gene[CurrentCommand];
                command.Apply(this);

                if (Energy == MaxEnergy)
                {
                    Separate();
                    return;
                }

                if (command.IsFinal)
                {
                    return;
                }
            }
        }

        public void Photosynthesis()
        {
            if (IsAlive == false)
                return;
            int energyToAdd = Map!.GetSunEnergy(Position);
            AddEnergy(energyToAdd);
            EnergyFromSun += energyToAdd;
        }
        
        public void ConvertMinerals()
        {
            if (IsAlive == false)
                return;
            int mineralToConvert = (int)MathF.Min(_minerals, MAX_MINERALS_TO_COVERT);

            Minerals -= mineralToConvert;
            int energyToAdd = mineralToConvert * MINERAL_TO_ENERGY_MULTIPLIER;
            Energy += energyToAdd;
            EnergyFromMinerals += energyToAdd; // in original += mineralToConvert
        }

        public bool TryMove(Vector2Int position)
        {
            if (IsAlive == false)
                return false;
            if (Map!.GetEntityType(position) == EntityType.Void)
            {
                Map!.SetEntityPos(this, position);
                return true;
            }
            return false;
        }

        public override void Kill()
        {
            if (IsAlive == false)
                return;

            Vector2Int position = Position;
            Map map = Map!;
            base.Kill();
            map.AddEntity(position, new Organic(map));
        }

        public void GeneAttack(Bot other)
        {
            Energy -= ENERGY_TO_GENE_ATTACK;
            if (IsAlive == false)
                return;
            other.Mutate();
        }

        public void Fight(Bot other)
        {
            if (IsAlive == false)
                return;

            if (ReferenceEquals(this, other))
                throw new ArgumentException("Bot can't fight himself");

            if(Minerals >= other.Minerals)
            {
                Minerals -= other.Minerals;
                int energyToAdd = FIGHT_BONUS + other.Energy / 2;
                Energy += energyToAdd;
                other.Kill();
                EnergyFromOrganic += energyToAdd;
            }
            else
            {
                Minerals = 0;
                other.Minerals -= Minerals;
                if(Energy >= 2 * other.Minerals)
                {
                    int energyToAdd = FIGHT_BONUS + other.Energy / 2 - 2 * other.Minerals;
                    Energy += energyToAdd;
                    other.Minerals = 0;
                    other.Energy = 0;
                    other.Kill();
                    EnergyFromOrganic += energyToAdd;
                }
                else
                {
                    other.Minerals -= Energy; // in original :  other.Minerals -= Energy / 2;
                    Energy = 0;
                    Kill();
                }
            }
        }

        public void EatOrganic(Organic organic)
        {
            AddEnergy(organic.GetEnergy());
            organic.Kill();
        }

        public void Mutate()
        {
            if (IsAlive == false)
                return;

            int mutationPos = Map!.Random.Next(0, _gene.Length);
            int commandCode = Map!.Random.Next(0, _gene.Length);
            BotGeneCommand newCommand = BotGeneCommand.GenerateCommand(commandCode);
            _gene[mutationPos] = newCommand;
        }

        public Bot? Separate()
        {
            if (IsAlive == false)
                return null;

            Energy -= ENERGY_TO_SEPARATE;
            if(Energy <= 0)
            {
                Kill();
                return null;
            }
            Vector2Int? position = Map!.GetEmptyPositionAroundPosition(Position);
            if (position == null)
            {
                Kill();
                return null;
            }

            Bot newBot = new Bot(this);
            Map!.AddEntity(position.Value, newBot);

            newBot.Energy = Energy / 2;
            Energy -= newBot.Energy;

            newBot.Minerals = Minerals / 2;
            Minerals -= newBot.Minerals;

            newBot.Direction = (BotDirection)Map.Random.Next(BotDirectionExtensions.GetDirectionsCount());

            if(Map.Random.NextSingle() < MUTATION_CHANCE)
            {
                newBot.Mutate();
            }
            return newBot;
        }

        public void Share(Bot other)
        {
            if (IsAlive == false)
                return;

            if (ReferenceEquals(this, other))
                throw new ArgumentException("Bot can't share with himself");

            if (Energy >= other.Energy)
            {
                int averageEnergy = (Energy + other.Energy) / 2;
                Energy = averageEnergy;
                other.Energy = averageEnergy;
            }
            if (Minerals >= other.Minerals)
            {
                int averageMinerals = (Minerals + other.Minerals) / 2;
                Minerals = averageMinerals;
                other.Minerals = averageMinerals;
            }
        }
        
        public void GiveQuater(Bot other)
        {
            if (IsAlive == false)
                return;

            if (ReferenceEquals(this, other))
                throw new ArgumentException("Bot can't share with himself");

            int energyToGive = Energy / 4;
            Energy -= energyToGive;
            other.Energy += energyToGive;

            int mineralsToGive = Minerals / 4;
            Minerals -= mineralsToGive;
            other.Minerals += mineralsToGive;
        }

        public void AddEnergy(int count)
        {
            if(count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Energy += count;
        }

        public BotGeneCommand GetCommand(int index)
        {
            if (index >= _gene.Length)
                index %= _gene.Length;
            if(index < 0)
                index += _gene.Length;
            return _gene[index];
        }

        public void MoveCommand(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            CurrentCommand += count;
            if(CurrentCommand >= _gene.Length)
                CurrentCommand %= _gene.Length;
        }
    }
}
