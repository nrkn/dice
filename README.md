# dice

A .NET library for rolling dice

## Die Class

### ctor
```cs
// defaults to 6
var d6 = new Die();
var d8 = new Die( 8 );

// seeded
var d10 = new Die( 10, new Random( 54321 ) );
```

### methods

#### Roll

```cs
// roll once
int result = d6.Roll();

// sum of 3 rolls
int result3d6 = d6.Roll( 3 );

// the actual dice value for each of 3 rolls
int[] results = d6.Rolls( 3 );

// dice pools
var successThreshold = 4;
int successes = d6.Successes( 10, successThreshold );

// dice pools with custom predicate
Func<int, bool> predicate = n => n == 3;
int successesWhen3 = d6.Successes( 10, predicate );
```

## DieFactory Class

### ctor
```cs
// unseeded
var dice = new DiceFactory();
```

```cs
// seeded
var dice = new DiceFactory( new Random( 54321 ) );
```

### methods

#### Die Factory

```cs
// die using same seed as factory
Die d6 = dice.Die( 6 );
```

#### Roll

```cs
// 1d6 by default
int result = dice.Roll();
```

```cs
// 2d6
int result = dice.Roll( 2 );
```

```cs
// sum of 3d8
int result = dice.Roll( 3, 8 );
```

```cs
// from simple notation
int result = dice.Roll( "3d6" );
```

```cs
// keep the highest 2 results
int result = dice.Roll( "3d6H2" );
```

```cs
// keep the lowest 2 result
int result = dice.Roll( "3d6L2" );
```

```cs
// modifiers - evaluated in order - supports * / + -
int result = dice.Roll( "3d6*2+1" );
```

```cs
// if H<n> or L<n> are present, modifiers must come after
int result = dice.Roll( "3d6H2*2+1" );
```

## Roll Class

You can also construct rolls from code instead of using notation:

### ctor

```cs
// 3d6
var roll = new Roll( 3, 6 );

var dice = DiceFactory();

var result = dice.Roll( roll );
```

```cs
// 3d6, keep highest two
var keep = new RollKeep( RollKeepType.Highest, 2 );
var roll = new Roll( 3, 6, keep );

var dice = DiceFactory();

var result = dice.Roll( roll );
```

```cs
// 3d6 with modifiers
var modifiers = new RollModifier[]
{
  new RollModifier( RollModifierType.Multiply, 2 ),
  new RollModifier( RollModifierType.Add, 1 ),
};

var roll = new Roll( 3, 6, modifiers );

var dice = DiceFactory();

var result = dice.Roll( roll );
```

```cs
// everything
var keep = new RollKeep( RollKeepType.Highest, 2 );

var modifiers = new RollModifier[]
{
  new RollModifier( RollModifierType.Multiply, 2 ),
  new RollModifier( RollModifierType.Add, 1 ),
};

var roll = new Roll( 3, 6, keep, modifiers );

var dice = DiceFactory();

var result = dice.Roll( roll );
```

## Parser Class

Parses a string into a `Roll` instance

```cs
Roll roll = Parser.parse( "3d6" );
```

## license

MIT License

Copyright (c) 2019 Nik Coughlin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.