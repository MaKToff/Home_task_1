/*
This program has a buffer overflow vulnerability which allows 
to replace return address and call the external function.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <string.h>

void overflow(char str[]) 
{
	char buffer[4];
	strcpy(buffer, str);
}

void f() 
{
	printf("\nSUCCESSFULLY HACKED!\n");
}

int main(void) 
{
	char str[] = "aaaaaaaaaaaa"     // This is a "rubbish"
		"\x78\x10\x41\x00";     // This is return address of the function f
	printf("f: 0x%x\n", (int)(&f)); // Writes return address of the function f (0x00411078) 
	overflow(str);
	return 0;
}
