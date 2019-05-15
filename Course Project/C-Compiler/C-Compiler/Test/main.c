static int a = 3;

typedef int myType;

int add(int a, int b) 
{
	return a + b;
}

int main(int argc, char* argv[]) 
{
	myType z = 0;
	int a = 5;
	int b = 4;
	int c = add(a, b);
	return z;
}