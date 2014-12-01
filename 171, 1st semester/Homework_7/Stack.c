/*
Реализация стека
====================
Realization of stack

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "Stack.h"

//clears all elements from satck
void stack_clear_all(stack_node **head)
{
	stack_node *temp = *head;
	
	while (temp != NULL)
	{
		*head = (*head)->next;
		longNum_clear(&temp->value);
		temp = *head;
	}
	return;
}

//returns size of stack
int stack_size(stack_node **head)
{
	stack_node *temp = *head;
	int current = 0;
	
	while (temp != NULL)
	{
		temp = temp->next;
		current++;
	}
	return current;
}

//returns first element and deletes it from stack
void stack_pop(stack_node **head, number **num)
{
	if (*head == NULL) 
	{
		error(STACK_IS_EMPTY);
		return;
	}
	*num = (*head)->value;
	*head = (*head)->next;
	return;
}

//adds new element to stack
void stack_push(stack_node **head, number **data)
{
	stack_node *temp = (stack_node*) malloc(sizeof(stack_node));
	
	if (!temp)
	{
		error(NOT_ENOUGHT_MEMORY);
		return;
	}
	temp->value = longNum_init();
	while((*data)->head != NULL)
	{
		intList_push_back(&temp->value->head, &temp->value->tail, (*data)->head->value);
		(*data)->head = (*data)->head->next;
	}
	temp->value->sign = (*data)->sign;
	temp->next = *head;
	*head = temp;
	return;
}