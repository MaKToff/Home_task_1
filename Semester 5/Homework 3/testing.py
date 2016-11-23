"""
Homework 3.
Performs recognition of test images.

Author: Mikhail Kita, group 371
"""

import re
import cv2
import numpy as np
from sklearn.externals import joblib
from common import *

tests, labels = joblib.load("lbp.pkl")
images = get_images(tests_path)
ok = 0

for file in images:
    hist = calc_hist(file[1])
    results = []

    for index, x in enumerate(tests):
        score = cv2.compareHist(np.array(x, dtype=np.float32), np.array(hist, dtype=np.float32), 1)
        results.append((labels[index], score))

    results = sorted(results, key=lambda t: t[1])
    answer = re.sub(r"(.*)-.*", r"\1", file[0])

    if answer == results[0][0]:
        ok += 1
        print("OK")
    else:
        print("Incorrect, image {} is not a {}.".format(file[0], results[0][0]))

print("Accuracy: ", str(round(float(ok) / float(len(images)) * 100, 2)) + "%")
