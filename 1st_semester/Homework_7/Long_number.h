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
} number;

//executes the initial declaration variable of type "number"
number* longNum_init();

//clears given number
void longNum_clear(number **num);

//deletes given number
void longNum_delete(number **num);

//reads digits of number 
void longNum_read(number **result, char first_digit, int *ok);

//deletes leading zeroes in number
void longNum_delete_leading_zeroes(number **num);

//reverses all digits in number
void longNum_reverse(number **num);
