//Returns "1" if the number is fits in a given machine word. Else returns "0"


#include <stdio.h>

int fitsBits(int number, int word) 
{
	int temp = 33 + ~word; //temp = 32 - word
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
