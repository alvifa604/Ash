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

- Any: Any type of value
    - Default: 0.
- Integer: number with no decimal.
    - Default: 0.
- Double: number with decimal.
    - Default: 0.0.
- Boolean: true or false.
    - Default: false.

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

When declaring a variable this can be done in different ways:

1. `let variableName;`
    - This will create a variable with the type 'any'.
    - This allows further initialisation of the variable to other types.
2. `let variableName = value;`
    - This will create a variable with the type of the value.
    - This allows the compiler to infer the variable type.
3. `<type> variableName;`
    - This will create a variable with the specified type with a default value.
4. `<type> variableName = value;`
    - This will create a variable with the specified type and value.

#### Notes:

- Once assigned a type, a variable cannot be changed to another type. [See Types](#Types)
- Variables of type 'any' cannot be accessed, they need to be initialised first.
- Variables can be reassigned in a local context, once out of it, the variable will be reset to it's previous value.

#### Specifications:

- Must start with a letter.
- Can include numbers and underscores.
- Declaration and initialisation must end with a semicolon.
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
example_1 = 24;  // Valid

// Typed variables
let anyVariable;     // Any type
anyVariable;         // Error, variable has not been initialised.
anyVariable = 23;    // Integer type
anyVaraible = false  // Error

integer example_4 = 24;      // Integer type
double  example_5 = 2,3;     // Double type
boolean example_6 = false;   // Boolean type

// Reassigning variables in a local context
let example_7 = 23;

{
    example_7 = 24; // Valid
}

example_7 => 23
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

function sum(integer a, double b) {

   // Do something

}
```

### Function call

- A function is called with its name and parenthesis that contain the arguments.
- Must end with a semicolon.
- The arguments must be separated by a comma.
- The arguments must be of the same type as the parameters. [See Types](#Types).
- The arguments must be in the same order as the parameters.
- The arguments must be of the same amount as the parameters.
- The result of a function can be assigned to a variable.

```
example();
let result  = sum(5, 10);
integer res = sum(5, 10);
```