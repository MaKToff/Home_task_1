length' []     = 0
length' (x:xs) = 1 + (length' xs)

append' []     = id
append' (x:xs) = (x:).append' xs 

-- A trivial way
reverse' []    = []
reverse' (x:xs) = (reverse' xs) ++ [x]

reverse2 = rev [] where
    rev a []     = a
    rev a (x:xs) = rev (x:a) xs

fix f = f (fix f)
reverse3 = fix (\ f a x -> case x of
           [] -> a
           (x:xs) -> f (x:a)xs) []

concat' []     = []
concat' (x:xs) = x : concat' xs

intersperse _ ys | length ys < 2 = ys
intersperse x (y:ys) = y : x : intersperse x ys

zip' _ [] = []
zip' [] _ = []
zip' (x:xs) (y:ys) = (x, y) : zip' xs ys


unzip' [] = ([], [])
unzip' ((x,y):l) = (x:l1, y:l2) where
     (l1, l2) = unzip l

zipwith f [] _ = []
zipwith f _ [] = []
zipwith f (x:xs) (y:ys) = f x y : zipwith f xs ys