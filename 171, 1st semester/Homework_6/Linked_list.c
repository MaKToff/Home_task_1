/*
Реализация односвязного списка
===============================
Linked list

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "Linked_list.h"

//prints message about error
void intList_error(int value)
{
	switch(value)
	{
		case UNKNOWN_COMMAND:
			printf("== Unknown command.\n");
			break;

		case LIST_IS_EMPTY:
			printf("== Incorrect operation: list is empty.\n");
			break;

		case INCORRECT_ARGUMENT:
			printf("== Incorrect argument.\n");
			break;

		case NOT_ENOUGHT_MEMORY:
			printf("== Not enought memory.\n");
			break;
	}
	return;
}

//deletes all elements from list
void intList_delete_all(node **head)
{
	node *temp = *head;
	while (temp != NULL)
	{
		*head = (*head)->next;
		free(temp);
		temp = *head;
	}
	return;
}

//deletes first element with given value
void intList_delete_first(node **head, int data)
{
	node *temp = *head;
	node *prev = NULL;
	int ok = 0;
	while (temp != NULL)
	{
		if (temp->value == data)
		{
			if (temp == (*head)) *head = (*head)->next;
			else prev->next = temp->next;
			ok = 1;
			break;
		}
		prev = temp;
		temp = temp->next;
		if (ok) 
			break;
	}
	free(temp);
	return;
}

//returns size of list
int intList_size(node **head)
{
	node *temp = *head;
	int current = 0;
	while (temp != NULL)
	{
		temp = temp->next;
		current++;
	}
	return current;
}

//prints all elements of list
void intList_print(node **head)
{
	node *temp = *head;
	if (temp == NULL) 
	{
		intList_error(LIST_IS_EMPTY);
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
void intList_push_front(node **head, int data)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		intList_error(NOT_ENOUGHT_MEMORY);
		return;
	}
	temp->value = data;
	temp->next = *head;
	*head = temp;
	return;
}

//adds new element at the end of list
void intList_push_back(node **head, node **tail, int data)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		intList_error(NOT_ENOUGHT_MEMORY);
		return;
	}
	if (*head == NULL) 
	{
		intList_push_front(head, data);
		*tail = *head;
		return;
	}
	temp->value = data;
	temp->next = NULL;
	(*tail)->next = temp;
	*tail = temp;
	return;
}