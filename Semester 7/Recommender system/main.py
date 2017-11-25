"""
Main file.

Author: Mikhail Kita, 471
"""

import time
import datetime
from data import load_data, get_data, get_song_sets
from model import learning
from metrics import rmse, mae, ndcg

max_users = 1000
iterations = 5

print("Data loading...")
load_data(max_users)
print("Finished.")

# Cross validation
for i in range(0, iterations):
    print("Iteration", i + 1, "/", iterations)
    print("Learning is in process...")

    learning_set, testing_set = get_song_sets()
    data = get_data()

    start_time = time.time()
    learning(data, learning_set, 100)
    finish_time = time.time()

    print("Learning finished. Time:", datetime.timedelta(seconds=finish_time - start_time))

    print("RMSE:", rmse(data, testing_set))
    print("MAE: ", mae(data, testing_set))
    print("NDCG:", ndcg(data, testing_set))
    print("=====")
