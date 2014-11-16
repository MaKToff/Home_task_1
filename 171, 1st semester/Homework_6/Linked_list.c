/*
Реализация односвязного списка
===============================
Linked list

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define UNKNOWN_COMMAND 1
#define LIST_IS_EMPTY 2
#define INCORRECT_ARGUMENT 3
#define NOT_ENOUGHT_MEMORY 4

typedef struct node
{
	int value;
	struct node *next;
} node;

void error(int value)
{
	if (value == 1) printf("== Unknown command.\n");
	else if (value == 2) printf("== Incorrect operation: list is empty.\n");
	else if (value == 3) printf("== Incorrect argument.\n");
	else if (value == 4) printf("== Not enought memory.\n");
	return;
}

void del_all(node **head)
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

void del_first(node **head, int data)
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
		if (ok) break;
	}
	free(temp);
	return;
}

int size(node **head)
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

void print(node **head)
{
	node *temp = *head;
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

void push_front(node **head, int data)
{
	node *temp = (node*) malloc(sizeof(node));
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

void push_back(node **head, node **tail, int data)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		error(NOT_ENOUGHT_MEMORY);
		return;
	}
	if (*head == NULL) 
	{
		push_front(head, data);
		*tail = *head;
		return;
	}
	temp->value = data;
	temp->next = NULL;
	(*tail)->next = temp;
	*tail = temp;
	return;
}
