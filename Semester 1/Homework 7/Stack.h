/*
Declaring functions for stack.

Author: Mikhail Kita, group 171
*/

#include "math.h"

typedef struct stack_node
{
	number *value;
	struct stack_node *next;
} stack_node;

// Deletes all elements from the stack
void stack_delete(stack_node **head);

// Clears all elements from the stack
void stack_clear(stack_node **head);

// Returns size of the stack
int stack_size(stack_node **head);

// Returns first element and deletes it from the stack
void stack_pop(stack_node **head, number **num);

// Adds new element to the stack
void stack_push(stack_node **head, number **data);
