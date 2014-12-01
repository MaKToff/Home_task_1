/*
Стековый калькулятор
====================
Stack calculator

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "Stack.h"

//deletes all numbers
void calc_delete_all_numbers(number **value, number **first, number **second, number **result)
{
	longNum_delete(value);
	longNum_delete(first);
	longNum_delete(second);
	longNum_delete(result);
	return;
}

//processes the commands and calls the necessary functions
void calc_start(stack_node **stack_head)
{
	number *value = longNum_init();
	number *first = longNum_init();
	number *second = longNum_init();
	number *result = longNum_init();
	char space, operation;
	int i = 0, ok = 1;

	while(1)
	{	
		//reading number
		longNum_clear(&value);
		longNum_read(&value, &ok);
		if (!value->sign)
		{
			calc_delete_all_numbers(&value, &first, &second, &result);
			return; //an error occurred
		}
		if (!ok)
		{
			calc_delete_all_numbers(&value, &first, &second, &result);
			exit(0);
		}
		stack_push(stack_head, &value);

		//executes necessary operation
		if (stack_size(stack_head) == 2)
		{
			scanf("%c", &operation);
			scanf("%c", &space);
			stack_pop(stack_head, &second);
			stack_pop(stack_head, &first);
			result = longNum_init();

			//choice of appropriate command
			if (operation == '+' || operation == '-')
			{
				if (first->sign == second->sign)
				{
					if (operation == '+') 
						longNum_sum(&first, &second, &result);
					else 
						longNum_subtract(&first, &second, &result);
				}
				else 
				{
					if (operation == '-') 
						longNum_sum(&first, &second, &result);
					else 
						longNum_subtract(&first, &second, &result);
				}
				result->sign *= first->sign;
			}
			else 
			{
				if (operation == '*')
					longNum_multiply(&first, &second, &result);
				
				else if (operation == '/')
					longNum_divide(&first, &second, &result);

				else
				{
					error(UNKNOWN_COMMAND);
					calc_delete_all_numbers(&value, &first, &second, &result);
					return;
				}
				if (first->sign != first->sign)
					result->sign *= (-1);
				else result->sign = 1;
			}
			result = longNum_reverse(&result);
			stack_push(stack_head, &result);
			
			//if this is newline
			if ((int)space == 10)
				break;
		}
	}
	if (stack_size(stack_head) != 1)
		error(INCORRECT_EXPRESSION);
	else
	{
		printf("=== ");
		stack_pop(stack_head, &value);
		if (value->sign == -1)
			printf("-");
		intList_print(&longNum_reverse(&value)->head);
	}
	calc_delete_all_numbers(&value, &first, &second, &result);
	return;
}

//prints useful information for user
void calc_help()
{
	printf("STACK CALCULATOR\n\n");
	printf("This is stack calculator for long numbers.\n");
	printf("It supports four operations:\n\n");
	printf("addition (+)\nsubtraction (-)\nmultiplication (*)\ndivision (/)\n\n");
	printf("You should enter expressions through the spaces\n");
	printf("in reverse polish notation (RPN):\n\n");
	printf("a b + c *     ===     (a + b) * c\n\n\n");
	printf("Enter '#' for quit");
}

int main(void)
{
	stack_node *stack_head = NULL;
	calc_help();
	while(1)
	{
		printf("\n\n\n________________________________\n");
		printf("Enter the arithmetic expression:\n\n");
		calc_start(&stack_head);
		stack_clear_all(&stack_head);
	}
	return 0;
}