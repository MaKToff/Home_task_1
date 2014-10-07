//Binary powering


#include <stdio.h>

int f(int a, int n) 
{
	int ans = 1;
	while (n) 
	{
		if (n & 1) ans *= a;
		a *= a;
		n = n >> 1;
	}
	return ans;
}

int main(void)
{
	int a, n;
	scanf("%d %d", &a, &n);
	printf("%d\n", f(a,n));
	return 0;
}
