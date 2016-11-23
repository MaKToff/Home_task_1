/*
Addition and subtraction of long numbers.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "linked_list.h"

typedef struct number
{
	int sign;
	node *head;
} number;

// Executes the initial declaration of a number variable
number* longNum_init()
{
	number *temp = (number*)malloc(sizeof(number));
	if (temp == NULL) 
	{
		intList_error(NOT_ENOUGHT_MEMORY);
		return NULL;
	}
	temp->sign = 1;
	temp->head = NULL;
	return temp;
}

// Deletes all digits in all numbers
void longNum_clear_all(number **num1, number **num2, number **result)
{
	intList_delete_all(&((*num1)->head));
	intList_delete_all(&((*num2)->head));
	intList_delete_all(&((*result)->head));
	return;
}

// Reads digits of a number 
number* longNum_read(int *ok)
{
	char digit;
	number *num = longNum_init();

	scanf("%c", &digit);
	if (digit == '-') 
		num->sign = -1;
	else if (digit == '#')
	{
		*ok = 0; // User wants to close the application
		return num; 
	}
	else if (digit < '0' || digit > '9') 
	{
		intList_error(INCORRECT_ARGUMENT);
		while((int)digit != 10) 
			scanf("%c", &digit);
		num->sign = 0;
		return num;
	}
	else 
		intList_push_front(&num->head, (int)digit - (int)('0'));
	while (1)
	{
		scanf("%c", &digit);
		if (digit < ('0') || digit > ('9'))
		{
			if ((int)digit == 10 || digit == ' ') 
				break;
			else 
			{
				intList_error(INCORRECT_ARGUMENT);
				while((int)digit != 10) 
					scanf("%c", &digit);
				num->sign = 0;
				return num;
			}
		}
		intList_push_front(&num->head, (int)digit - (int)('0'));
	}
	return num;
}

// Deletes leading zeroes in a number
void longNum_delete_leading_zeroes(number **num)
{
	while(((*num)->head)->value == 0) 
	{
		intList_delete_first(&(*num)->head, 0);
		if ((*num)->head == NULL) 
		{
			intList_push_front(&(*num)->head, 0);
			break;
		}
	}
	return;
}

// Executes addition of two numbers
void longNum_sum(number **num1, number **num2, number **result)
{
	node *temp1 = (*num1)->head;
	node *temp2 = (*num2)->head;
	int current = 0;
	
	while(temp1 != NULL || temp2 != NULL)
	{
		if (temp1 != NULL) 
		{
			current += temp1->value;
			temp1 = temp1->next;
		}
		if (temp2 != NULL) 
		{
			current += temp2->value;
			temp2 = temp2->next;
		}
		intList_push_front(&(*result)->head, current % 10);
		current /= 10;
	}
	if (current) 
		intList_push_front(&(*result)->head, current);
	longNum_delete_leading_zeroes(result);
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1)
		(*result)->sign = 0;
	return;
}

// Executes subtraction of two numbers
void longNum_subtract(number **num1, number **num2, number **result)
{
	node *temp1 = (*num1)->head;
	node *temp2 = (*num2)->head;
	node *tempResHead = NULL;
	node *tempResTail = NULL;
	int current = 0, less = 0, first = 1, zero = 0;
	
	(*result)->sign = 1;
	if (temp1->value == 0 && intList_size(&temp1) == 1) zero = 1;
	while(temp1 != NULL || temp2 != NULL)
	{
		if (temp1 == NULL && current) 
			less = 1;
		if (temp1 != NULL) 
		{
			current += temp1->value;
			temp1 = temp1->next;
		}
		if (temp2 != NULL) 
		{
			current -= temp2->value;
			temp2 = temp2->next;
		}

		if (current < 0) 
		{
			intList_push_back(&tempResHead, &tempResTail, current + 10);
			current = -1;
		}
		else
		{
			intList_push_back(&tempResHead, &tempResTail, current);
			current = 0;
		}
	}
	if (current) 
		less = 1;
	if (less & !zero)
	{
		while(tempResHead != NULL)
		{
			if (first) 
			{
				intList_push_front(&(*result)->head, 10 - tempResHead->value);
				first = 0;
			}
			else intList_push_front(&(*result)->head, 9 - tempResHead->value); 
			tempResHead = tempResHead->next;
		}
		(*result)->sign = -1;
	}
	else if (!zero) 
	{
		while(tempResHead != NULL)
		{
			intList_push_front(&(*result)->head, tempResHead->value);
			tempResHead = tempResHead->next;
		}
	}
	else // If the subtrahend is equals to zero
	{
		temp2 = (*num2)->head;
		while(temp2 != NULL)
		{
			intList_push_front(&(*result)->head, temp2->value);
			temp2 = temp2->next;
		}
		(*result)->sign = -1;
	}
	longNum_delete_leading_zeroes(result);
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1) 
		(*result)->sign = 0; 
	return;
}

// Reads the expression and calls the appropriate command
void longNum_start()
{
	number *num1 = longNum_init();
	number *num2 = longNum_init();
	number *result = longNum_init();
	char space, operation;
	int i = 0, ok = 1;
	
	printf("\n\n\n________________________________\n");
	printf("Enter the arithmetic expression:\n\n");

	// Reading the expression
	num1 = longNum_read(&ok);
	if (!num1->sign) 
		return; // An error occurred
	if (!ok)
	{
		longNum_clear_all;
		exit(0);
	}
	scanf("%c", &operation);
	scanf("%c", &space);
	if (operation != '+' && operation != '-') 
	{
		intList_error(UNKNOWN_COMMAND);
		return;
	}
	num2 = longNum_read(&ok);
	if (!num2->sign)
		return; // An error occurred
	if (!ok)
	{
		longNum_clear_all;
		exit(0);
	}

	// Choice of the appropriate command
	if (num1->sign == num2->sign)
	{
		if (operation == '+') 
			longNum_sum(&num1, &num2, &result);
		else 
			longNum_subtract(&num1, &num2, &result);
	}
	else 
	{
		if (operation == '-') 
			longNum_sum(&num1, &num2, &result);
		else 
			longNum_subtract(&num1, &num2, &result);
	}
	result->sign *= num1->sign;
	
	// Printing the answer
	printf("===\n");
	if (result->sign == -1) 
		printf("-");
	intList_print(&result->head);
	
	// Clearing the data
	longNum_clear_all(&num1, &num2, &result);
}

// Prints useful information for user
void help()
{
	printf("CALCULATOR\n\n");
	printf("This is calculator for long numbers.\n");
	printf("It supports only two operations: addition (+) and subtraction (-).\n");
	printf("You may enter expressions as through the spaces\n\n");
	printf("a + b\n\n");
	printf("well as through the newlines\n\n");
	printf("a\n+\nb\n\n");
	printf("Enter '#' for quit");
}

int main(void)
{
	help();
	while(1)
		longNum_start();
	return 0;
}