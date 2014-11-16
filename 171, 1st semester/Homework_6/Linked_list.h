/*
Объявление функций для односвязного списка
==========================================
Declaring functions for linked list

Author: Mikhail Kita, group 171
*/

#define UNKNOWN_COMMAND 1
#define LIST_IS_EMPTY 2
#define INCORRECT_ARGUMENT 3
#define NOT_ENOUGHT_MEMORY 4

typedef struct node
{
	int value;
	struct node *next;
} node;

void error(int value);
void del_all(node **head);
void del_first(node **head, int data);
int size(node **head);
void print(node **head);
void push_front(node **head, int data);
void push_back(node **head, node **tail, int data);
