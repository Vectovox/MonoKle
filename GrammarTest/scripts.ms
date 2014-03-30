/* TESTS */

script void TestLogic()

print "Success: " + not false
print "Success: " + (5 is greater than 1 + 3)

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