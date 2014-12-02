/*
Описание всех типов ошибок, которые могут встретиться в проекте
================================================================
Description of all types of errors that can occur in the project

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include "Errors.h"

//prints message about error
void error(int value)
{
	switch(value)
	{
		case LIST_IS_EMPTY:
			printf("== Incorrect operation: list is empty.\n");
			break;

		case NOT_ENOUGHT_MEMORY:
			printf("== ERROR: not enought memory.\n");
			break;

		case INCORRECT_ARGUMENT:
			printf("== ERROR: incorrect argument.\n");
			break;

		case INCORRECT_EXPRESSION:
			printf("== ERROR: incorrect expression.\n");
			break;

		case UNKNOWN_COMMAND:
			printf("== ERROR: unknown command.\n");
			break;

		case STACK_IS_EMPTY:
			printf("== ERROR: stack is empty.\n");
			break;

		case DIVISION_BY_ZERO:
			printf("== ERROR: integer division by zero.\n");
			break;
	}
	return;
}
