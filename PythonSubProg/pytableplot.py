from tabledifffun import *
import sys

def main():
    filename = ""
    while True:
        line = sys.stdin.readline().strip()
        if not line:
            break
        if line.startswith("FILE="):
            filename = line[5:]
            with open("python_alive.txt", "w") as f:
                        f.write(f"Python is alive! Processing: {filename}")
        elif line == "CHECK!":
            print (filename, flush=True)

if __name__ == "__main__":
    main()