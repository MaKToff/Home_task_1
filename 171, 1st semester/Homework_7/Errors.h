/*
Объявление всех типов ошибок, которые могут встретиться в проекте
=================================================================
Declaration of all types of errors that can occur in the project

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
	DIVISION_BY_ZERO = 7
} errors;

//prints message about error
void error(int value);
