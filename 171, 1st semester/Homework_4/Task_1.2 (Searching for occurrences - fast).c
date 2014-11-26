/*
Алгоритм КМП для поиска вхождений подстроки в строку.
===================================================================
KMP algorithm to search for occurrences of a substring in a string.

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
	char temp[MAX_SIZE_OF_STRING], str[2 * MAX_SIZE_OF_STRING], subStr[MAX_SIZE_OF_STRING];
	int lenStr = 0, lenSubStr = 0, prefix[2000], i = 0, current = 0, counter = 0;
	
	str[0]='\1';
	str[1]='\0';
	scanf("%s", &temp);
	scanf("%s", &subStr);
	
	lenSubStr = strlen(subStr);
	strcat(str, subStr); 
	strcat(str, "\2");
	strcat(str, temp);

	lenStr = strlen(str), 
	prefix[1] = 0;
	for (i = 2; i < lenStr; ++i)
	{
		current = prefix[i-1];
		while (current > 0 && str[i]!=str[current + 1]) 
			current = prefix[current];
		if (str[i] == str[current + 1]) current++;
		prefix[i] = current;
	}
	for (i = lenSubStr + 1; i < lenStr; ++i)
		if (prefix[i] == lenSubStr)
			counter++;
	printf("%d\n", counter);
	return 0;
}