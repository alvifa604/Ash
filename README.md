# Ash Language

## Table of Contents

1. [Intro](#Intro)
2. [Operators](#Operators)
    1. [Arithmetic](#Arithmetic)
    2. [Assignment](#Assignment)
    3. [Comparison](#Comparison)
3. [Keywords](#Keywords)
4. [Rules](#Rules)
    1. [Variables](#Variables)

## Intro

This is a simple language that I made for fun and learning.

It's named Ash as A#, A as in my name and # as it's written in C#.

## Operators

### Arithmetic

1. Plus [+]: int/double + int/double
2. Minus [-]: int/double - int/double
3. Multiply [*]: int/double * int/double
4. Divide [/]: int/double / int/double
5. Exponent [^]: int/double ^ int/double
6. Parenthesis [( )]: (int/double)

### Assignment

1. Assigment [=]: variable = int/double

### Comparison

1. Equals [==]: int/double/bool == int/double/bool
2. Not equals [!=]: int/double/bool != int/double/bool
3. Greater than [>]: int/double > int/double
4. Less than [<]: int/double < int/double
5. Greater than or equal to [>=]: int/double >= int/double
6. Less than or equal to [<=]: int/double <= int/double

### Logical

1. And [&&]: bool && bool
2. Or [||]: bool || bool
3. Not [!]: !bool

## Keywords

- let
- true
- false

## Rules

### Variables

- Must be declared with the 'let' keyword (for now).
- Must start with a letter.
- Can include numbers and underscores.
- Cannot reassign types.

#### Example

let example_1 = 23

example_1 = true // Error



