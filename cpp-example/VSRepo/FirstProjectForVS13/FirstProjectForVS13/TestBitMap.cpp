#include <iostream>
#include <cstdint>
#include <iomanip>

using namespace std;

class Bitmap {
private:
	uint32_t* rgba; // this is pointer to an array, but it is still a pointer
	uint32_t rows, cols;

public:
	Bitmap() {
		rgba = new uint32_t[1];
		rows = 0;
		cols = 0;
	}

	Bitmap(uint32_t r, uint32_t c) {
		rows = r;
		cols = c;
		rgba = new uint32_t[r * c];
		for (uint32_t i = 0; i < r * c; i++) {
			rgba[i] = 0;
		}
	}

	// copy constructor
	Bitmap(const Bitmap& orig):
		rgba(new uint32_t[orig.rows * orig.cols]),
		rows(orig.rows),
		cols(orig.cols) {
		for (uint32_t i = 0; i < rows * cols; ++i)
			rgba[i] = orig.rgba[i];
	}

	Bitmap& operator=(const Bitmap& orig) {
		Bitmap copy(orig);
		swap(copy.rgba, rgba);
		rows = copy.rows;
		cols = copy.cols;

		return *this;
	}

	uint32_t& operator()(uint32_t c, uint32_t r) {
		return rgba[r * cols + c];
	}

	~Bitmap() {
		delete[] rgba;
	}

	Bitmap& horizLine(uint32_t c1, uint32_t c2, uint32_t r, uint32_t x) {
		for (uint32_t i = c1; i <= c2; i++) {
			rgba[r * cols + i] = x;
		}
		return *this;
	}

	Bitmap& vertLine(uint32_t r1, uint32_t r2, uint32_t c, uint32_t x) {
		for (uint32_t i = r1; i <= r2; i++) {
			rgba[i * cols + c] = x;
		}
		return *this;
	}

	friend ostream& operator<<(ostream& s, const Bitmap& a);
};

ostream& operator<<(ostream& s, const Bitmap& a) {
	for (uint32_t i = 0; i < a.rows; i++) {
		for (uint32_t j = 0; j< a.cols; j++) {
			s << setw(4) << a.rgba[i * a.cols + j];
		}
		s << endl;
	}
	return s;
}

/*
int main() {
	Bitmap b1(3, 5); // rows, cols
	cout << b1;
	Bitmap b2(10, 20);
	b2.horizLine(3, 15, 0, 0xFF00FF); // go from (3, 0) to (15, 0) writing color
	b2.vertLine(0, 8, 0, 0x000100);// go from (0, 0) to (0, 8) writing color
	cout << b2;

	return 0;
}
*/