"""
Homework 2.
Contains common functions and variables.

Author: Mikhail Kita, group 371
"""

import os
import cv2
import numpy as np
import matplotlib.pyplot as plot
from math import sin, cos, pi

file = cv2.imread("chessboard.bmp")
array = np.array(file, dtype='u1')
line = array[4]
path = "./results/"

if not os.path.exists(path):
    os.makedirs(path)


# Returns a function which sets the analog signal
def analog_signal(x, t_s):
    def helper(t):
        m = 4
        l = int((-m / 2.0 + t) / t_s) + 1
        r = int((m / 2.0 + t) / t_s)
        module = len(x)
        result = np.zeros(3)

        for n in range(l, r):
            index = n % module
            divider = pi * (t - n * t_s)
            arg = divider / t_s

            if divider != 0:
                result += sin(arg) / divider * x[index]
            else:
                result += x[index]

        return np.array(result, dtype='u1')

    return helper


# Passes signals with a frequency lower than a certain cutoff
def low_pass_filter(x):
    m = 4
    size = len(x)
    omega = pi / 4.0
    h = np.zeros(size)

    for n in range(0, m):
        t = n - m / 2.0
        w = 0.3 - 0.5 * cos(2 * pi * n / m)

        if t != 0:
            h[n] = sin(omega * t) / (pi * t) * w

    result = np.zeros((size, 3), dtype='u1')

    for n in range(0, size):
        temp = np.zeros(3)

        for k in range(0, m):
            temp += h[k] * x[n - k]

        result[n] = np.array(temp, dtype='u1')

    return result


# Scales given row in the image
def scale(x, t_s, k):
    y = analog_signal(x, t_s)
    new_t = 1.0 / k
    result = np.zeros((k, 3), dtype='u1')

    for n in range(0, k):
        result[n] = y(n * new_t)

    return result


# Creates a plot for given array and saves results into the file
def draw_plot(source, filename):
    plot.plot(source, linewidth=2.0)
    plot.savefig(path + filename)
    plot.close()


# Creates a map for given matrix and saves results into the file
def draw_map(source, filename):
    plot.imshow(source)
    plot.savefig(path + filename)
    plot.close()


draw_plot(abs(np.fft.fft(line)), "fig1.png")
draw_map(abs(np.fft.fft2(array)), "fig2.png")
