/*
Длинная арифметика
===================
Long arithmetics

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "Math.h"

//deletes all numbers
void arithm_delete_all_numbers(number **num1, number **num2, number **result)
{
	longNum_delete(num1);
	longNum_delete(num2);
	longNum_delete(result);
	return;
}

//reads expression and calls the appropriate command
void arithm_start(number **num1, number **num2, number **result)
{
	char space, operation;
	int i = 0, ok = 1;

	//reading the expression
	longNum_read(num1, &ok);
	if (!(*num1)->sign) 
		return; //an error occurred
	if (!ok)
	{
		arithm_delete_all_numbers(num1, num2, result);
		exit(0);
	}
	scanf("%c", &operation);
	scanf("%c", &space);
	longNum_read(num2, &ok);
	if (!(*num2)->sign)
		return; //an error occurred
	if (!ok)
	{
		arithm_delete_all_numbers(num1, num2, result);
		exit(0);
	}

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

		else 
		{
			error(UNKNOWN_COMMAND);
			return;
		}

		if ((*num1)->sign != (*num2)->sign) 
			(*result)->sign *= (-1);
		else (*result)->sign = 1;
	}
	
	//printing the answer
	printf("===\n");
	if ((*result)->sign == -1) 
		printf("-");
	intList_print(&(*result)->head);
	return;
}

//prints useful information for user
void arithm_help()
{
	printf("LONG ARITHMETICS\n\n");
	printf("This program can compute the value of expression for the two operands.\n");
	printf("It supports four operations:\n\n");
	printf("addition (+)\nsubtraction (-)\nmultiplication (*)\ndivision (/)\n\n");
	printf("You may enter expressions as through the spaces: a + b\n");
	printf("well as through the newlines:\n\n");
	printf("a\n+\nb\n\n");
	printf("Enter '#' for quit");
}

int main(void)
{
	number *num1 = longNum_init();
	number *num2 = longNum_init();
	number *result = longNum_init();
	arithm_help();
	while(1)
	{
		printf("\n\n\n________________________________\n");
		printf("Enter the arithmetic expression:\n\n");
		
		arithm_start(&num1, &num2, &result);

		//clearing of data
		longNum_clear(&num1);
		longNum_clear(&num2);
		longNum_clear(&result);
	}
	return 0;
}
