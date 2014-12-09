/*
Стековый калькулятор
====================
Stack calculator

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "Stack.h"

//processes the commands and calls the necessary functions
void calc_start(stack_node **stack_head, number **value, number **num1, number **num2, number **result, int *finished)
{
	char trash = '\0', operation = '\0', first_digit = '\0';
	int i = 0, ok = 0;

	while(1)
	{	
		//reading number
		longNum_clear(value);
		operation = '\0';
		scanf("%c", &first_digit);

		if (first_digit == '#') //user wants to close application
		{
			*finished = 1;
			return;
		}

		else if (first_digit == '+' || first_digit == '*' || first_digit == '/')
		{
			scanf("%c", &trash);
			operation = first_digit;
		}

		else
		{
			longNum_read(value, first_digit, &ok);
			
			if (intList_size(&(*value)->head) == 0)
			{
				if (first_digit == '-')
					operation = first_digit;
				else
				{
					error(UNKNOWN_COMMAND);
					printf("== Expected: +, -, * or /. Found: %c\n", first_digit);
					return;
				}
			}

			else if ((*value)->sign == 0xDEAD)
			{
				error(INCORRECT_ARGUMENT);
				return;
			}

			else
				stack_push(stack_head, value);
		}

		//executes necessary operation
		if (operation != '\0')
		{
			if (stack_size(stack_head) < 2)
			{
				error(STACK_IS_TOO_SMALL);
				while((int)trash != 10) 
					scanf("%c", &trash);
				return;
			}
			longNum_delete(num1);
			longNum_delete(num2);
			longNum_clear(result);

			stack_pop(stack_head, num2);
			stack_pop(stack_head, num1);

			//choice of appropriate command
			if (operation == '+' || operation == '-')
			{
				if ((*num1)->sign == (*num2)->sign)
				{
					if (operation == '+') 
						longNum_sum(num1, num2, result);
					else 
						longNum_subtract(num1, num2, result);
				}
				else 
				{
					if (operation == '-') 
						longNum_sum(num1, num2, result);
					else 
						longNum_subtract(num1, num2, result);
				}
				(*result)->sign *= (*num1)->sign;
			}
			else 
			{
				if (operation == '*')
					longNum_multiply(num1, num2, result);
				
				else if (operation == '/')
				{
					if (intList_size(&(*num2)->head) == 1 && (*num2)->head->value == 0)
					{
						error(DIVISION_BY_ZERO);
						return;
					}
					longNum_divide(num1, num2, result);
				}
				if ((*num1)->sign != (*num2)->sign)
					(*result)->sign *= (-1);
				else (*result)->sign = 1;
			}
			longNum_reverse(result);
			stack_push(stack_head, result);
			
			if ((int)trash == 10 || ok) //if newline was introduced
				break;
		}
		if (ok) //the end of input found
			break;
	}
	if (stack_size(stack_head) != 1)
		error(INCORRECT_EXPRESSION);
	else
	{
		longNum_delete(value);
		printf("=== ");
		stack_pop(stack_head, value);
		if ((*value)->sign == -1)
			printf("-");
		longNum_reverse(value);
		intList_print(&(*value)->head);
	}
	return;
}

//prints useful information for user
void calc_help()
{
	printf("STACK CALCULATOR\n\n");
	printf("This is stack calculator for long numbers.\n");
	printf("It supports four operations:\n\n");
	printf("addition (+)\nsubtraction (-)\nmultiplication (*)\ninteger  division (/)\n\n");
	printf("You should enter expressions through the spaces\n");
	printf("in reverse polish notation (RPN):\n\n");
	printf("a b + c *     ===     (a + b) * c\n\n\n");
	printf("Enter '#' for quit");
}

int main(void)
{
	stack_node *stack_head = NULL;
	number *value = longNum_init();
	number *num1 = longNum_init();
	number *num2 = longNum_init();
	number *result = longNum_init();
	int finished = 0;

	calc_help();
	while(1)
	{
		printf("\n\n\n________________________________\n");
		printf("Enter the arithmetic expression:\n\n");

		calc_start(&stack_head, &value, &num1, &num2, &result, &finished);

		if (finished)
			break;

		//clearing of data
		longNum_clear(&value);
		longNum_clear(&result);
		stack_clear(&stack_head);
	}

	//deleting all numbers
	longNum_delete(&value);
	longNum_delete(&num1);
	longNum_delete(&num2);
	longNum_delete(&result);
	stack_delete(&stack_head);
	return 0;
}
