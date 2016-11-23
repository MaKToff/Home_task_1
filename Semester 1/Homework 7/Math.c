/*
Realization of mathematics for long numbers.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include "math.h"

// Executes addition of two numbers
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
		intList_push(&(*result)->head, current % 10);
		current /= 10;
	}
	if (current) 
		intList_push(&(*result)->head, current);
	longNum_delete_leading_zeroes(result);

	// If the result is equal to zero
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1)
		(*result)->sign = 0;
	return;
}

// Executes subtraction of two numbers
void longNum_subtract(number **num1, number **num2, number **result)
{
	intList_node *temp = NULL;
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
			intList_push(&tempRes->head, current + 10);
			current = -1;
		}
		else
		{
			intList_push(&tempRes->head, current);
			current = 0;
		}
	}
	longNum_reverse(&tempRes);
	if (current)
		less = 1;
	if (less)
	{
		current = 0;
		temp = tempRes->head;
		while(temp != NULL)
		{
			digit = 9 - (int)*(&temp->value) + current;
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
				intList_push(&(*result)->head, digit);
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
				intList_push(&(*result)->head, digit); 
			}
			temp = temp->next;
		}
		(*result)->sign = -1;
	}
	else
	{
		temp = tempRes->head;
		while(temp != NULL)
		{
			digit = (int)*(&temp->value);
			intList_push(&(*result)->head, digit);
			temp = temp->next;
		}
	}
	longNum_delete_leading_zeroes(result);

	// If the result is equal to zero
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1) 
		(*result)->sign = 0;
	
	// Clearing the data
	longNum_delete(&tempRes);
	return;
}

// Executes multiplication of two numbers
void longNum_multiply(number **num1, number **num2, number **result)
{
	intList_node *temp = NULL;
	intList_node *temp1Mult = (*num1)->head;
	intList_node *temp2Mult = (*num2)->head;
	number *temp1Sum = longNum_init();
	number *temp2Sum = longNum_init();
	number *tempResSum = longNum_init();
	int digit = 0, less = 0, current = 0, i = 0, counter = 0;

	// If the first number have less digits than the second
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
			intList_push(&temp1Sum->head, current % 10);
			current /= 10;
			temp1Mult = temp1Mult->next;
		}
		if (current)
			intList_push(&temp1Sum->head, current);
		longNum_reverse(&temp1Sum);
		for (i = 0; i < counter; ++i)
			intList_push(&temp1Sum->head, 0);

		intList_clear_all(&tempResSum->head);
		longNum_sum(&temp1Sum, &temp2Sum, &tempResSum);
		intList_clear_all(&temp2Sum->head);

		temp = tempResSum->head;
		while(temp != NULL)
		{
			digit = (int)*(&temp->value);
			intList_push(&temp2Sum->head, digit);
			temp = temp->next;
		}
		counter++;
		temp2Mult = temp2Mult->next;
	}
	temp = temp2Sum->head;
	while(temp != NULL)
	{
		digit = (int)*(&temp->value);
		intList_push(&(*result)->head, digit);
		temp = temp->next;
	}
	longNum_delete_leading_zeroes(result);

	// If the result is equal to zero
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1)
		(*result)->sign = 0;

	// Clearing the data
	longNum_delete(&temp1Sum);
	longNum_delete(&temp2Sum);
	longNum_delete(&tempResSum);
	return;
}

// Executes integer division of two numbers
void longNum_divide(number **num1, number **num2, number **result)
{
	intList_node *temp = NULL;
	intList_node *temp1 = NULL;
	number *temp1Div = *num1;
	number *temp1Sub = longNum_init();
	number *temp2Sub = *num2;
	number *tempResSub = longNum_init();
	int digit = 0, tempDigit = 0, less = 0, current = 0, mod0 = 0;

	longNum_reverse(&temp1Div);
	temp1 = temp1Div->head;
	while(temp1 != NULL)
	{
		current = 0;
		digit = (int)temp1->value;
		intList_push(&temp1Sub->head, digit);

		// While we can subtract
		while(1)
		{
			longNum_clear(&tempResSub);
			longNum_subtract(&temp1Sub, &temp2Sub, &tempResSub);

			if (tempResSub->sign < 0)
				break;

			longNum_clear(&temp1Sub);
			temp = tempResSub->head;
			while(temp != NULL)
			{
				tempDigit = (int)*(&temp->value);
				intList_push(&temp1Sub->head, tempDigit);
				temp = temp->next;
			}
			current++;
		}
		temp1 = temp1->next;
		intList_push(&(*result)->head, current);
	}
	if (intList_size(&temp1Sub->head) == 1 && temp1Sub->head->value == 0)
		mod0 = 1;
	if ((*num1)->sign == -1 && !mod0)
	{
		longNum_clear(&temp1Sub);
		temp = (*result)->head;
		while(temp != NULL)
		{
			tempDigit = (int)*(&temp->value);
			intList_push(&temp1Sub->head, tempDigit);
			temp = temp->next;
		}
		temp2Sub = longNum_init();
		intList_push(&temp2Sub->head, 1);
		longNum_clear(result);
		longNum_sum(&temp1Sub, &temp2Sub, result);
	}
	longNum_reverse(&(*result));
	longNum_delete_leading_zeroes(result);

	// If the result is equal to zero
	if ((((*result)->head)->value == 0) && intList_size(&(*result)->head) == 1)
			(*result)->sign = 0;

	// Clearing the data
	longNum_delete(&temp1Sub);
	longNum_delete(&tempResSub);
	return;
}
