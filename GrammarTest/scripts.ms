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


script void TestLogic()

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



script void TestArithmetic()

	print "Success: " + ((5 + 5) = 10)
	print "Success: " + ((5 - 5) = 0)
	print "Success: " + ((5 * 5) = 25)
	print "Success: " + ((5 / 5) = 1)
	print "Success: " + ((2 ^ 8) = 256)
	print "Success: " + ((12 % 10) = 2)
	print "Success: " + (-5 = 0 - 5)

endscript


script void TestComment()

	// print "Success: false"
	/*
	// print "Success: false"
	// print "Success: false"
	print "Success: false"
	// print "Success: false"
	print "Success: false"
	*/
	print "Success: true"

endscript



script void TestConditional()

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


script void TestVariables()

	int x : 5
	x : x + 1
	int y : x - 2
	x : x - y

	if x = 2 and y = 4 then
	  print "Success: true"
	else
	  print "Success: false"
	endif

endscript


script void TestLoops()

  int x : 0
  while x < 5 do
    x : x + 1
  endwhile

  print "Success: " + (x = 5)

endscript


script int TestFunctions()

	int x : AddInt(AddInt(AddInt(GetOne(), GetOne()), GetOne()), 5)
	print "Success: " + (x = 8)
	return x

endscript


script void TestObjects(object testObj)

	int x <- testObj.plainIntField
	if x = 8 then
		print "Success: true"
	else 
		print "Success: false"
	endif

	int y : 10
	y <- testObj.Property
	if y = 10 then
		print "Success: false"
	else 
		print "Success: true"
	endif


endscript



/* HELPER SCRIPTS */

script int GetOne()
return 1
endscript

script int AddInt(int a, int b)
return a + b
endscript

script string Concat(string a, string b)
return a + b
endscript