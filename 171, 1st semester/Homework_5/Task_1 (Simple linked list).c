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

void error(int value)
{
	if (value == 1) printf("== Unknown command.\n");
	else if (value == 2) printf("== Incorrect operation: list is empty.\n");
	else if (value == 3) printf("== Incorrect argument.\n");
	else if (value == 4) printf("== Not enought memory.\n");
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
		error(2); //List is empty
		return 0xefffffff;
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
		error(2); //List is empty
		return 0xefffffff;
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
		error(2); //List is empty
		return;
	}
	printf("== ");
	while (temp != NULL)
	{
		printf("%d ", temp->value);
		temp = temp->next;
	}
	printf("\n");
}

void push_front(int data)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		error(4); //Not enought memory 
		return;
	}
	temp->value = data;
	temp->next = head; 
	if (head == NULL) tail = temp;
	head = temp;
}

void push_back(int data)
{
	node *temp = (node*) malloc(sizeof(node));
	if (!temp) 
	{
		error(4); //Not enought memory 
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
}

void start(char str[22])
{
	int n = strlen(str), i = 0, argument = 0, ok = 0, counter = 0, value = 0, negative = 0;
	char temp[22];
	for (i = 0; i < n; ++i)
	{
		if (ok)
		{
			if (((int)str[i] < (int)('0') || (int)str[i] > (int)('9')) && (int)str[i] != (int)('-'))
			{
				error(3); //Incorrect argument
				return;
			}
			if ((int)str[i] == (int)('-')) negative = 1;
				else argument = argument * 10 + ((int)str[i] - (int)('0'));
			counter++;
		}
		if (str[i] != ' ') temp[i] = str[i];
		else {ok = 1; temp[i] = '\0';}
	}
	temp[n - counter] = '\0';
	if (negative) argument *= -1;
	if (!counter && (!strcmp(temp, "push_front") || !strcmp(temp, "push_back") || !strcmp(temp, "del_first")))
	{
		error(3); //Incorrect argument
		return;
	}

	if (strcmp(temp, "exit") == 0) exit(0);
	else if (strcmp(temp, "del_first") == 0) del_first(argument);
	else if (strcmp(temp, "push_front") == 0) push_front(argument);
	else if (strcmp(temp, "push_back") == 0) push_back(argument);
	else if (strcmp(temp, "print") == 0) print();
	else if (strcmp(temp, "pop_front") == 0)
	{
		value = pop_front();
		if (value != 0xefffffff) printf("== %d\n", value);
	}
	else if (strcmp(temp, "pop_back") == 0)
	{
		value = pop_back();
		if (value != 0xefffffff) printf("== %d\n", value);
	}
	else if (strcmp(temp, "size") == 0) printf("== %d\n", size());
	else error(1); //Unknown command
	return;
}

void api()
{
	printf("COMMANDS:\n\n");
	printf("exit __________________  Close application\n");
	printf("del_first <int arg> ___  Delete first element with value = arg\n");
	printf("pop_back ______________  Return last element and delete it from list\n");
	printf("pop_front _____________  Return first element and delete it from list\n");
	printf("print _________________  Print all elements of list\n");
	printf("push_back <int arg> ___  Create new element with value = arg at the end of list\n");
	printf("push_front <int arg> __  Create new element with value = arg at the top of list\n");
	printf("size __________________  Return size of list\n\n\n");
}

int main(void)
{
	char str[22];
	api();
	while (1)
	{
		gets(str);
		if (strlen(str) > 22) error(1); //Unknown command
		else start(str);
	}
	return 0;
}
