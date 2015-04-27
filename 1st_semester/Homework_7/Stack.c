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

//deletes all elements from stack
void stack_delete(stack_node **head)
{
	stack_clear(head);
	free(*head);
	return;
}

//clears all elements from stack
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

//adds new element to stack
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
