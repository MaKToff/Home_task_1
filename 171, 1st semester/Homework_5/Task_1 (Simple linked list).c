/*
Первичное знакомство с односвязными списками. Реализованы только основные функции
=================================================================================
Simple linked list (only basic functions are realized)

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct node
{
	int value;
	struct node *next;
} node;

node *head = NULL;
node *tail = NULL;

#define UNKNOWN_COMMAND 1
#define LIST_IS_EMPTY 2
#define INCORRECT_ARGUMENT 3
#define NOT_ENOUGHT_MEMORY 4

void error(int value)
{
	if (value == 1) printf("== Unknown command.\n");
	else if (value == 2) printf("== Incorrect operation: list is empty.\n");
	else if (value == 3) printf("== Incorrect argument.\n");
	else if (value == 4) printf("== Not enought memory.\n");
	return;
}

void del_all()
{
	node *temp = head;
	while (temp != NULL)
	{
		head = head->next;
		free(temp);
		temp = head;
	}
	return;
}

void del_first(int data)
{
	node *temp = head;
	node *prev = NULL;
	int ok = 0;
	while (temp != NULL)
	{
		if (temp->value == data)
		{
			if (temp == head) head = head->next;
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

int size()
{
	node *temp = head;
	int current = 0;
	while (temp != NULL)
	{
		temp = temp->next;
		current++;
	}
	return current;
}

int pop_front()
{
	node *temp = head;
	int value = 0;
	if (head == NULL) 
	{
		error(LIST_IS_EMPTY);
		return 0;
	}
	value = temp->value;
	head = head->next;
	free(temp);
	return value;
}

int pop_back()
{
	node *temp = head;
	int value = 0, i = size();
	if (head == NULL) 
	{
		error(LIST_IS_EMPTY);
		return 0;
	}
	if (i == 1) return pop_front();
	value = tail->value;
	while (i > 2) 
	{
		temp = temp->next;
		i--;
	}
	temp->next = NULL;
	free(tail);
	tail = temp;
	return value;
}

void print()
{
	node *temp = head;
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
	printf("\n");
	return;
}

void push_front(int data)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		error(NOT_ENOUGHT_MEMORY);
		return;
	}
	temp->value = data;
	temp->next = head; 
	if (head == NULL) tail = temp;
	head = temp;
	return;
}

void push_back(int data)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		error(NOT_ENOUGHT_MEMORY);
		return;
	}
	if (head == NULL) 
	{
		push_front(data);
		return;
	}
	temp->value = data;
	temp->next = NULL;
	tail->next = temp;
	tail = temp;
	return;
}

void start(char str[22])
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
		del_all();
		exit(0);
	}
	else if (strcmp(command, "del_all") == 0) del_all();

	else if (strcmp(command, "del_first") == 0) del_first(argument);

	else if (strcmp(command, "push_front") == 0) push_front(argument);

	else if (strcmp(command, "push_back") == 0) push_back(argument);

	else if (strcmp(command, "print") == 0) print();

	else if (strcmp(command, "pop_front") == 0) printf("== %d\n", pop_front());

	else if (strcmp(command, "pop_back") == 0) printf("== %d\n",  pop_back());

	else if (strcmp(command, "size") == 0) printf("== %d\n", size());

	else error(UNKNOWN_COMMAND);
	return;
}

void api()
{
	printf("COMMANDS:\n\n");
	printf("exit  ================  Close application\n\n");
	printf("del_all  =============  Delete all elements from list\n\n");
	printf("del_first (int)arg  ==  Delete first element with value = arg\n\n");
	printf("pop_back  ============  Return last element and delete it from list\n\n");
	printf("pop_front  ===========  Return first element and delete it from list\n\n");
	printf("print  ===============  Print all elements of list\n\n");
	printf("push_back (int)arg  ==  Create new element with value = arg at the end of list\n\n");
	printf("push_front (int)arg  =  Create new element with value = arg at the top of list\n\n");
	printf("size  ================  Return size of list\n\n\n");
}

int main(void)
{
	char str[22];
	api();
	while (1)
	{
		gets(str);
		if (strlen(str) > 22) error(UNKNOWN_COMMAND);
		else start(str);
	}
	return 0;
}
