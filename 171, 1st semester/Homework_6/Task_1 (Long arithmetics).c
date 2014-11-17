/*
Сложение и вычитание длинных чисел
========================================
Addition and subtraction of long numbers

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "Linked_list.h"

int read(node **head)
{
	char digit;
	int sign = 1;
	scanf("%c", &digit);
	if (digit == '-') sign = -1;
	else if (digit == '#') exit(0); 
	else if (digit < '0' || digit > '9') 
	{
		error(INCORRECT_ARGUMENT);
		while((int)digit != 10) scanf("%c", &digit);
		return 0;
	}
	else push_front(head, (int)digit - (int)('0'));
	while (1)
	{
		scanf("%c", &digit);
		if (digit < ('0') || digit > ('9'))
		{
			if ((int)digit == 10 || digit == ' ') break;
			else 
			{
				error(INCORRECT_ARGUMENT);
				while((int)digit != 10) scanf("%c", &digit);
				return 0;
			}
		}
		push_front(head, (int)digit - (int)('0'));
	}
	return sign;
}

void del_leading_zeroes(node **head)
{
	while((*head)->value == 0) 
	{
		del_first(head, 0);
		if (*head == NULL) 
		{
			push_front(head, 0);
			break;
		}
	}
	return;
}

int sum(node *head1, node *head2, node **headRes)
{
	node *temp1 = head1;
	node *temp2 = head2;
	int current = 0, sign = 1;
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
		push_front(headRes, current % 10);
		current /= 10;
	}
	if (current) push_front(headRes, current);
	del_leading_zeroes(headRes);
	if (((*headRes)->value == 0) && size(headRes) == 1) sign = 0; 
	return sign;
}

int subtract(node *head1, node *head2, node **headRes)
{
	node *temp1 = head1;
	node *temp2 = head2;
	node *tempResHead = NULL;
	node *tempResTail = NULL;
	int current = 0, less = 0, sign = 1, first = 1, zero = 0;
	
	if (temp1->value == 0 && size(&temp1) == 1) zero = 1;
	while(temp1 != NULL || temp2 != NULL)
	{
		if (temp1 == NULL && current) less = 1;
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
	if (current) less = 1;
	if (less & !zero)
	{
		while(tempResHead != NULL)
		{
			if (first) 
			{
				push_front(headRes, 10 - tempResHead->value);
				first = 0;
			}
			else push_front(headRes, 9 - tempResHead->value); 
			tempResHead = tempResHead->next;
		}
		sign = -1;
	}
	else if (!zero) 
	{
		while(tempResHead != NULL)
		{
			push_front(headRes, tempResHead->value);
			tempResHead = tempResHead->next;
		}
	}
	else //Если вычитаемое - ноль
	{
		temp2 = head2;
		while(temp2 != NULL)
		{
			push_front(headRes, temp2->value);
			temp2 = temp2->next;
		}
		sign = -1;
	}
	del_leading_zeroes(headRes);
	if (((*headRes)->value == 0) && size(headRes) == 1) sign = 0; 
	return sign;
}

void start()
{
	node* number1 = NULL;
	node* number2 = NULL;
	node* result = NULL;
	char space, operation;
	int signNum1 = 1, signNum2 = 1, signRes = 1, i = 0;
	
	printf("\n\n\n_____________________\n");
	printf("Enter the expression:\n\n");
	signNum1 = read(&number1);
	if (!signNum1) return;
	scanf("%c", &operation);
	scanf("%c", &space);
	if (operation != '+' && operation != '-') 
	{
		error(UNKNOWN_COMMAND);
		return;
	}
	signNum2 = read(&number2);
	if (!signNum2) return;

	if (signNum1 == signNum2)
	{
		if (operation == '+') 
			signRes = signNum1 * sum(number1, number2, &result);
		else 
			signRes = signNum1 * subtract(number1, number2, &result);
	}
	else 
	{
		if (operation == '-') 
			signRes = signNum1 * sum(number1, number2, &result);
		else 
			signRes = signNum1 * subtract(number1, number2, &result);
	}
	printf("===\n");
	if (signRes == -1) printf("-");
	print(&result);
	
	del_all(&number1);
	del_all(&number2);
	del_all(&result);
}

int main(void)
{
	printf("CALCULATOR\n\n");
	printf("Enter '#' for quit");
	while(1)
	{
		start();
	}
	return 0;
}
