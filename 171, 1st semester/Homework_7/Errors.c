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
			printf("== Not enought memory.\n");
			break;

		case INCORRECT_ARGUMENT:
			printf("== Incorrect argument.\n");
			break;

		case INCORRECT_EXPRESSION:
			printf("== Incorrect expression.\n");
			break;

		case UNKNOWN_COMMAND:
			printf("== Unknown command.\n");
			break;

		case STACK_IS_EMPTY:
			printf("== Incorrect operation: stack is empty.\n");
			break;
	}
	return;
}