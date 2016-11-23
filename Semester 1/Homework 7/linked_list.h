/*
Declaring functions for linked list.

Author: Mikhail Kita, group 171
*/

#include "errors.h"

typedef struct intList_node
{
	int value;
	struct intList_node *next;
} intList_node;

// Deletes given intList
void intList_delete(intList_node **head);

// Clears all elements of the list
void intList_clear_all(intList_node **head);

// Clears first element with a given value
void intList_clear_first(intList_node **head, int data);

// Returns size of the list
int intList_size(intList_node **head);

// Prints all elements of the list
void intList_print(intList_node **head);

// Adds new element to a head of the list
void intList_push(intList_node **head, int data);
