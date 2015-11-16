module F where

import Data.Ratio

type R     = Ratio Integer -- рациональное число неограниченной точности
type Point = (R, R)        -- двумерная рациональная точка

data F = -- двумерная геометрическая фигура
  C Point R   | -- окружность: центр, радиус
  S Point R   | -- квадрат: центр, размер стороны
  R Point R R | -- прямоугольник: центр, размер по горизонтали, 
                -- размер по вертикали
  M [F]         -- непустой список фигур

boundingBox :: F -> (Point, R, R) -- вычисляет ограничивающий прямоугольник, 
                                  -- задаваемый координатой левого верхнего угла,
                                  -- размером по горизонтали, размером по вертикали

boundingBox (C (x, y) r)   = ((x - r, x + r), d, d) where d = 2 * r
boundingBox (S (x, y) a)   = ((x - h, x + h), a, a) where h = a / 2
boundingBox (R (x, y) h v) = ((x - h / 2, y + v / 2), h, v)
boundingBox (M (x:xs))     = foldl union (boundingBox x) $ map boundingBox xs where
    union b1@(p1, _, _) b2@(p2, _, _) = (p, xp' - xp, yp - yp') where
        p@(xp, yp)          = m p1 p2
        (xp', yp')          = swap $ m (swap $ br b1) (swap $ br b2)
        m (x1, y1) (x2, y2) = (min x1 x2, max y1 y2)
        br ((x, y), h, v)   = (x + h, y - v)
        swap (x, y)         = (y, x)