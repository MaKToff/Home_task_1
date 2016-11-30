"""
Homework 3.
Generates data for training and testing.

Author: Mikhail Kita, group 371
"""

import os
import cv2
import numpy as np
from math import sqrt, pi, sin, cos
from random import randint

squares_path = "./data/squares/"
triangles_path = "./data/triangles/"
tests_path = "./data/tests/"

if not os.path.exists(squares_path):
    os.makedirs(squares_path)

if not os.path.exists(triangles_path):
    os.makedirs(triangles_path)

if not os.path.exists(tests_path):
    os.makedirs(tests_path)


# Generates a square
def generate_square(filename, side):
    image = np.ones((side, side, 3)) * 255

    edge = randint(int(side * 0.2), int(side / 2.0 * sqrt(2.0)))
    temp = int(edge / sqrt(2.0))
    x0 = randint(temp, side - temp)
    y0 = randint(temp, side - temp)
    a = randint(0, 90) * pi / 180.0
    t = int(edge / 2.0)

    def turn(x, y):
        return [x0 + int(x * cos(a) - y * sin(a)),
                y0 + int(x * sin(a) + y * cos(a))]

    square = np.array([turn(-t, -t), turn(t, -t), turn(t, t), turn(-t, t)])
    color = (randint(0, 255), randint(0, 255), randint(0, 255))

    cv2.fillPoly(image, [square], color)
    cv2.imwrite(filename, image)


# Generates a triangle
def generate_triangle(filename, side):
    image = np.ones((side, side, 3)) * 255

    left = int(side * 0.7)
    right = int(side * 0.3)

    x = [randint(0, side), randint(0, right)]
    y = [randint(0, right), randint(left, side)]
    z = [randint(left, side), randint(left, side)]

    triangle = np.array([x, y, z])
    color = (randint(0, 255), randint(0, 255), randint(0, 255))

    cv2.fillPoly(image, [triangle], color)
    cv2.imwrite(filename, image)


for i in range(0, 800):
    generate_square(squares_path + "square-" + str(i) + ".bmp", 50)
    generate_triangle(triangles_path + "triangle-" + str(i) + ".bmp", 50)

for i in range(0, 200):
    generate_square(tests_path + "square-" + str(i) + ".bmp", 50)
    generate_triangle(tests_path + "triangle-" + str(i) + ".bmp", 50)
