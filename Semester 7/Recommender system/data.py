"""
Data loader.

Author: Kirill Smirenko, 471
"""

from random import shuffle

# In IntelliJ IDEA the working directory is sometimes the root directory of the repo, so use the input_file that works
input_file = "train_triplets.txt"
data = {}
songs = set()


def load_data(max_users=-1):
    """
    Loads the data from txt. It's important that this function is called before other functions in this file.
    :param max_users: the maximum count of loaded users, after which the process stops
    """
    # First pass
    # Count songs for each user
    file = open(input_file, mode='r')
    song_counts = {}
    cur_user = ''
    cur_count = 0

    for line in file:
        new_user = line.split(sep='\t')[0]

        if cur_user != new_user:
            if cur_user != '':
                song_counts[cur_user] = cur_count

            cur_user = new_user
            cur_count = 0

        cur_count += 1

    song_counts[cur_user] = cur_count
    file.close()

    # Select top <max_users>
    if max_users > 0:
        top_users = sorted(song_counts.items(), key=lambda x: x[1], reverse=True)[:max_users]
        top_users = [user for user, _ in top_users]
    else:
        top_users = song_counts.keys()

    top_users = set(top_users)

    # Second pass: read and store data for selected users
    file = open(input_file, mode='r')

    for line in file:
        user, song, play_count = line.split(sep='\t')

        if user not in top_users:
            continue

        play_count = int(play_count)
        songs.add(song)

        if user not in data:
            data[user] = {}

        data[user][song] = play_count

    file.close()

    # Normalize rates
    for user in data:
        max_play_count = max([data[user][song] for song in data[user]])

        for song in data[user]:
            data[user][song] = round(float(data[user][song]) / max_play_count * 9) + 1


def get_data():
    """
    Returns all loaded data in the format {user: {song: playCount}}.
    """
    return data


def get_songs():
    """
    Returns the set of all loaded songs.
    """
    return songs


def get_song_sets(test_proportion=0.2):
    """
    Separates the full song set randomly into the training and testing sets and return both.
    """
    all_songs = list(songs)
    shuffle(all_songs)
    train_proportion = 1 - test_proportion
    train_count = int(len(all_songs) * train_proportion)
    train_songs = all_songs[:train_count]
    test_songs = all_songs[train_count:]
    return set(train_songs), set(test_songs)
