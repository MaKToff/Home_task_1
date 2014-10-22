//Returns "1" if the number is fits in a given machine word. Else returns "0"


#include <stdio.h>

int fitsBits(int number, int word) 
{
	const int bitsInInt = sizeof(int) * 8;
	int temp = bitsInInt + 1 + ~word; //temp = bitsInInt - word
	int ans = !(((word << temp) >> temp) ^ word);
	return ans;
}

int main(void)
{
	int number = 0, word = 0;
	scanf("%d %d", &number, &word);
	printf("%d", fitsBits(number, word));
	return 0;
}
