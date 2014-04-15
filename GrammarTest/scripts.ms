/* TESTS */

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


script int TestFunctions()

int x : AddInt(AddInt(AddInt(GetOne(), GetOne()), GetOne()), 5)
print "Should be 8: " + x
return x

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