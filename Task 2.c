//Output the binary representation of number


#include <stdio.h>

int main(void)
{
    int a, i;
    scanf("%d", &a);
    for (i=0; i<32; ++i)
    {
        printf("%d", (a >> 31) & 1);
        a = a << 1;
    }
    return 0;
}
