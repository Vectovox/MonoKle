script void test2()



endscript


; TEST SCRIPT
;;;;;;;;;;;;;;
script void test ( int x, int y )

print concat("CONCATENATING: ", get()) + concat("    <-->    abc", "def")
int z : x + y
print x
print y
print z
string s : "abc"
set s : "SUPER VALUE: " + (x + y + z)
print s
print "Greater than: " + ((155 + 27) > ((1 - 1) * 27))
print "Less than: " + ((155 + 27)^0 < ((1 - 1) * 27) + 2)

endscript


; CONCATENATES TWO STRINGS
;;;;;;;;;;;;;;;;;;;;;;;;;;;
script string concat(string a, string b)

string s : a + b
return s

endscript


; RETURNS "banana"
;;;;;;;;;;;;;;;;;;
script string get()

return "banana"

endscript


script void printer()

print "PRINT...PRINT...PRINT...DONE!"

endscript