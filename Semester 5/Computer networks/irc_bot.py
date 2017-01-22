import re
import html
from socket import *
from threading import *


class IRCClient:
    def __init__(self, nickname, channel):
        self.__nickname = nickname
        self.__channel = channel
        self.__socket = socket()
        self.__jokes = self.__get_jokes()

    def __del__(self):
        self.__send("QUIT")
        self.__socket.close()

    def __send(self, message):
        self.__socket.send((message + "\r\n").encode())

    def __get_jokes(self):
        s = socket()
        s.connect(("www.bash.im", 80))
        s.send(("GET /random HTTP/1.1\r\nHost: bash.im\r\n\r\n").encode())

        data = str(s.recv(65536), encoding='windows-1251')
        jokes = re.findall(r"<div class=\"text\">.*</div>", data)
        result = []

        for text in jokes:
            text = text[18:-6]
            text = html.unescape(text)
            text = text.replace("<br>", "\n").replace("<br/>", "\n").replace("<br />", "\n")
            text = text.split("\n")
            result.append(text)

        return result

    def __check(self, message):
        if "show some magic" in message and self.__nickname in message:
            text = self.__jokes.pop()

            for string in text:
                if string != "":
                    print("<" + self.__nickname + ">: " + string)
                    self.__send("PRIVMSG " + self.__channel + " :" + string)

            if len(self.__jokes) == 0:
                self.__jokes = self.__get_jokes()

    def __write(self):
        while True:
            message = input()
            self.__send("PRIVMSG " + self.__channel + " :" + message)
            self.__check(message)

    def __listen(self):
        while True:
            try:
                data = str(self.__socket.recv(2048), encoding='utf-8')
            except UnicodeDecodeError:
                continue

            author = re.sub(r":(.*?)!.*", r"\1", data)[:-1].split("\n")[0]
            index = data.find("@ " + self.__channel + " :")

            if index > 0:
                names = data[index + 4 + len(self.__channel):data.find("\n")]
                print("Participants:", names.replace(' ', ', '))

            if re.match(r".*PING.*", data) is not None:
                self.__send("PO" + data[2:-2])
            elif re.match(r".*JOIN.*", data) is not None:
                print(author + " has joined " + self.__channel)
            elif re.match(r".*QUIT.*", data) is not None:
                print(author + " has quit")
            elif re.match(r".*PRIVMSG.*", data) is not None:
                messages = re.findall(r"PRIVMSG " + self.__channel + " :.*", data)

                for message in messages:
                    print("<" + author + ">: " + message[10 + len(self.__channel):-1])
                    self.__check(message)

    def start(self):
        print("Connecting...")
        self.__socket.connect(("irc.freenode.net", 6667))
        self.__send("USER " + self.__nickname + " * * : ")
        self.__send("NICK " + self.__nickname)
        self.__send("JOIN " + self.__channel)

        listen_thread = Thread(target=self.__listen)
        write_thread = Thread(target=self.__write)

        listen_thread.start()
        write_thread.start()
        listen_thread.join()
        write_thread.join()


client = IRCClient("MaKToff", "#spbnet")
client.start()
