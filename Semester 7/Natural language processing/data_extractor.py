import os
import json
from bs4 import BeautifulSoup
from lang_config import language_alphabet, language_codes


def is_correct_word(word, code):
    correct = sum([1 for symbol in word if symbol in language_alphabet[code]])

    return correct == len(word) and correct != 0


def extract(text, filename):
    text = ''.join(symbol if symbol.isalnum() else ' ' for symbol in text)
    text = ' '.join(text.split())
    text = text.lower()

    words = text.split(' ')
    code = filename[:2]
    text = ' '.join([word for word in words if is_correct_word(word, code)])
    text += ' '

    with open(os.path.join("./data/" + filename + ".txt"), 'a', encoding='utf-8') as w:
        w.write(text)


def extract_from_books():
    for root, _, files in os.walk("./books"):
        for name in files:
            with open(os.path.join(root, name), 'r', encoding='utf-8') as f:
                extract(f.read(), name[:2] + "_books")


def extract_from_html():
    for root, _, files in os.walk("./html"):
        for name in files:
            with open(os.path.join(root, name), 'r', encoding='utf-8') as f:
                text = BeautifulSoup(f.read(), "html.parser").get_text()
                extract(text, name[:2] + "_html")


def extract_from_json():
    for root, _, files in os.walk("./json"):
        for name in files:
            try:
                with open(os.path.join(root, name), 'r', encoding='utf-8') as f:
                    line = f.readline()

                    while line:
                        a = json.loads(line)

                        if 'user' in a.keys():
                            code = str(a['user']['lang'])

                            if code in language_codes:
                                text = str(a['text']).encode('utf-8').decode('utf-8') + " " + \
                                       str(a['user']['description']).encode('utf-8').decode('utf-8')
                                extract(text, code + "_twitter")

                        line = f.readline()

            except UnicodeDecodeError:
                continue


extract_from_books()
