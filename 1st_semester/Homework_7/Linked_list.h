/*
Объявление функций для односвязного списка
==========================================
Declaring functions for linked list

Author: Mikhail Kita, group 171
*/

#include "Errors.h"

typedef struct intList_node
{
	int value;
	struct intList_node *next;
} intList_node;

//deletes given intList
void intList_delete(intList_node **head);

//clears all elements from list
void intList_clear_all(intList_node **head);

//clears first element with given value
void intList_clear_first(intList_node **head, int data);

//returns size of list
int intList_size(intList_node **head);

//prints all elements of list
void intList_print(intList_node **head);

//adds new element at the head of list
void intList_push(intList_node **head, int data);
