//Returns "1" if the number is fits in a given machine word. Else returns "0"


#include <stdio.h>

int fitsBits(int x, int n) 
{
	int a = 33 + ~n;
	int b = !(((x << a) >> a) ^ x);
	return b;
}

int main(void)
{
	int x, n;
	scanf("%d %d", &x, &n);
	printf("%d", fitsBits(x,n));
	return 0;
}
