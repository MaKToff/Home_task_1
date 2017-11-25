"""
Model.

Author: Mikhail Kita, 471
"""

import math

similar_for_user = {}


def get_user_vector(data, user, songs_set):
    """
    Returns songs vector, which contains only elements from the songs_set.
    """
    vector = {}

    if user in data:
        items = list(data[user].keys() & songs_set)
        vector = {key: data[user][key] for key in items}

    return vector


def cosine_similarity(user1, user2):
    """
    Calculates the cosine similarity of two users.
    """
    intersection = list(set(user1.keys()) & set(user2.keys()))
    common_sum = sum([user1[i] * user2[i] for i in intersection])

    values1 = list(user1.values())
    values2 = list(user2.values())
    sum1 = sum([x ** 2 for x in values1])
    sum2 = sum([x ** 2 for x in values2])
    sqrt = math.sqrt(sum1 * sum2)

    return common_sum / sqrt if sqrt != 0 else 0


def get_similar(data, user, number_of_nearest_users, learning_set):
    """
    Returns an array of users most similar to the given user.
    """
    similarity = []
    user_vector = get_user_vector(data, user, learning_set)

    for another_user in data.keys():
        another_user_vector = get_user_vector(data, another_user, learning_set)
        cos = cosine_similarity(user_vector, another_user_vector)
        similarity.append((cos, another_user))

    similarity.sort()
    length = len(similarity)

    return similarity[length - number_of_nearest_users - 1: length - 1]


def learning(data, learning_set, number_of_nearest_users=10):
    """
    Fills the similarity array for every user.
    """
    for user_id in data.keys():
        similar_for_user[user_id] = get_similar(data, user_id, number_of_nearest_users, learning_set)


def rate_prediction(data, user, song):
    """
    Returns the predicted rate for the current user and song.
    """
    similarity = similar_for_user[user]
    number_of_rates = 0
    sum_of_rates = 0.0

    for (rate, similar) in similarity:
        if data.get(similar) is None:
            continue

        value = data[similar].get(song)

        if value is not None:
            number_of_rates += 1
            sum_of_rates += value

    return sum_of_rates / number_of_rates if number_of_rates != 0 else 0
