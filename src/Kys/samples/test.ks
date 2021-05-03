#!/bin/kys

var bool = true;
var otherbool = true;

var i = 0;

if(bool)
{
	trace("condition is true");
}
else 
{
	trace("condition is false");
}

while(true)
{
	trace("i is {0}",i);
	i = i++;					// i++ not change the value of i
}

timed while(bool, 25)
{
}
timeout(5)
{
}

wait(bool, 25)
{
}
timeout(5)
{
}

for(var a = 0; a!=10; 1)
{
}
