/*
Declaring functions for long numbers.

Author: Mikhail Kita, group 171
*/

#include "linked_list.h"

typedef struct number
{
	int sign;
	intList_node *head;
} number;

// Executes the initial declaration of a number variable
number* longNum_init();

// Clears a given number
void longNum_clear(number **num);

// Deletes a given number
void longNum_delete(number **num);

// Reads digits of a number 
void longNum_read(number **result, char first_digit, int *ok);

// Deletes leading zeroes in a number
void longNum_delete_leading_zeroes(number **num);

// Reverses all digits in a number
void longNum_reverse(number **num);
