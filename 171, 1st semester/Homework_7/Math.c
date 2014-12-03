/*
Реализация математики для длинных числел
===========================================
Realization of mathematics for long numbers

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include "Math.h"

//executes the addition of two numbers
void longNum_sum(number **num1, number **num2, number **result)
{
	intList_node *temp1 = (*num1)->head;
	intList_node *temp2 = (*num2)->head;
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
		intList_push_front(&(*result)->head, &(*result)->tail, current % 10);
		current /= 10;
	}
	if (current) 
		intList_push_front(&(*result)->head, &(*result)->tail, current);
	longNum_delete_leading_zeroes(result);

	//if result is zero
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1)
		(*result)->sign = 0;

	//clearing of data
	intList_delete(&temp1);
	intList_delete(&temp2);
	return;
}

//executes the subtraction of two numbers
void longNum_subtract(number **num1, number **num2, number **result)
{
	intList_node *temp1 = (*num1)->head;
	intList_node *temp2 = (*num2)->head;
	number *tempRes = longNum_init();
	int current = 0, less = 0, first = 1, digit = 0;
	
	(*result)->sign = 1;
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
			intList_push_back(&tempRes->head, &tempRes->tail, current + 10);
			current = -1;
		}
		else
		{
			intList_push_back(&tempRes->head, &tempRes->tail, current);
			current = 0;
		}
	}
	if (current) 
		less = 1;
	if (less)
	{
		current = 0;
		while(tempRes->head != NULL)
		{
			digit = 9 - (int)*(&tempRes->head->value) + current;
			if (first) 
			{
				digit++;
				if (digit == 10)
				{
					digit = 0;
					current = 1;
				}
				else
					current = 0;
				intList_push_front(&(*result)->head, &(*result)->tail, digit);
				first = 0;
			}
			else
			{
				if (digit == 10)
				{
					digit = 0;
					current = 1;
				}
				else
					current = 0;
				intList_push_front(&(*result)->head, &(*result)->tail, digit); 
			}
			tempRes->head = tempRes->head->next;
		}
		(*result)->sign = -1;
	}
	else
	{
		while(tempRes->head != NULL)
		{
			digit = (int)*(&tempRes->head->value);
			intList_push_front(&(*result)->head, &(*result)->tail, digit);
			tempRes->head = tempRes->head->next;
		}
	}
	longNum_delete_leading_zeroes(result);

	//if result is zero
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1) 
		(*result)->sign = 0;

	//clearing of data
	intList_delete(&temp1);
	intList_delete(&temp2);
	longNum_delete(&tempRes);
	return;
}

//executes the multiplication of two numbers
void longNum_multiply(number **num1, number **num2, number **result)
{
	intList_node *temp1Mult = (*num1)->head;
	intList_node *temp2Mult = (*num2)->head;
	number *temp1Sum = longNum_init();
	number *temp2Sum = longNum_init();
	number *tempResSum = longNum_init();
	int digit = 0, less = 0, current = 0, i = 0, counter = 0;

	//if the first number have less digits than the second
	if (intList_size(&(*num1)->head) < intList_size(&(*num2)->head))
	{
		temp1Mult = (*num2)->head;
		temp2Mult = (*num1)->head;
		less = 1;
	}

	while(temp2Mult != NULL)
	{
		digit = temp2Mult->value;
		if (!less) 
			temp1Mult = (*num1)->head;
		else
			temp1Mult = (*num2)->head;
		current = 0;
		intList_clear_all(&temp1Sum->head);

		while(temp1Mult != NULL)
		{
			current += digit * temp1Mult->value;
			intList_push_back(&temp1Sum->head, &temp1Sum->tail, current % 10);
			current /= 10;
			temp1Mult = temp1Mult->next;
		}
		if (current)
			intList_push_back(&temp1Sum->head, &temp1Sum->tail, current);
		for (i = 0; i < counter; ++i)
			intList_push_front(&temp1Sum->head, &temp1Sum->tail, 0);

		intList_clear_all(&tempResSum->head);
		longNum_sum(&temp1Sum, &temp2Sum, &tempResSum);
		intList_clear_all(&temp2Sum->head);

		while(tempResSum->head != NULL)
		{
			digit = (int)*(&tempResSum->head->value);
			intList_push_front(&temp2Sum->head, &temp2Sum->tail, digit);
			tempResSum->head = tempResSum->head->next;
		}
		counter++;
		temp2Mult = temp2Mult->next;
	}
	while(temp2Sum->head != NULL)
	{
		digit = (int)*(&temp2Sum->head->value);
		intList_push_front(&(*result)->head, &(*result)->tail, digit);
		temp2Sum->head = temp2Sum->head->next;
	}
	longNum_delete_leading_zeroes(result);

	//if result is zero
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1)
		(*result)->sign = 0;

	//clearing of data
	intList_delete(&temp1Mult);
	intList_delete(&temp2Mult);
	longNum_delete(&temp1Sum);
	longNum_delete(&temp2Sum);
	longNum_delete(&tempResSum);
	return;
}

//executes the integer division of two numbers
void longNum_divide(number **num1, number **num2, number **result)
{
	number *temp1Div = longNum_reverse(num1);
	number *temp1Sub = longNum_init();
	number *temp1ReverseSub = longNum_init();
	number *temp2Sub = longNum_init();
	number *tempResSub = longNum_init();
	int digit = 0, tempDigit = 0, less = 0, current = 0;

	temp2Sub->head = (*num2)->head;
	while(temp1Div->head != NULL)
	{
		current = 0;
		digit = (int)temp1Div->head->value;
		intList_push_back(&temp1Sub->head, &temp1Sub->tail, digit);

		//while we can subtract
		while(1)
		{
			intList_clear_all(&tempResSub->head);
			temp1ReverseSub = longNum_reverse(&temp1Sub);
			longNum_subtract(&temp1ReverseSub, &temp2Sub, &tempResSub);

			if (tempResSub->sign < 0)
				break;

			intList_clear_all(&temp1Sub->head);
			while(tempResSub->head != NULL)
			{
				tempDigit = (int)*(&tempResSub->head->value);
				intList_push_back(&temp1Sub->head, &temp1Sub->tail, tempDigit);
				tempResSub->head = tempResSub->head->next;
			}
			current++;
		}
		temp1Div->head = temp1Div->head->next;
		intList_push_back(&(*result)->head, &(*result)->tail, current);
	}
	longNum_delete_leading_zeroes(result);

	//if result is zero
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1)
			(*result)->sign = 0;

	//clearing of data
	longNum_delete(&temp1Div);
	longNum_delete(&temp1Sub);
	longNum_delete(&temp1ReverseSub);
	longNum_delete(&tempResSub);
	return;
}
