"""
Homework 3.
Contains common functions and variables.

Author: Mikhail Kita, group 371
"""

import os
import cv2
import numpy as np
from skimage.feature import local_binary_pattern
from scipy.stats import itemfreq

squares_path = "./data/squares/"
triangles_path = "./data/triangles/"
tests_path = "./data/tests/"


# Gets all images located in given directory
def get_images(path):
    result = []

    for current_path, dirs, files in os.walk(path):
        for file in files:
            image = cv2.imread(current_path + "/" + file, cv2.IMREAD_GRAYSCALE)
            result.append((file, image))

    return result


# Calculates the normalized histogram
def calc_hist(image):
    radius = 3
    points = 8 * radius

    lbp = local_binary_pattern(image, points, radius, method='uniform')
    temp = itemfreq(lbp.ravel())
    hist = np.zeros(points + 2)

    for elem in temp:
        hist[np.int(elem[0])] = elem[1]

    return cv2.normalize(hist, 0, 1, cv2.NORM_MINMAX)
