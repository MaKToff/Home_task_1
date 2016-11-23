/*
Functions for long numbers.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "long_number.h"

// Executes the initial declaration of a number variable
number* longNum_init()
{
	number *temp = (number*) malloc(sizeof(number));

	if (temp == NULL) 
	{
		error(NOT_ENOUGHT_MEMORY);
		return NULL;
	}
	temp->sign = 1;
	temp->head = NULL;
	return temp;
}

// Clears a given number
void longNum_clear(number **num)
{
	intList_delete(&(*num)->head);
	(*num)->sign = 1;
	return;
}

// Deletes a given number
void longNum_delete(number **num)
{
	longNum_clear(num);
	free(*num);
	return;
}

// Reads digits of a number 
void longNum_read(number **num, char first_digit, int *ok)
{
	char digit;

	digit = first_digit;
	if (digit == '-') 
		(*num)->sign = -1;
	else if (digit < '0' || digit > '9') 
	{
		while((int)digit != 10) 
			scanf("%c", &digit);
		(*num)->sign = 0xDEAD;
		return;
	}
	else 
		intList_push(&(*num)->head, (int)digit - (int)('0'));
	while (1)
	{
		digit = getchar();
		if (digit < ('0') || digit > ('9'))
		{
			if ((int)digit == 10 || digit == ' ' || (int)digit == EOF)
			{
				if ((int)digit == 10 || (int)digit == EOF)
					*ok = 1;
				break;
			}
			else 
			{
				while((int)digit != 10) 
					scanf("%c", &digit);
				(*num)->sign = 0xDEAD;
				return;
			}
		}
		intList_push(&(*num)->head, (int)digit - (int)('0'));
	}
	return;
}

// Deletes leading zeroes in a number
void longNum_delete_leading_zeroes(number **num)
{
	while(((*num)->head)->value == 0) 
	{
		intList_clear_first(&(*num)->head, 0);
		if ((*num)->head == NULL) 
		{
			intList_push(&(*num)->head, 0);
			break;
		}
	}
	return;
}

// Reverses all digits in a number
void longNum_reverse(number **num)
{
	intList_node *result = NULL;
	intList_node *temp = (*num)->head;
	int sign = (*num)->sign;

	while(temp != NULL)
	{
		intList_push(&result, temp->value);
		temp = temp->next;
	}
	longNum_clear(num);
	(*num)->head = result;
	(*num)->sign = sign;
	return;
}
