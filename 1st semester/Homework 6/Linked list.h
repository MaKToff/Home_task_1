/*
Declaring functions for linked list.

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

// Prints message about error
void intList_error(int value);

// Deletes all elements of the list
void intList_delete_all(node **head);

// Deletes first element with a given value
void intList_delete_first(node **head, int data);

// Returns size of the list
int intList_size(node **head);

// Prints all elements of the list
void intList_print(node **head);

// Adds new element to a head of the list
void intList_push_front(node **head, int data);

// Adds new element to an end of the list
void intList_push_back(node **head, node **tail, int data);