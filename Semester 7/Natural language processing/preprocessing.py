import os

import keras
import numpy as np
from sklearn import preprocessing
from sklearn.model_selection import train_test_split
from lang_config import *


# region Sampling and generating input vectors for the NN

# Counts number of chars in text based on given alphabet
def count_chars(text):
    alphabet_counts = []

    for letter in alphabet:
        count = text.count(letter)
        alphabet_counts.append(count)

    return alphabet_counts


def get_sample(file_content, start_index, sample_size):
    """Returns a sample of text from `file_content` of length no more than `sample_size`, starting at `start_index`
    and preserving full words."""
    # we want to start from full first word
    # if the first character is not space, move to next ones
    while not (file_content[start_index].isspace()):
        start_index += 1

    # now we look for first non-space character - beginning of any word
    while file_content[start_index].isspace():
        start_index += 1

    end_index = min(len(file_content) - 1, start_index + sample_size)

    # we also want full words at the end
    while not (file_content[end_index].isspace()):
        end_index -= 1

    return file_content[start_index:end_index]


def build_input_vector(sample_text):
    """Creates an input vector for the NN from the provided sample.
    Currently it is the vector of letter counts."""
    return count_chars(sample_text.lower())


vector_size = len(build_input_vector(""))


def create_sample_vectors(cleaned_data_directory, out_vectors_path):
    vectors = []

    for filename in os.listdir(cleaned_data_directory):
        if not filename.endswith(".txt"):
            continue

        path = os.path.join(cleaned_data_directory, filename)
        f = open(path, mode='r', encoding='utf8')

        print(f"Processing {path}")

        lang = filename[:2]
        lang_number = language_codes.index(lang)

        print(f"\tLanguage: {lang} ({lang_number})")
        print("\tReading...", end=' ')

        file_content = f.read()
        content_length = len(file_content)

        print("done.")
        print("\tExtracting vectors...", end=' ')

        sample_start_index = 0
        count = 0

        while sample_start_index + text_sample_size < content_length:
            sample = get_sample(file_content, sample_start_index, text_sample_size)
            input_vector = build_input_vector(sample)
            vector = input_vector + [lang_number]
            vectors.append(vector)
            sample_start_index += text_sample_size
            count += 1

        print("done.")
        print(f"\tExtracted {count} vectors.")

        del file_content

    print(f"Total {len(vectors)} vectors.")

    np_vectors = np.array(vectors, dtype=np.uint16)

    print(f"Converted to NumPy array, shape: {np_vectors.shape}.")

    np.savez_compressed(out_vectors_path, data=np_vectors)

    print(f"Saved to {out_vectors_path}.")


# endregion

# region Generating train/test data, with preprocessing

def size_kb(size):
    """Utility function, returns file size in KB"""
    size = '{:.2f}'.format(size / 1000.0)

    return f"{size} KB"


def gen_train_test(vectors_path, out_train_test_path):
    data = np.load(vectors_path)['data']
    x = data[:, 0:vector_size]
    y = data[:, vector_size]

    del data

    # x preprocessing
    standard_scaler = preprocessing.StandardScaler().fit(x)
    x = standard_scaler.transform(x)

    # Convert y to binary class matrix (for categorical_crossentropy)
    y = keras.utils.to_categorical(y, num_classes=len(language_codes))

    # Static seed to have comparable results for different runs
    seed = 42
    x_train, x_test, y_train, y_test = train_test_split(x, y, test_size=0.20, random_state=seed)

    del x, y

    np.savez_compressed(out_train_test_path, X_train=x_train, y_train=y_train, X_test=x_test, y_test=y_test)

    print(f"Saved train/test data to {out_train_test_path}, size: {size_kb(os.path.getsize(out_train_test_path))} KB.")

    del x_train, y_train, x_test, y_test

# endregion
