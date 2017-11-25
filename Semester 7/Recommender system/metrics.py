"""
Metrics implementation.

Author: Evgeny Sergeev, 471
"""

import math
from model import get_user_vector, rate_prediction


def calculate_error(data, test_songs, f):
    """
    The subsidiary function for MAE and RMSE metrics.
    """
    number_of_rates = 0
    sum_of_rates = 0.0
    all_users = data.keys()

    for user in all_users:
        user_vector = get_user_vector(data, user, test_songs)

        for song in user_vector:
            real_rate = data[user].get(song, -1)

            if real_rate == -1:
                continue

            prediction = rate_prediction(data, user, song)
            number_of_rates += 1
            diff = real_rate - prediction
            sum_of_rates += f(diff)

    return sum_of_rates / number_of_rates if number_of_rates != 0 else 0


def rmse(data, test_songs):
    """
    Calculates the root square mean error.
    """
    error = calculate_error(data, test_songs, lambda x: x ** 2)
    return math.sqrt(error)


def mae(data, test_songs):
    """
    Calculates the mean absolute error.
    """
    return calculate_error(data, test_songs, abs)


def dcg(data, test_songs, ideal=False):
    """
    Calculates the discounted cumulative gain.
    """
    number_of_rates = 0
    sum_of_rates = 0.0
    all_users = data.keys()

    for user in all_users:
        user_data = data[user]
        sorted_user_data = sorted(user_data.items())
        items = enumerate(sorted_user_data, 1)

        for song_position, (song, real_rate) in items:
            if song not in test_songs:
                continue

            number_of_rates += 1
            log = math.log2(float(song_position))
            maximum = max(1.0, log)

            if ideal:
                sum_of_rates += real_rate / maximum
            else:
                sum_of_rates += rate_prediction(data, user, song) / maximum

    return sum_of_rates / number_of_rates if number_of_rates != 0 else 0


def ndcg(data, test_songs):
    """
    Calculates the normalized discounted cumulative gain.
    """
    ideal_dcg_value = dcg(data, test_songs, ideal=True)
    dcg_value = dcg(data, test_songs)
    return dcg_value / ideal_dcg_value if ideal_dcg_value != 0 else 0
