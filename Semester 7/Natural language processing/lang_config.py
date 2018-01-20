# languages that our classifier will cover
language_codes = ["be", "bg", "ru", "sr", "uk"]

language_descriptions = {
    "be": "Belorussian",
    "bg": "Bulgarian",
    "ru": "Russian",
    "sr": "Serbian",
    "uk": "Ukrainian"}

language_alphabet = {
    'be': 'абвгдеёжзійклмнопрстуўфхцчшыьэюя',
    'bg': 'абвгдежзийклмнопрстуфхцчшщъьюя',
    'ru': 'абвгдеёжзийклмнопрстуфхцчшщъыьэюя',
    'sr': 'абвгдђежзијклљмнњопрстћуфхцчџш',
    'uk': 'абвгґдеєжзиіїйклмнопрстуфхцчшщьюя'}

# length of text samples used for training and prediction
text_sample_size = 140


def define_alphabet():
    """
    Creates a list of unique characters from all alphabets.
    """
    all_lang_chars = ''

    for language in language_alphabet.values():
        all_lang_chars += language

    unique_chars = list(set(list(all_lang_chars)))
    unique_chars.sort()

    return unique_chars


alphabet = define_alphabet()
char_index = {c: i for i, c in enumerate(alphabet)}

print("Alphabet:", alphabet)
