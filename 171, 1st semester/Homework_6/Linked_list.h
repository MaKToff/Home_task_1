/*
Объявление функций для односвязного списка
==========================================
Declaring functions for linked list

Author: Mikhail Kita, group 171
*/

enum errors
{
	UNKNOWN_COMMAND = 1,
	LIST_IS_EMPTY = 2,
	INCORRECT_ARGUMENT = 3,
	NOT_ENOUGHT_MEMORY = 4
};

typedef struct node
{
	int value;
	struct node *next;
} node;

//prints message about error
void intList_error(int value);

//deletes all elements from list
void intList_delete_all(node **head);

//deletes first element with given value
void intList_delete_first(node **head, int data);

//returns size of list
int intList_size(node **head);

//prints all elements of list
void intList_print(node **head);

//adds new element at the head of list
void intList_push_front(node **head, int data);

//adds new element at the end of list
void intList_push_back(node **head, node **tail, int data);