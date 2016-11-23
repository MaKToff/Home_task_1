/*
Stack calculator for automatic check.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "stack.h"

// Processes commands and calls necessary functions
int calc_start(stack_node **stack_head, number **value, number **num1, number **num2, number **result, char *trash)
{
	intList_node *temp = NULL;
	intList_node *tempRev = NULL;
	char operation = '\0', first_digit = '\0';
	int i = 0, ok = 0, digit = 0;

	while((first_digit = getchar()) != EOF)
	{
		// Reading a number
		longNum_clear(value);
		operation = '\0';

		if (first_digit == '=')
		{
			scanf("%c", trash);
			if (stack_size(stack_head) < 1)
			{
				printf("Not enough arguments\n");
				return 1;
			}
			temp = (*stack_head)->value->head;
			while(temp != NULL)
			{
				digit = (int)*(&temp->value);
				intList_push(&tempRev, digit);
				temp = temp->next;
			}
			if ((*stack_head)->value->sign == -1)
				printf("-");
			intList_print(&tempRev);
			printf("\n");
		}

		else if (first_digit == '+' || first_digit == '*' || first_digit == '/')
		{
			scanf("%c", trash);
			operation = first_digit;
			if ((int)*trash != 10 && *trash != ' ')
				return 1;
		}

		else
		{
			longNum_read(value, first_digit, &ok);
			
			if (intList_size(&(*value)->head) == 0)
			{
				if (first_digit == '-')
					operation = first_digit;
				else
					return 1;
			}

			else if ((*value)->sign == 0xDEAD)
			{
				printf("Unknown command\n");
				return 1;
			}

			else
				stack_push(stack_head, value);
		}

		// Executes necessary operation
		if (operation != '\0')
		{
			if (stack_size(stack_head) < 2)
			{
				printf("Not enough arguments\n");
				return 1;
			}
			longNum_delete(num1);
			longNum_delete(num2);
			longNum_clear(result);

			stack_pop(stack_head, num1);
			stack_pop(stack_head, num2);

			// Choice the appropriate command
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
						printf("Division by zero\n");
						return 1;
					}
					longNum_divide(num1, num2, result);
				}
				if ((*num1)->sign != (*num2)->sign)
					(*result)->sign *= (-1);
				else (*result)->sign = 1;
			}
			longNum_reverse(result);
			stack_push(stack_head, result);
		}
	}

	printf("[");
	while (stack_size(stack_head) != 0)
	{
		longNum_delete(value);
		stack_pop(stack_head, value);
		if ((*value)->sign == -1)
			printf("-");
		longNum_reverse(value);
		intList_print(&(*value)->head);
		if (stack_size(stack_head) != 0)
			printf("; ");
	}
	printf("]");

	*trash = '\12';
	return 0;
}

int main(void)
{
	stack_node *stack_head = NULL;
	number *value = longNum_init();
	number *num1 = longNum_init();
	number *num2 = longNum_init();
	number *result = longNum_init();
	char trash = '\12';
	int returnCode = 0;

	returnCode = calc_start(&stack_head, &value, &num1, &num2, &result, &trash);

	// Deleting all numbers
	longNum_delete(&value);
	longNum_delete(&num1);
	longNum_delete(&num2);
	longNum_delete(&result);
	stack_delete(&stack_head);
	return returnCode;
}
