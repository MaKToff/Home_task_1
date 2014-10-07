//Returns "1" if the number is positive, "-1" if number is negative and "0" if it's zero


#include <stdio.h>

int f(int n)
{
	return (n >> 31) + !(n >> 31) + ~(!n + ~0);
}

int main(void)
{
	int n;
	scanf("%d", &n);
	printf ("%d\n", f(n));
	return(0);
}
