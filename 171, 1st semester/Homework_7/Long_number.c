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
	temp->tail = NULL;
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
	intList_delete(&(*num)->head);
	free(*num);
	return;
}

//reads digits of number 
void longNum_read(number **num, int *ok)
{
	char digit;

	scanf("%c", &digit);
	if (digit == '-') 
		(*num)->sign = -1;
	else if (digit == '#')
	{
		*ok = 0; //user wants to close application
		return; 
	}
	else if (digit < '0' || digit > '9') 
	{
		error(INCORRECT_ARGUMENT);
		while((int)digit != 10) 
			scanf("%c", &digit);
		(*num)->sign = 0;
		return;
	}
	else 
		intList_push_front(&(*num)->head, &(*num)->tail, (int)digit - (int)('0'));
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
				(*num)->sign = 0;
				return;
			}
		}
		intList_push_front(&(*num)->head, &(*num)->tail, (int)digit - (int)('0'));
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
			intList_push_front(&(*num)->head, &(*num)->tail, 0);
			break;
		}
	}
	return;
}

//reverses all digits in number
number* longNum_reverse(number **num)
{
	number *result = longNum_init();
	intList_node *tempNum = (*num)->head;

	result->sign = (*num)->sign;
	while(tempNum != NULL)
	{
		intList_push_front(&result->head, &result->tail, tempNum->value);
		tempNum = tempNum->next;
	}
	return result;
}