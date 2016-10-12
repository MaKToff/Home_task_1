/*
Declaration of all errors types that can occur in the project.

Author: Mikhail Kita, group 171
*/

enum errors
{
	LIST_IS_EMPTY = 1,
	NOT_ENOUGHT_MEMORY = 2,
	INCORRECT_ARGUMENT = 3,
	INCORRECT_EXPRESSION = 4,
	UNKNOWN_COMMAND = 5,
	STACK_IS_EMPTY = 6,
	DIVISION_BY_ZERO = 7,
	STACK_IS_TOO_SMALL = 8
} errors;

// Prints message about error
void error(int value);
