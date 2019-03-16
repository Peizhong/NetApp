#include "pch.h"
#include <iostream>
#include "hello.h"

using namespace std;

Hello::Hello(void)
{
	cout <<"Object is being created"<< endl;
}

Hello::~Hello(void)
{
	cout << "Object is being deleted" << endl;
}

double Hello::getLength(void)
{
	return length;
}

void Hello::setLength(double len)
{
	length = len;
}

String^ Hello::getTypeName() {
	String^ s = this->GetType()->ToString();
	return s;
}

#pragma region Algorithm
int Hello::ex1_2(int x) 
{
	if (x == 0)
		return 0;
	else
		return 2 * ex1_2(x - 1) + x * x;
}
#pragma endregion
