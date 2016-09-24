{-
Homework 1 (07.09.2015)

Author: Mikhail Kita, group 271
-}

inc = (+ 1)
dec = (subtract 1)

-- Sum of two numbers
add x 
    | x == 0    = id
    | x > 0     = inc.(add $ dec x)
    | otherwise = (add $ inc x).dec

-- Multiplication of two numbers
mult x y
    | x == 0    = 0
    | x > 0     = add y (mult (dec x) y)
    | otherwise = add (-y) (mult (inc x) y)

-- Greatest common divider
gcd' a b
    | a == 0 || b == 0 = a + b
    | a > b            = gcd' (mod a b) b 
    | otherwise        = gcd' a $ mod b a

-- Least common multiple
lcm' a b = div (a * b) $ gcd' a b

-- Returns list of dividers of a given number
dividersOf a = find 1 [] where
    find n list
        | n == a       = list ++ [n]
        | mod a n == 0 = find (inc n) $ list ++ [n]
        | otherwise    = find (inc n) list

-- Checks if number is prime
isPrime a
    | a == 1    = False
    | otherwise = check 2
    where
        check n      
            | n == a        = True
            | gcd' n a == 1 = check $ n + 1
            | otherwise     = False

-- Checks whether given numbers are coprime
areCoprime a b = if (gcd' a b == 1) then True else False

-- Clculates the Euler function
euler a = count 1 0 where
    count n res
        | n == a         = res
        | areCoprime a n = count (n + 1) $ res + 1
        | otherwise      = count (n + 1) res