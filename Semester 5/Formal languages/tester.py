"""
Tester for grammar.

Authors: Mikhail Kita, Sharganov Artem
"""

import sys


# Reads grammar rules from specified file.
def load_grammar(filename):
    grammar = []

    with open(filename, 'r') as f:
        text = f.readlines()

        for line in text:
            left, right = line.split(" -> ")
            right = right.replace("\n", "")
            grammar.append((left, right))

    return grammar


# Checks whether grammar can derive given string.
def check(grammar, string):
    if (string == "00") | (string == "10"):
        string = "S"
    elif "1" in string:
        string = "#" + string + "[q1]$"
    elif len(string) > 2:
        string = "#" + string[:-2] + "[q1]$"

    derivation = []
    finished = False

    while not finished:
        finished = True

        for rule in grammar:
            s = string.replace(rule[0], rule[1], 1)

            if s != string:
                string = s
                derivation += [rule[0] + " -> " + rule[1]]
                finished = False
                break

    if all(elem in ["0", "1"] for elem in string):
        return derivation


# Generates all strings, which can be derived by the type-0 grammar.
def language0():
    strings = ["1"]

    for elem in strings:
        derivation = check(grammar0, elem)

        if derivation:
            print(elem + " = " + str(int(elem, 2)))

        strings += [elem + "0", elem + "1"]


# Generates all strings, which can be derived by the type-1 grammar.
def language1():
    string = "0"

    while True:
        derivation = check(grammar1, string)

        if derivation:
            print(string + " = " + str(len(string)))

        string += "0"


# Prints given derivation.
def print_derivation(derivation):
    if derivation:
        print(*derivation, sep='\n')
    else:
        print("Can't derive given string.")


grammar0 = load_grammar("grammar_type0.txt")
grammar1 = load_grammar("grammar_type1.txt")

if len(sys.argv) == 3:
    if sys.argv[1] == "-d0":
        print_derivation(check(grammar0, sys.argv[2]))
    elif sys.argv[1] == "-d1":
        print_derivation(check(grammar1, sys.argv[2]))
if len(sys.argv) == 2:
    if sys.argv[1] == "-t0":
        language0()
    elif sys.argv[1] == "-t1":
        language1()
