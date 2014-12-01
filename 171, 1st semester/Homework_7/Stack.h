/*
Объявление функций для стека
==============================
Declaring functions for stack

Author: Mikhail Kita, group 171
*/

#include "Math.h"

typedef struct stack_node
{
	number *value;
	struct stack_node *next;
} stack_node;

//clears all elements from list
void stack_clear_all(stack_node **head);

//returns size of list
int stack_size(stack_node **head);

//returns first element and deletes it from list
void stack_pop(stack_node **head, number **num);

//adds new element at the head of list
void stack_push(stack_node **head, number **data);