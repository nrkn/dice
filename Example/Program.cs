using Dice;
using System;
using System.Linq;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var seed = 54321;

			var number = 5;
			var sides = 6;
			var die = new Die(sides, new Random(seed));
			var values = die.Rolls(number);
			var list = String.Join(", ", values);
			var sum = values.Sum();
			var notation = $"{ number }d{ sides }";

			Console.WriteLine("Unseeded:");

			Roll(notation);
			Successes(3, 6, 3);
			Successes(10, 10, 8);

			Console.WriteLine($"Seeded with { seed }:");
			Console.WriteLine(
				$"  { notation } rolls = { list } = { sum }"
			);

			RollSeeded(notation, seed);
			RollSeeded("d6", seed);
			RollSeeded($"{ notation }*3", seed);
			RollSeeded($"{ notation }*3+2", seed);
			RollSeeded($"{ notation }L3", seed);
			RollSeeded($"{ notation }H3", seed);
			RollSeeded($"{ notation }H3*3", seed);
			RollSeeded($"{ notation }H3*3+2", seed);
			RollSeeded("d6", seed);
			RollSeeded("1 d 6", seed);
			RollSeeded("3d", seed);
			RollSeeded("3 d6 l2 *2 -1", seed);
			RollSeeded("10d10*10+10", seed);

			Console.WriteLine("Construct Roll from code");

			var modifiers = new RollModifier[]
			{
				new RollModifier( RollModifierType.Multiply, 2 ),
				new RollModifier( RollModifierType.Add, 1 ),
			};

			var keep = new RollKeep( RollKeepType.Highest, 2 );

			var roll = new Roll(number, sides, keep, modifiers);
			var result = new DiceFactory().Roll(roll);

			Console.WriteLine(
				$"  Rolled { roll.ToString() } = { result }"
			);

			Console.ReadKey();
		}

		static void Roll(string notation, DiceFactory dice)
		{
			var result = dice.Roll(notation);
			var normalizedRollString = Parser.Parse(notation).ToString();

			Console.WriteLine(
				$"  Rolled { notation } = { normalizedRollString } = { result }"
			);
		}

		static void Roll(string notation)
		{
			var dice = new DiceFactory();

			Roll(notation, dice);
		}

		static void RollSeeded(string notation, int seed)
		{
			var dice = new DiceFactory(new Random(seed));

			Roll(notation, dice);
		}

		static void Successes(int number, int sides, int minValue)
		{
			var dice = new DiceFactory();
			var die = dice.Die(sides);

			var successes = die.Successes(number, minValue);

			Console.WriteLine(
				$"  Successes for { number }{ die } = { successes }"
			);
		}
	}
}
