/*
Returns "1" if a number can fit in the given machine word. 
Else returns "0".

Author: Mikhail Kita, group 171
*/

#include <stdio.h>

int fitsBits(int number, int word) 
{
	const int temp = sizeof(int) * 8 + 1 + ~word;
	int ans = !(((number << temp) >> temp) ^ number);
	return ans;
}

int main(void)
{
	int number = 0, word = 0;
	scanf("%d %d", &number, &word);
	printf("%d", fitsBits(number, word));
	return 0;
}
