# Numbers
This library allows you to use a more realistic real number representation in C#, with the operaror overloading you can use the `RealNumber` class as any other numeric type

This is an example of what can you do
```C#
RealNumber pi = new Constant(3.14, name: "Pi");
RealNumber e = new Constant(2.71, name: "e");

RealNumber sqrt2 = 2;
RealNumber sqrt3 = 3;

sqrt2 = sqrt2 ^ 0.5;
sqrt3 = sqrt3 ^ 0.5;

Console.WriteLine($"({e}+{pi})*({e}-{pi})={(e+pi)*(e-pi)}");
Console.WriteLine($"{pi}*{pi}*{e} = {r}");
Console.WriteLine($"({sqrt2}+{sqrt3})*({sqrt2}-{sqrt3}) = {(sqrt2+sqrt3)*(sqrt2-sqrt3)}");
Console.WriteLine($"({sqrt2})^2 = {sqrt2^2}");
Console.WriteLine($"({sqrt2})*({sqrt2})= {sqrt2 * sqrt2}");
```
And the output is
```console
(e+Pi)*(e-Pi)=(e)^(2) + -1*(Pi)^(2)
Pi*Pi*e = e*(Pi)^(2)
((2)^(1/2)+(3)^(1/2))*((2)^(1/2)-(3)^(1/2)) = -1
((2)^(1/2))^2 = 2
((2)^(1/2))*((2)^(1/2))= 2
```
When an operation is performed it creates a structure categorizing every number structure
```C#
RealNumber n = 3;

Console.WriteLine($"The type of n (n = {n}) is {n.GetType()}");

n *= 0.5;
Console.WriteLine($"The type of n (n = {n}) is {n.GetType()}");

n = n ^ 2;
Console.WriteLine($"The type of n (n = {n}) is {n.GetType()}");

n = n ^ 0.5;
Console.WriteLine($"The type of n (n = {n}) is {n.GetType()}");

n = n ^ 0.5;
Console.WriteLine($"The type of n (n = {n}) is {n.GetType()}");
```
Output:
```console
The type of n (n = 3) is Numbers.NaturalNumber
The type of n (n = 3/2) is Numbers.Fraction
The type of n (n = 9/4) is Numbers.Fraction
The type of n (n = 3/2) is Numbers.Fraction
The type of n (n = (3/2)^(1/2)) is Numbers.Radical
```
