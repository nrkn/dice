using System;
using System.Linq;

namespace Dice
{
	public class DiceFactory
	{
		private readonly Random random;

		public DiceFactory(Random random = null)
		{
			this.random = random ?? new Random();
		}

		public Die Die(int sides = 6) => new Die(sides, random);

		public int Roll(int number = 1, int sides = 6 ) =>
			Die(sides).Roll(number);

		public int Roll(Roll roll)
		{
			var die = Die(roll.Sides);
			var values = die.Rolls(roll.Number);

			if (roll.Keep.Type != RollKeepType.All)
			{
				Array.Sort(values);

				if (roll.Keep.Type == RollKeepType.Highest)
				{
					Array.Reverse(values);
				}

				Array.Resize(ref values, roll.Keep.Amount);
			}

			var sum = values.Sum();

			foreach (var modifier in roll.Modifiers)
			{
				switch (modifier.Type)
				{
					case RollModifierType.Add:
						sum += modifier.Amount;
						break;
					case RollModifierType.Subtract:
						sum -= modifier.Amount;
						break;
					case RollModifierType.Multiply:
						sum *= modifier.Amount;
						break;
					case RollModifierType.Divide:
						sum /= modifier.Amount;
						break;
				}
			}

			return sum;
		}

		public int Roll(string roll) => Roll(Parser.Parse(roll));
	}
}
