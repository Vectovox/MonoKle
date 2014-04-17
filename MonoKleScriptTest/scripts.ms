/* TESTS */
script void RunTests(object testObj)

	print "<< Testing functions >>"
	TestFunctions()
	print "<< Testing logic >>"
	TestLogic()
	print "<< Testing arithmetic >>"
	TestArithmetic()
	print "<< Testing comments >>"
	TestComment()
	print "<< Testing conditionals >>"
	TestConditional()
	print "<< Testing variables >>"
	TestVariables()
	print "<< Testing loops >>"
	TestLoops()
	print "<< Testing objects >>"
	TestObjects(testObj)

endscript


script void TestLogic() > test

	print "Success: " + (not false)
	print "Success: " + (5 > 4)
	print "Success: " + (5 >= 3)
	print "Success: " + (5 >= 5)
	print "Success: " + (5 <= 5)
	print "Success: " + (3 <= 5)
	print "Success: " + (2 < 5)
	print "Success: " + (5 = 5)
	print "Success: " + (5 != 3)
	print "Success: " + (true or false)
	print "Success: " + (not (true and false))

endscript



script void TestArithmetic() > test
	
	Assert(((5 + 5) = 10))
	print "Success: " + ((5 - 5) = 0)
	print "Success: " + ((5 * 5) = 25)
	print "Success: " + ((5 / 5) = 1)
	print "Success: " + ((2 ^ 8) = 256)
	print "Success: " + ((12 % 10) = 2)
	print "Success: " + (-5 = 0 - 5)

endscript


script void TestComment() > test

	// print "Success: false"
	/*
	// print "Success: false"
	// print "Success: false"
	Assert(false)
	// print "Success: false"
	Assert(false)
	*/
	Assert(true)

endscript



script void TestConditional() > test

	if true then
	  print "Success: true"
	endif

	if false then
	  print "Success: false"
	endif

	if true then
	  print "Success: true"
	else
	  print "Success: false"
	endif

	if false then
	  print "Success: false"
	else
	  print "Success: true"
	endif

	if false then
	  print "Success: false"
	else if true then
	  print "Success: true"
	else
	  print "Success: false"
	endif

	if true then
	  if false then
		print "Success: false"
	  else
		if false then
		  print "Success: false"
		else if false then
		  print "Success: false"
		else if true then
		  if false then
			print "Success: false"
		  else if false then
			print "Success: false"
		  else
			print "Success: true"
		  endif
		endif
	  endif
	else
	  print "Success: false"
	endif

	if true then
	else if true then
	  print "Success: false"
	else
	  print "Success: false"
	endif

	if false then
	  print "Success: false"
	else if true then
	else
	  print "Success: false"
	endif

endscript


script void TestVariables() > test

	int x : 5
	x : x + 1
	int y : x - 2
	x : x - y

	Assert(x = 2 and y = 4)

endscript


script void TestLoops() > test

  int x : 0
  while x < 5 do
    x : x + 1
  endwhile

  Assert(x = 5)

endscript


script int TestFunctions() > test

	int x : AddInt(AddInt(AddInt(GetOne(), GetOne()), GetOne()), 5)
	Assert(x = 8)
	return x

endscript


script void TestObjects(object testObj) > test

	int x <- testObj.plainIntField
	Assert(x = 8)

	// Tests assignment AND property
	int y : 10
	y <- testObj.Property
	Assert(y != 10)

	int z <- testObj.Getter()
	Assert(z = 0)

	int a <- testObj.Add(2, 3)
	Assert(a = 5)

	testObj.Setter(1337)
	int b <- testObj.Getter()
	Assert(b = 1337)

	testObj.plainIntField <- 12
	x <- testObj.plainIntField
	Assert(x = 12)

	testObj.Property <- 15
	x <- testObj.Property
	Assert(x = 15)

endscript



/* HELPER SCRIPTS */

script void Assert(bool value)
	if value then
		print "Success: true"
	else 
		print "Success: false"
	endif
endscript

script int GetOne()
return 1
endscript

script int AddInt(int a, int b)
return a + b
endscript

script string Concat(string a, string b)
return a + b
endscript


/* MISC */
script void ShouldNotCompileEither2()
	ShouldNotCompileEither()
endscript

script void ShouldNotCompile()
	banana = "xx
endscript

script void ShouldNotCompileEither()
	ShouldNotCompile()
endscript