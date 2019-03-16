#pragma once

using namespace System;

public ref class Hello
{
public:
	Hello();
	~Hello();
	double length;
	void setLength(double len);
	double getLength(void);
	String^ getTypeName();
	//algorithm
	int ex1_2(int x);
};
