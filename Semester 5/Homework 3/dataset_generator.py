"""
Homework 3.
Generates data for training and testing.

Author: Mikhail Kita, group 371
"""

import os
import cv2
import numpy as np
from random import randint
from common import *

if not os.path.exists(squares_path):
    os.makedirs(squares_path)

if not os.path.exists(triangles_path):
    os.makedirs(triangles_path)

if not os.path.exists(tests_path):
    os.makedirs(tests_path)


# Generates a square
def generate_square(filename, side):
    image = np.ones((side, side, 3)) * 255

    edge = randint(int(side * 0.1), side)
    x = randint(0, side - edge)
    y = randint(0, side - edge)

    square = np.array([[x, y], [x + edge, y], [x + edge, y + edge], [x, y + edge]])
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
    generate_square(squares_path + "square-" + str(i) + ".png", 50)
    generate_triangle(triangles_path + "triangle-" + str(i) + ".png", 50)

for i in range(0, 200):
    generate_square(tests_path + "square-" + str(i) + ".png", 50)
    generate_triangle(tests_path + "triangle-" + str(i) + ".png", 50)
