one_of [] a = False
one_of (x:xs) a = (x == a)|| (one_of xs a) 

intereleave [] y = y
interleave (x:xs) y = x: interleave y xs

one_of' :: [Integer] -> (Integer ->Bool)
one_of' = foldl(\f x y -> y==x || f y)(\_ ->False)

super [] = id
super (x : xs) = x.super xs

super' :: [a -> a] -> a -> a
super' = foldr (.) id

is_increasing l = length l <2 || all (>=0) (zipWith (-) (tail l) l)

is_progression l = (length l < 2 )|| 
                   --(length(nub (zipWith (-) (tail l) l) == 1)
                   ((\d -> all (== head d) d)$zipWith (-) (tail l) l)

sum' s l = if l == [] then s
                      else let value = head l 
                               tail' = tail l
                      in 
                          sum' (s+value) tail'
sum = sum' 0

map' :: (a -> b) -> [a] -> [b]
map' f xs = [ f x | x <- xs]

foldl'' :: (a -> b -> a) -> a -> [b] -> a
foldl'' _ acc [] = acc
foldl'' f acc (x:xs) = foldl'' f (f acc x) xs

foldr'' :: (a -> b -> b) -> b -> [a] -> b
foldr'' _ acc [] = acc
foldr'' f acc (x:xs) = f x (foldr'' f acc xs)

primes :: [Integer]
primes = [ x | x <- [2..], ld x == 2]
    where ld x = length [ y | y <- [1..x], x `mod` y == 0]

fib :: Int -> [Integer]
fib = 0 : 1 : [ fib !! (n - 1) + fib !! (n - 2) | n <- [2..]]