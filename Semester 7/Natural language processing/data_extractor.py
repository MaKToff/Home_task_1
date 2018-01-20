import os
import sys
import json
from bs4 import BeautifulSoup
from lang_config import language_alphabet, language_codes

data_path = "./data/"
max_data_size = 27 * 1024 * 1024


def is_correct_word(word, code):
    """
    Checks the correctness of the `word` for a given language.
    """
    correct = sum([1 for symbol in word if symbol in language_alphabet[code]])

    return correct == len(word) and correct != 0


def clean_text(text, filename):
    """
    Removes incorrect characters and words from other languages.
    """
    text = ''.join(symbol if symbol.isalnum() else ' ' for symbol in text)
    text = ' '.join(text.split())
    text = text.lower()

    words = text.split(' ')
    code = filename[:2]
    text = ' '.join([word for word in words if is_correct_word(word, code)])
    text += ' '

    with open(os.path.join(data_path, filename + ".txt"), 'a', encoding='utf-8') as w:
        w.write(text)


def extract_from_books():
    """
    Retrieves data from books.
    """
    for root, _, files in os.walk("./books"):
        for name in files:
            with open(os.path.join(root, name), 'r', encoding='utf-8') as f:
                clean_text(f.read(), name[:2] + "_books")


def extract_from_html():
    """
    Retrieves data from HTML documents.
    """
    for root, _, files in os.walk("./html"):
        for name in files:
            with open(os.path.join(root, name), 'r', encoding='utf-8') as f:
                text = BeautifulSoup(f.read(), "html.parser").get_text()
                clean_text(text, name[:2] + "_html")


def extract_from_json():
    """
    Retrieves data from Twitter posts.
    """
    for root, _, files in os.walk("./json"):
        for name in files:
            try:
                with open(os.path.join(root, name), 'r', encoding='utf-8') as f:
                    line = f.readline()

                    while line:
                        loaded = json.loads(line)

                        if 'user' in loaded.keys():
                            code = str(loaded['user']['lang'])

                            if code in language_codes:
                                text = str(loaded['text']).encode('utf-8').decode('utf-8') + " " + \
                                       str(loaded['user']['description']).encode('utf-8').decode('utf-8')
                                clean_text(text, code + "_twitter")

                        line = f.readline()

            except UnicodeDecodeError:
                continue


def generate_dataset():
    """
    Creates a fixed size dataset.
    """
    for code in language_codes:
        text = ""

        for filename in os.listdir(data_path):
            if not filename.endswith(".txt"):
                continue

            if not filename.startswith(code):
                continue

            with open(os.path.join(data_path, filename), 'r', encoding='utf-8') as f:
                text += f.read()

        words = text.split(' ')
        string = ""
        counter = 0

        for word in words:
            if sys.getsizeof(string) < max_data_size:
                string += word + ' '
                counter += 1
            else:
                break

        with open(os.path.join(data_path, code + ".txt"), 'a', encoding='utf-8') as w:
            w.write(string)

        words = words[counter:]
        string = ' '.join([word for word in words])

        with open(os.path.join(data_path, code + "_other.txt"), 'a', encoding='utf-8') as w:
            w.write(string)


generate_dataset()
