"""
Homework 3.
Performs training using generated images.

Author: Mikhail Kita, group 371
"""

from sklearn.externals import joblib
from common import *

squares = get_images(squares_path)
triangles = get_images(triangles_path)

tests = []
labels = []

for square in squares:
    tests.append(calc_hist(square[1]))
    labels.append("square")

for triangle in triangles:
    tests.append(calc_hist(triangle[1]))
    labels.append("triangle")

joblib.dump((tests, labels), "lbp.pkl", compress=3)
