/*
Binary exponentiation.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>

int binExp(int base, int power)
{
	int ans = 1;
	while (power) 
	{
		if (power & 1) ans *= base;
		base *= base;
		power = power >> 1;
	}
	return ans;
}

int main(void)
{
	int base = 0, power = 0;
	scanf("%d %d", &base, &power);
	printf("%d\n", binExp(base, power));
	return 0;
}
