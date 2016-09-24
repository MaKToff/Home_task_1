/*
Returns "1" if a number is positive, "-1" if it is negative 
and "0" if it is zero.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>

int sign(int number)
{
	const int temp = number >> (sizeof(int) * 8 - 1);
	return temp + !temp + ~(!number + ~0);
}

int main(void)
{
	int number = 0;
	scanf("%d", &number);
	printf ("%d\n", sign(number));
	return 0;
}
