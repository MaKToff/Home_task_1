/*
Первичное знакомство с односвязными списками. Реализованы только основные функции
=================================================================================
Simple linked list (only basic functions are realized)

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

//prints message about error
void error(int value)
{
	switch(value)
	{
		case UNKNOWN_COMMAND:
			printf("== Unknown command.\n\n");
			break;

		case LIST_IS_EMPTY:
			printf("== Incorrect operation: list is empty.\n\n");
			break;

		case INCORRECT_ARGUMENT:
			printf("== Incorrect argument.\n\n");
			break;

		case NOT_ENOUGHT_MEMORY:
			printf("== Not enought memory.\n\n");
			break;
	}
	return;
}

//deletes all elements from list
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

//deletes first element with given value
void del_first(int data, node **head)
{
	node *temp = *head;
	node *prev = NULL;
	int ok = 0;
	while (temp != NULL)
	{
		if (temp->value == data)
		{
			if (temp == *head) *head = (*head)->next;
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

//returns size of list
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

//returns first element and deletes it from list
int pop_front(node **head)
{
	node *temp = *head;
	int value = 0;
	if (*head == NULL) 
	{
		error(LIST_IS_EMPTY);
		return 0;
	}
	value = temp->value;
	*head = (*head)->next;
	free(temp);
	return value;
}

//returns last element and deletes it from list
int pop_back(node **head, node **tail)
{
	node *temp = *head;
	int value = 0, i = size(head);
	if (*head == NULL) 
	{
		error(LIST_IS_EMPTY);
		return 0;
	}
	if (i == 1) return pop_front(head);
	value = (*tail)->value;
	while (i > 2) 
	{
		temp = temp->next;
		i--;
	}
	temp->next = NULL;
	free(*tail);
	*tail = temp;
	return value;
}

//prints all elements of list
void print(node **head)
{
	node *temp = *head;
	if (temp == NULL) 
	{
		error(LIST_IS_EMPTY);
		return;
	}
	printf("== ");
	while (temp != NULL)
	{
		printf("%d ", temp->value);
		temp = temp->next;
	}
	printf("\n\n");
	return;
}

//adds new element at the head of list
void push_front(int data, node **head, node **tail)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		error(NOT_ENOUGHT_MEMORY);
		return;
	}
	temp->value = data;
	temp->next = *head; 
	if (*head == NULL) *tail = temp;
	*head = temp;
	return;
}

//adds new element at the end of list
void push_back(int data, node **head, node **tail)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		error(NOT_ENOUGHT_MEMORY);
		return;
	}
	if (*head == NULL) 
	{
		push_front(data, head, tail);
		return;
	}
	temp->value = data;
	temp->next = NULL;
	(*tail)->next = temp;
	*tail = temp;
	return;
}

//converts the source string and calls the appropriate command
void start(char str[22], node **head, node **tail)
{
	int n = strlen(str), i = 0, argument = 0, ok = 0, counter = 0, value = 0, sign = 1;
	char command[22];
	for (i = 0; i < n; ++i)
	{
		if (ok)
		{
			if (counter == 1 && (int)str[i] == '-') sign = -1;
			else if ((int)str[i] < (int)('0') || (int)str[i] > (int)('9'))
			{
				error(INCORRECT_ARGUMENT);
				return;
			}
			else argument = argument * 10 + ((int)str[i] - (int)('0'));
			counter++;
		}
		if (str[i] != ' ') command[i] = str[i];
		else {ok = 1; counter++;}
	}
	command[n - counter] = '\0';
	if ((!strcmp(command, "push_front") || !strcmp(command, "push_back") || !strcmp(command, "del_first")) 
		&& (!counter || argument > 2147483648))
	{
		error(INCORRECT_ARGUMENT);
		return;
	}
	argument *= sign;

	if (strcmp(command, "exit") == 0)
	{
		del_all(head);
		exit(0);
	}
	else if (strcmp(command, "del_all") == 0) 
		del_all(head);

	else if (strcmp(command, "del_first") == 0) 
		del_first(argument, head);

	else if (strcmp(command, "push_front") == 0) 
		push_front(argument, head, tail);

	else if (strcmp(command, "push_back") == 0) 
		push_back(argument, head, tail);

	else if (strcmp(command, "print") == 0) 
		print(head);

	else if (strcmp(command, "pop_front") == 0) 
		printf("== %d\n\n", pop_front(head));

	else if (strcmp(command, "pop_back") == 0) 
		printf("== %d\n\n",  pop_back(head, tail));

	else if (strcmp(command, "size") == 0) 
		printf("== %d\n\n", size(head));

	else error(UNKNOWN_COMMAND);
	return;
}

//prints all commands
void help()
{
	printf("COMMANDS:\n\n");
	printf("exit  ================  Close application\n");
	printf("del_all  =============  Delete all elements from list\n");
	printf("del_first (int)arg  ==  Delete first element with value = arg\n");
	printf("pop_back  ============  Return last element and delete it from list\n");
	printf("pop_front  ===========  Return first element and delete it from list\n");
	printf("print  ===============  Print all elements of list\n");
	printf("push_back (int)arg  ==  Create new element with value = arg at the end of list\n");
	printf("push_front (int)arg  =  Create new element with value = arg at the head of list\n");
	printf("size  ================  Return size of list\n\n");
}

int main(void)
{
	node *head = NULL;
	node *tail = NULL;
	char str[22];
	help();
	while (1)
	{
		gets(str);
		if (strlen(str) > 22) error(UNKNOWN_COMMAND);
		else start(str, &head, &tail);
	}
	return 0;
}
