#include <stdio.h>
void main()
{
	int max(int x, int y);
	int a, b, c;
	printf("Please input two number:\n");
	scanf("%d %d", &a, &b); 
	c=max(a, b);
	printf("The max value is %d.\n", c);
}

int max(int x, int y)
{
	int z; 
	if(x>y) z=x;
	else z=y;
	return(z);
}

