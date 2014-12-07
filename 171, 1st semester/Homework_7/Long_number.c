/*
Функции для длинных числел
==========================
Functions for long numbers

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "Long_number.h"

//executes the initial declaration variable of type "number"
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

//clears given number
void longNum_clear(number **num)
{
	intList_delete(&(*num)->head);
	(*num)->sign = 1;
	return;
}

//deletes given number
void longNum_delete(number **num)
{
	longNum_clear(num);
	free(*num);
	return;
}

//reads digits of number 
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
		scanf("%c", &digit);
		if (digit < ('0') || digit > ('9'))
		{
			if ((int)digit == 10 || digit == ' ')
			{
				if ((int)digit == 10)
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

//deletes leading zeroes in number
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

//reverses all digits in number
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
