/*
Сложение и вычитание длинных чисел
========================================
Addition and subtraction of long numbers

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "Linked_list.h"

typedef struct number
{
	int sign;
	node *head;
} number;

//executes the initial declaration variable of type "number"
number* number_init()
{
	number *temp = (number*)malloc(sizeof(number));
	if (temp == NULL) 
	{
		error(NOT_ENOUGHT_MEMORY);
		return NULL;
	}
	temp->sign = 1;
	temp->head = NULL;
	return temp;
}

//deletes all digits in all numbers
void clear_all(number **num1, number **num2, number **result)
{
	del_all(&((*num1)->head));
	del_all(&((*num2)->head));
	del_all(&((*result)->head));
	return;
}

//reads digits of number 
number* read(int *ok)
{
	char digit;
	number *num = number_init();

	scanf("%c", &digit);
	if (digit == '-') 
		num->sign = -1;
	else if (digit == '#')
	{
		*ok = 0; //user wants to close application
		return num; 
	}
	else if (digit < '0' || digit > '9') 
	{
		error(INCORRECT_ARGUMENT);
		while((int)digit != 10) 
			scanf("%c", &digit);
		num->sign = 0;
		return num;
	}
	else 
		push_front(&num->head, (int)digit - (int)('0'));
	while (1)
	{
		scanf("%c", &digit);
		if (digit < ('0') || digit > ('9'))
		{
			if ((int)digit == 10 || digit == ' ') 
				break;
			else 
			{
				error(INCORRECT_ARGUMENT);
				while((int)digit != 10) 
					scanf("%c", &digit);
				num->sign = 0;
				return num;
			}
		}
		push_front(&num->head, (int)digit - (int)('0'));
	}
	return num;
}

//deletes leading zeroes in number
void del_leading_zeroes(number **num)
{
	while(((*num)->head)->value == 0) 
	{
		del_first(&(*num)->head, 0);
		if ((*num)->head == NULL) 
		{
			push_front(&(*num)->head, 0);
			break;
		}
	}
	return;
}

//executes the addition of two numbers
void sum(number **num1, number **num2, number **result)
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
		push_front(&(*result)->head, current % 10);
		current /= 10;
	}
	if (current) 
		push_front(&(*result)->head, current);
	del_leading_zeroes(result);
	if ((((*result)->head)->value == 0) && size(&(*result)->head) == 1)
		(*result)->sign = 0;
	return;
}

//executes the subtraction of two numbers
void subtract(number **num1, number **num2, number **result)
{
	node *temp1 = (*num1)->head;
	node *temp2 = (*num2)->head;
	node *tempResHead = NULL;
	node *tempResTail = NULL;
	int current = 0, less = 0, first = 1, zero = 0;
	
	(*result)->sign = 1;
	if (temp1->value == 0 && size(&temp1) == 1) zero = 1;
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
			push_back(&tempResHead, &tempResTail, current + 10);
			current = -1;
		}
		else
		{
			push_back(&tempResHead, &tempResTail, current);
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
				push_front(&(*result)->head, 10 - tempResHead->value);
				first = 0;
			}
			else push_front(&(*result)->head, 9 - tempResHead->value); 
			tempResHead = tempResHead->next;
		}
		(*result)->sign = -1;
	}
	else if (!zero) 
	{
		while(tempResHead != NULL)
		{
			push_front(&(*result)->head, tempResHead->value);
			tempResHead = tempResHead->next;
		}
	}
	else //if subtrahend is zero
	{
		temp2 = (*num2)->head;
		while(temp2 != NULL)
		{
			push_front(&(*result)->head, temp2->value);
			temp2 = temp2->next;
		}
		(*result)->sign = -1;
	}
	del_leading_zeroes(result);
	if ((((*result)->head)->value == 0) && size(&(*result)->head) == 1) 
		(*result)->sign = 0; 
	return;
}

//reads expression and calls the appropriate command
void start()
{
	number *num1 = number_init();
	number *num2 = number_init();
	number *result = number_init();
	char space, operation;
	int i = 0, ok = 1;
	
	printf("\n\n\n________________________________\n");
	printf("Enter the arithmetic expression:\n\n");

	//reading the expression
	num1 = read(&ok);
	if (!num1->sign) 
		return; //an error occurred
	if (!ok)
	{
		clear_all;
		exit(0);
	}
	scanf("%c", &operation);
	scanf("%c", &space);
	if (operation != '+' && operation != '-') 
	{
		error(UNKNOWN_COMMAND);
		return;
	}
	num2 = read(&ok);
	if (!num2->sign)
		return; //an error occurred
	if (!ok)
	{
		clear_all;
		exit(0);
	}

	//choice of appropriate command
	if (num1->sign == num2->sign)
	{
		if (operation == '+') 
			sum(&num1, &num2, &result);
		else 
			subtract(&num1, &num2, &result);
	}
	else 
	{
		if (operation == '-') 
			sum(&num1, &num2, &result);
		else 
			subtract(&num1, &num2, &result);
	}
	result->sign *= num1->sign;
	
	//printing the answer
	printf("===\n");
	if (result->sign == -1) 
		printf("-");
	print(&result->head);
	
	//clearing of data
	clear_all(&num1, &num2, &result);
}

//prints useful information for user
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
		start();
	return 0;
}
