from tabledifffun import *
import sys

def main():
    filename = ""
    while True:
        line = sys.stdin.readline().strip()
        if not line:
            break
        if filename == "":
            filename = line
        elif line == "CHECK!":
            print (filename, flush=True)