{-
Homework 1 (07.09.2015)

Author: Mikhail Kita, group 271
-}

inc = (+ 1)
dec = (subtract 1)

--sum of two numbers
add x | x == 0    = id
      | x > 0     = inc.(add $ dec x)
      | otherwise = (add $ inc x).dec

--multiplication of two numbers
mult x y | x == 0 || y == 0 = 0
         | y > 0            = add x (mult x $ dec y)
         | x > 0            = add y (mult (dec x) y)
         | otherwise        = add (-x) (mult x $ inc y)

--greatest common divider
gcd' a b | a == 0 || b == 0 = a + b
         | a > b            = gcd' (mod a b) b 
         | otherwise        = gcd' a $ mod b a

--least common multiple
lcm' a b = div (a * b) $ gcd' a b

--returns list of dividers of given number
dividersOf a = [x | x <- [1 .. a], a `mod` x == 0]

--checks if number is prime
isPrime a | a == 1    = False
          | otherwise = null [x | x <- [ 1 .. div a 2], gcd' a x /= 1]

--checks whether given numbers are coprime
areCoprime a b = if (gcd' a b == 1) then True else False

--calculates the Euler function
euler a = length [x | x <- [ 1 .. a - 1], areCoprime a x]