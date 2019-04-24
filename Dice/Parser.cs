using System;
using System.Collections.Generic;

namespace Dice
{
	public static class Parser
	{
		public static Roll Parse(string notation)
		{
			notation = notation.ToUpper();

			var number = 0;
			var sides = 0;
			var modifiers = new List<RollModifier>();
			var currentState = State.Number;
			var digits = "";
			var currentModifier = new RollModifier();
			var keep = RollKeep.All;

			Action setNumber = () =>
			{
				if (digits.Length == 0)
				{
					number = 1;
				}
				else
				{
					number = int.Parse(digits);
				}

				digits = "";
			};

			Action setSides = () =>
			{
				if (digits.Length == 0)
				{
					sides = 6;
				}
				else
				{
					sides = int.Parse(digits);
				}

				digits = "";
			};

			Action setKeep = () =>
			{
				if (digits.Length == 0)
				{
					keep.Amount = 1;
				}
				else
				{
					keep.Amount = int.Parse(digits);
				}

				digits = "";
			};

			Action setModifier = () =>
			{
				currentModifier.Amount = int.Parse(digits);
				modifiers.Add(currentModifier);

				digits = "";
			};

			for (var i = 0; i < notation.Length; i++)
			{
				var current = notation[i];

				switch (currentState)
				{
					case State.Number:
						if (current == 'D')
						{
							setNumber();
							currentState = State.Sides;
						}
						else if (current >= '0' && current <= '9')
						{
							digits += current;
						}
						else if (current != ' ')
						{
							throw new ArgumentException(
								$"Unexpected { current } in notation"
							);
						}
						break;
					case State.Sides:
						if (current == 'L')
						{
							setSides();
							currentState = State.Keep;
							keep.Type = RollKeepType.Lowest;
						}
						else if (current == 'H')
						{
							setSides();
							currentState = State.Keep;
							keep.Type = RollKeepType.Highest;
						}
						else if (CharToModifierType.ContainsKey(current))
						{
							setSides();
							currentState = State.Modifier;
							currentModifier = new RollModifier
							{
								Type = CharToModifierType[current]
							};
						}
						else if (current >= '0' && current <= '9')
						{
							digits += current;
						}
						else if (current != ' ')
						{
							throw new ArgumentException(
								$"Unexpected { current } in notation"
							);
						}
						break;
					case State.Keep:
						if (CharToModifierType.ContainsKey(current))
						{
							setKeep();
							currentState = State.Modifier;
							currentModifier = new RollModifier
							{
								Type = CharToModifierType[current]
							};
						}
						else if (current >= '0' && current <= '9')
						{
							digits += current;
						}
						else if (current != ' ')
						{
							throw new ArgumentException(
								$"Unexpected { current } in notation"
							);
						}
						break;
					case State.Modifier:
						if (CharToModifierType.ContainsKey(current))
						{
							setModifier();
							currentModifier = new RollModifier
							{
								Type = CharToModifierType[current]
							};
						}
						else if (current >= '0' && current <= '9')
						{
							digits += current;
						}
						else if (current != ' ')
						{
							throw new ArgumentException(
								$"Unexpected { current } in notation"
							);
						}
						break;
				}
			}


			if (currentState == State.Number)
			{
				throw new ArgumentException(
					$"Unexpected notation { notation }"
				);
			}
			else if (currentState == State.Sides)
			{
				setSides();
			}
			else if (currentState == State.Keep)
			{
				setKeep();
			}
			else if (currentState == State.Modifier)
			{
				setModifier();
			}

			return new Roll(number, sides, keep, modifiers);
		}

		enum State
		{
			Number, Sides, Keep, Modifier
		}

		public static Dictionary<char, RollModifierType> CharToModifierType =>
			new Dictionary<char, RollModifierType>
			{
				{ '+', RollModifierType.Add },
				{ '-', RollModifierType.Subtract },
				{ '*', RollModifierType.Multiply },
				{ '/', RollModifierType.Divide }
			};
	}
}
