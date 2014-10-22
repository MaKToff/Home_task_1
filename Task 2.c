//Output the binary representation of number


#include <stdio.h>

int main(void)
{
    int number = 0, i = 0;
	const int bitsInInt = sizeof(int)*8;
    scanf("%d", &number);
    for (i = 0; i < bitsInInt; ++i)
    {
        printf("%d", (number >> bitsInInt - 1) & 1);
        number = number << 1;
    }
    return 0;
}
