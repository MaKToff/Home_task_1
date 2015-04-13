/*
Реализация односвязного списка
===============================
Realization of linked list
Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include "Linked_list.h"

//deletes given intList
void intList_delete(intList_node **head)
{
	intList_clear_all(head);
	free(*head);
	return;
}

//clears all elements from list
void intList_clear_all(intList_node **head)
{
	intList_node *temp = *head;

	while (temp != NULL)
	{
		*head = (*head)->next;
		free(temp);
		temp = *head;
	}
	return;
}

//clears first element with given value
void intList_clear_first(intList_node **head, int data)
{
	intList_node *temp = *head;
	intList_node *previous = NULL;
	int ok = 0;
	
	while (temp != NULL)
	{
		if (temp->value == data)
		{
			if (temp == *head) *head = (*head)->next;
			else previous->next = temp->next;
			ok = 1;
			break;
		}
		previous = temp;
		temp = temp->next;
		if (ok) 
			break;
	}
	free(temp);
	return;
}

//returns size of list
int intList_size(intList_node **head)
{
	intList_node *temp = *head;
	int current = 0;
	
	while (temp != NULL)
	{
		temp = temp->next;
		current++;
	}
	return current;
}

//prints all elements of list
void intList_print(intList_node **head)
{
	intList_node *temp = *head;
	
	if (temp == NULL) 
	{
		error(LIST_IS_EMPTY);
		return;
	}
	while (temp != NULL)
	{
		printf("%d", temp->value);
		temp = temp->next;
	}
	return;
}

//adds new element at the head of list
void intList_push(intList_node **head, int data)
{
	intList_node *temp = (intList_node*) malloc(sizeof(intList_node));
	
	if (!temp) 
	{
		error(NOT_ENOUGHT_MEMORY);
		return;
	}
	temp->value = data;
	temp->next = *head;
	*head = temp;
	return;
}
