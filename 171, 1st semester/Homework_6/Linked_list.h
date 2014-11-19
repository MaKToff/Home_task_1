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

//prints message about error
void error(int value);

//deletes all elements from list
void del_all(node **head);

//deletes first element with given value
void del_first(node **head, int data);

//returns size of list
int size(node **head);

//prints all elements of list
void print(node **head);

//adds new element at the head of list
void push_front(node **head, int data);

//adds new element at the end of list
void push_back(node **head, node **tail, int data);
