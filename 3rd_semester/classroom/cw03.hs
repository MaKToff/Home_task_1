import Data.List

from_list :: Eq a => [(a,b)] -> a -> b
from_list list a = snd $ head [ l | l <- list, fst l == a]

fix f = f (fix f)

is_prime n = fix (\p i -> i * i > n || n `rem` i /= 0 && p (i + 1)) 2

primes n = 1 : step [2..n] where
    step []    = []
    step (h:t) = h : step [ k | k <- t, k `rem` h /= 0] 