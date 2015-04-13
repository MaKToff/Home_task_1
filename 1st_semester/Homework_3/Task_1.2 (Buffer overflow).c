/*
Эта программа при помощи уязвимости переполнения буфера 
подменяет адрес возврата, вызывая постороннюю функцию.
=========================================================
This program is using a buffer overflow vulnerability for 
replace return address on address of outside function.

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
	char str[] = "aaaaaaaaaaaa"     //This is "rubbish"
		"\x78\x10\x41\x00";     //This is return address of function f
	printf("f: 0x%x\n", (int)(&f)); //Writes return address of function f (0x00411078) 
	overflow(str);
	return 0;
}
