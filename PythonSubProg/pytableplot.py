import sys

def main():
    filename = ""
    while True:
        line = sys.stdin.readline().strip()
        if line.startswith("FILE="):
            filename = line[5:]
        elif line == "CHECK!":
            print (filename, flush=True)
        else:
            print("uncknown command", flush=True)
if __name__ == "__main__":
    main()