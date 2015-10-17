data List a = None | Cons a (List a) deriving (Show)

length' :: List a -> Int
length' None = 0
length' (Cons _ tail) = 1 + length' tail

reverse' :: List a -> List a
reverse' = rev None where
   rev x None = x
   rev x (Cons a tail) = rev (Cons a x) tail

append' :: List a -> List a -> List a
append' None list = list
append' (Cons a tail) list = Cons a $ append' tail list

to_list None = []
to_list (Cons a tail) = (a: to_list tail)

of_list [] = None
of_list (x:xs) = Cons x (of_list xs)  

foldr' f acc (Cons a None) = f a acc
foldr' f acc (Cons a tail) = foldr' f (f a acc) tail

foldl' f acc (Cons a None) = f acc a
foldl' f acc (Cons a tail) = foldl' f (f acc a) tail

map' f None = None
map' f (Cons a tail) = Cons (f a) (map' f tail)