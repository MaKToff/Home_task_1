/* 
Простая иллюстрация уязвимости переполнения буфера. Если длина входной строки составит
от 21 до 31 символа, то возможно будет выполнить вход, несмотря на неверный пароль.
======================================================================================
This simple program has a buffer overflow vulnerability. If input string will
have length from 21 to 31, you'll be logged in despite the incorrect password.

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
