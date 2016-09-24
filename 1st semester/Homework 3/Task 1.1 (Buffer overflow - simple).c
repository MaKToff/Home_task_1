/* 
This program has a buffer overflow vulnerability. 
If the input string contains from 21 to 31 symbols, 
you will be logged in despite the password is incorrect.

Author: Mikhail Kita, group 171
*/

#include <stdio.h>
#include <string.h>

void login()
{
	printf("You are logged in. Access granted!\n");
}

int isVerified(char password[])
{
	int ok = 0;
	char buffer[10];
	strcpy(buffer, password);
	if (strcmp(buffer, "qwerty123") == 0) ok = 1;
	return ok;
}

int main(void)
{
	char str[100];
	scanf("%s", &str);
	if (isVerified(str)) login();
	else printf("Incorrect password!");
	return 0;
}
