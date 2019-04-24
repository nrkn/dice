using System;
using System.Linq;

namespace Dice
{
	public class Die
	{
		private readonly Random random;

		public Die(int sides = 6, Random random = null)
		{
			if (sides < 2)
				throw new ArgumentOutOfRangeException(
					"Must have at least 2 sides"
				);

			this.random = random ?? new Random();
			Sides = sides;
		}

		public int Roll() => random.Next(Sides) + 1;

		public int Roll(int numberOfDice) => Rolls(numberOfDice).Sum();

		public int[] Rolls(int numberOfDice)
		{
			var values = new int[numberOfDice];

			for (var i = 0; i < numberOfDice; i++)
			{
				values[i] = Roll();
			}

			return values;
		}

		public int Successes(int numberOfDice, Func<int, bool> predicate)
		{
			var values = Rolls(numberOfDice);

			var successes = 0;

			for (var i = 0; i < values.Length; i++)
			{
				if (predicate(values[i])) successes++;
			}

			return successes;
		}

		public int Successes(int numberOfDice, int minValue)
		{
			if (minValue > Sides)
				throw new ArgumentOutOfRangeException(
					"minValue is higher than sides"
				);

			return Successes(numberOfDice, n => n >= minValue);
		}

		public int Sides { get; }

		public override string ToString()
		{
			return $"d{ Sides }";
		}
	}
}
