/*
Простое решение задачи.
===========================
Simple solution of problem.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <string.h>

int main()
{
	char temp[1000], str[2000], subStr[1000];
	int n = strlen(str), m = strlen(subStr), i = 0, j = 0, ok = 0;
	scanf("%s", &str);
	scanf("%s", &subStr);
	for (i = 0; i <= n - m; ++i)
	{
		ok = 1;
		for (j = 0; j < m; ++j)
		{
			if (str[i + j] != subStr[j])
			{
				ok = 0;
				break;
			}
		}
		if (ok) printf("%d ", i);
	}
	return 0;
}
