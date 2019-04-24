using System;
using System.Collections.Generic;
using System.Text;

namespace Dice
{
	public class Roll
	{
		public Roll(int number, int sides) :
			this(number, sides, RollKeep.All)
		{ }

		public Roll(int number, int sides, RollKeep keep) :
			this(number, sides, keep, new RollModifier[0])
		{ }

		public Roll(
			int number, int sides, IEnumerable<RollModifier> modifiers
		) : this(number, sides, RollKeep.All, modifiers) { }

		public Roll(
			int number, int sides, RollKeep keep,
			IEnumerable<RollModifier> modifiers
		)
		{
			Number = number;
			Sides = sides;
			Modifiers = (IReadOnlyCollection<RollModifier>)modifiers;

			if (keep.Type != RollKeepType.All && keep.Amount > number)
			{
				throw new ArgumentOutOfRangeException(
					$"Cannot keep more dice than are being rolled"
				);
			}

			this.keep = keep;
		}

		public int Number { get; private set; }
		public int Sides { get; private set; }
		public IReadOnlyCollection<RollModifier> Modifiers;
		public RollKeep Keep => keep;

		private RollKeep keep;

		public override string ToString()
		{
			var builder = new StringBuilder();

			builder.Append($"{ Number }d{ Sides }");

			switch (Keep.Type)
			{
				case RollKeepType.Lowest:
					builder.Append($"L{ Keep.Amount}");
					break;
				case RollKeepType.Highest:
					builder.Append($"H{ Keep.Amount}");
					break;
			}

			foreach (var modifier in Modifiers)
			{
				builder.Append(modifier.ToString());
			}

			return builder.ToString();
		}
	}

	public struct RollKeep
	{
		public RollKeep( RollKeepType type = RollKeepType.All, int amount = 0 )
		{
			Type = type;
			Amount = amount;
		}

		public RollKeepType Type;
		public int Amount;

		public static RollKeep All = new RollKeep { Type = RollKeepType.All };
	}

	public struct RollModifier
	{
		public RollModifier( 
			RollModifierType type = RollModifierType.Add, int amount = 0 
		)
		{
			Type = type;
			Amount = amount;
		}

		public RollModifierType Type;
		public int Amount;

		public override string ToString()
		{
			var c = ModifierTypeToChar[Type];

			return $"{ c }{ Amount }";
		}

		public static Dictionary<RollModifierType, char> ModifierTypeToChar =>
			new Dictionary<RollModifierType, char>
			{
				{ RollModifierType.Add, '+' },
				{ RollModifierType.Subtract, '-' },
				{ RollModifierType.Multiply, '*' },
				{ RollModifierType.Divide, '/' }
			};
	}

	public enum RollModifierType
	{
		Add, Subtract, Multiply, Divide
	}

	public enum RollKeepType
	{
		All, Lowest, Highest
	}

}
