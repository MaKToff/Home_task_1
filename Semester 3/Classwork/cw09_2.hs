module S where

infixl 5 |+|
infixl 6 |*|

class SemiGroup a where
    (|+|) :: a -> a -> a

class SemiGroup a => Monoid a where
    e :: a

class S.Monoid a => Group a where
    inv :: a -> a

class Group a => Ring a where
    (|*|) :: a -> a -> a

class Ring a => URing a where
    me :: a

class URing a => Field a where
    minv :: a -> a


-- Example
instance SemiGroup Int where
    (|+|) = (+)

instance S.Monoid Int where
    e = 0

data F = F (Int -> Int)

instance SemiGroup F where
    (F f) |+| (F g) = F (f . g)

instance S.Monoid F where
    e = F id

data M = M Int Int --- M m x :: 0 <= x <= m - 1
