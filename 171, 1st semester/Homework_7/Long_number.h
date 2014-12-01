/*
Объявление функций для длинных чисел
=====================================
Declaring functions for long numbers

Author: Mikhail Kita, group 171
*/

#include "Linked_list.h"

typedef struct number
{
	int sign;
	intList_node *head;
	intList_node *tail;
} number;

//executes the initial declaration variable of type "number"
number* longNum_init();

//clears given number
void longNum_clear(number **num);

//deletes given number
void longNum_delete(number **num);

//reads digits of number 
void longNum_read(number **result, int *ok);

//deletes leading zeroes in number
void longNum_delete_leading_zeroes(number **num);

//reverses all digits in number
number* longNum_reverse(number **num);