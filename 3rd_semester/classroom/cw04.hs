import Data.List

iter_with :: [a -> a] -> [a] -> [a]
iter_with fs xs = 
    [foldl (.) id (take i $ repeat f) $ x | (f, x, i) <- zip3 fs xs [0..]]

from_list :: Eq a => [(a,b)] -> a -> b
from_list ps x = head [z | (y,z) <- ps, y == x]

sums :: [Integer] -> [Integer]
sums xs = [ sum $ take i x | (i,x) <- zip [1..] (repeat xs)]

repeatBy :: [a] -> [Int] -> [a]
repeatBy l l' = concat [ take n $ repeat x | (x,n) <- zip l l']

newMap f list = foldr (\x acc -> (f x) : acc) [] list

combine :: [Int] -> [Int] -> [(Int, Int)]
combine l l' = [ (x, y) | x <- l, y <- l', x * 3 == y || y * 3 == x]

sum_prev :: [Int] -> [Int -> Int]
sum_prev list = [ (\x -> x + (sum $ take i list)) | (_, i) <- zip list [0..]]