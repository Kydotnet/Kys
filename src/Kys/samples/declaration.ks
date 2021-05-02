#!/home/koto/bin/kys


// declaration examples


var total = "hola mundo";	// variable from string

var year = 2021;			// variable from number

var day = true;				// variable from bool

var helo = year;			// variable from other var

var bool = (false);			// variable with parenthesis

var or = bool || day;		// variable from a boolean operation

var all = day || year;		// allowed because day is true and the operation is intercepted


// not allowed


var and = or && year;		// error because year is not a bool

var year = 2020;			// error because year is already defined

bash = "hola";				// error because bash is not defined