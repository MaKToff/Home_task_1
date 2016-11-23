{-
Homework 2 (14.09.2015)

Author: Mikhail Kita, group 271
-}

import Data.List(delete)

-- Takes n first elements from the list
take' 0 _      = []
take' n (x:xs) = [x] ++ (take (n - 1) xs)

-- Deletes repeat values from the list
nub []     = []
nub (x:xs) = [x] ++ nub (newList x xs) where
    newList a []   = []
    newList a list = [x | x <- list, x /= a]

-- List of permutations
perm []   = [[]]
perm list = [(x:xs) | x <- list, xs <- perm $ delete x list]

-- List of sublists
subs []   = [[]]
subs list = nub [(x:xs) | x <- list, xs <- (subs $ tail' x list) ++ [[]]] where
    tail' a []     = []
    tail' a (x:xs) = if (x == a) then xs else (tail' a xs)

-- List of Cartesian products
dprod _ [] = [[]]
dprod [] _ = [[]]
dprod a b  = [ [x,y] | x <- a, y <- b]