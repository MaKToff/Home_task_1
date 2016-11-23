/*
Stack realization.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "stack.h"

// Deletes all elements from the stack
void stack_delete(stack_node **head)
{
	stack_clear(head);
	free(*head);
	return;
}

// Clears all elements from the stack
void stack_clear(stack_node **head)
{
	stack_node *temp = *head;
	
	while (temp != NULL)
	{
		*head = (*head)->next;
		longNum_delete(&temp->value);
		free(temp);
		temp = *head;
	}
	return;
}

// Returns size of the stack
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

// Returns first element and deletes it from the stack
void stack_pop(stack_node **head, number **num)
{
	stack_node *temp = *head;

	if (*head == NULL) 
	{
		error(STACK_IS_EMPTY);
		return;
	}
	*num = temp->value;
	*head = (*head)->next;
	free(temp);
	return;
}

// Adds new element to the stack
void stack_push(stack_node **head, number **data)
{
	stack_node *temp = (stack_node*) malloc(sizeof(stack_node));
	intList_node *tempNum = (*data)->head;

	if (!temp)
	{
		error(NOT_ENOUGHT_MEMORY);
		return;
	}
	temp->value = longNum_init();
	while(tempNum != NULL)
	{
		intList_push(&temp->value->head, tempNum->value);
		tempNum = tempNum->next;
	}
	longNum_reverse(&temp->value);
	temp->value->sign = (*data)->sign;
	temp->next = *head;
	*head = temp;
	return;
}
