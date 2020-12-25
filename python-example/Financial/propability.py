import sys

def calCombine(total, pieces):
    val = 1.0
    for i in range(1, pieces + 1):
        val *= 1.0 * (total - pieces + i) / i

    return val


def lostNPiecesExpectation(n, lost_rate, total_amount, total_pieces):
    combine = calCombine(total_pieces, n)

    propability = 1.0
    for _ in range(n):
        propability *= lost_rate
    for _ in range(total_pieces - n):
        propability *= 1.0 - lost_rate

    return n * total_amount / total_pieces * combine * propability


def main(args):
    # deliver $10k, given the lost rate, split it into how many piece to have the min lost money?
    total_amount = 10000
    if len(args) > 1:
        total_amount = int(args[1])

    lost_rate = 1.0 / 100.0
    if len(args) > 2:
        lost_rate = float(args[2]) / 100.0

    pieces = 1
    if len(args) > 3:
        pieces = int(args[3])

    lost_expectation = 0.0
    for i in range(1, pieces + 1):
        lost_expectation += lostNPiecesExpectation(i, lost_rate, total_amount, pieces)

    print("With", pieces, "pieces, total lost expectation is", lost_expectation)


if __name__ == "__main__":
    main(sys.argv)
