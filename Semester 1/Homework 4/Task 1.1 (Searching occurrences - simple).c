/*
Simple realisation of the algorithm for searching 
occurrences of a string into another string.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <string.h>

enum 
{ 
	MAX_SIZE_OF_STRING = 1000 
};

int main(void)
{
	char str[MAX_SIZE_OF_STRING], subStr[MAX_SIZE_OF_STRING];
	int lenStr = 0, lenSubStr = 0, i = 0, j = 0, ok = 0, counter = 0;
	
	scanf("%s %s", &str, &subStr);
	lenStr = strlen(str);
	lenSubStr = strlen(subStr);
	for (i = 0; i <= lenStr - lenSubStr; ++i)
	{
		ok = 1;
		for (j = 0; j < lenSubStr; ++j)
		{
			if (str[i + j] != subStr[j])
			{
				ok = 0;
				break;
			}
		}
		if (ok) 
			counter++;
	}
	printf("%d\n", counter);
	return 0;
}