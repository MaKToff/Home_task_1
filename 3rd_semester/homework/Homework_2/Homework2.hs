{-
Homework 2 (14.09.2015)

Author: Mikhail Kita, group 271
-}

import Data.List(delete)

--takes n first elements from list
take' 0 _      = []
take' n (x:xs) = [x] ++ (take (n - 1) xs)

--deletes repeat values from list
nub []     = []
nub (x:xs) = [x] ++ nub (newList x xs) where
    newList a []   = []
    newList a list = [x | x <- list, x /= a]

--list of permutations
perm []   = [[]]
perm list = [(x:xs) | x <- list, xs <- perm $ delete x list]

--list of sublists
subs []   = [[]]
subs list = nub [(x:xs) | x <- list, xs <- (subs $ tail' x list) ++ [[]]] where
    tail' a []     = []
    tail' a (x:xs) = if (x == a) then xs else (tail' a xs)

--list of Cartesian products
dprod _ [] = [[]]
dprod [] _ = [[]]
dprod a b  = [ [x,y] | x <- a, y <- b]