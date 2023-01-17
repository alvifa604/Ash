# Ash Language

## Table of Contents

1. [Intro](#Intro)
2. [Types](#Types)
3. [Operators](#Operators)
    1. [Arithmetic](#Arithmetic)
    2. [Assignment](#Assignment)
    3. [Comparison](#Comparison)
4. [Keywords](#Keywords)
5. [Rules](#Rules)
    1. [Variables](#Variables)
    2. [If Statements](#If-statements)
    3. [Else Statements](#Else-statements)
    4. [For loops](#for-loops)
    5. [While loops](#while-loops)

## Intro

This is a simple language that I made for fun and learning.

It's named Ash as A#, A as in my name and # as it's written in C#.

## Types

- Integer: number with no decimal.
- Double: number with decimal.
- Boolean: true or false.

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

- integer
- double
- boolean
- let
- true
- false
- if
- else
- for
- to
- step
- while
- break
- function

## Rules

### Variables

- Must be declared with the 'let' keyword (for now).
- Must start with a letter.
- Can include numbers and underscores.
- Cannot reassign types.
- Declare and assign in one line.
- Declare and assign must end with a semicolon.
- Cannot be a keyword.
- Are case sensitive.
- Are accessed with the variable name.

#### Example

```
// Declaring and assigning variables
let example_1 = 23;
let example_2 = 23.5;
let example_3 = true;

// Reassigning variables
example_1 = true // Error
example_1 = 24; // Valid
```

### If statements

- Must be declared with the 'if' keyword.
- Must have a condition.
- Must have a body.
- The body must be surrounded by curly braces.

#### Example

```
if(1 == 1) {

   // Do something

}
```

### Else statements

- Must be declared with the 'else' keyword.
- Must have a body.
- The body must be surrounded by curly braces.
- Must be declared after an if statement.

#### Example

```
if(1 == 2){

   // Do something
   
} else {

   // Do something else
   
}
```

### For loops

- Must be declared with the 'for' keyword.
- Counter must be declared within the parenthesis.
- Elements within the parenthesis must be separated by a semicolon.
- Must have a condition.
- Must have a body.
- The step is optional (default is 1), but when used, it must be declared after the upper bound.
- The body must be surrounded by curly braces.
- Can be exited with the 'break' keyword.

#### Example

```
for (let i = 0; to 10; step 2) {

   // Do something

}

OR

for (let i = 0; to 10) {

   // Do something

}
```

### While loops

- Must be declared with the 'while' keyword.
- Must have a condition.
- Must have a body.
- The body must be surrounded by curly braces.
- Can be exited with the 'break' keyword.

#### Example

```
while (1 == 1) {

   // Do something

}
```

### Break

- Must be declared with the 'break' keyword.
- Can be used in for and while loops.
- Exits the loop.
- Ends with a semicolon.

#### Example

```
break;
```

### Function declaration

- Must be declared with the 'function' keyword.
- Must have a name.
- Must have a body.
- The body must be surrounded by curly braces.
- Parameters are optional.
- Parameters must be separated by a comma.
- Parameters must have the type specified. [See Types](#Types).

#### Example

```
function example() {

   // Do something

}

OR

function example(integer a, double b) {

   // Do something

}
```