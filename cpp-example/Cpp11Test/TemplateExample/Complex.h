#pragma once
#include <iostream>
#include <cmath>
#include <string>

template <typename Precision>
class Complex;

template <typename Precision>
std::ostream& operator<<(std::ostream& os, const Complex<Precision>& c);

template <typename Precision>
class Complex {
private:
    Precision r, i;

public:
    Complex(Precision r, Precision i): r(r), i(i) {}

    Complex(const Complex<Precision>& c): r(c.r), i(c.i) {}

    Precision abs() {return sqrt(r * r + i * i);}

    Complex operator-() {
        return Complex(r, -i);
    }

    friend std::ostream& operator<<<Precision>(std::ostream& os, const Complex<Precision>& c);
};

template <typename Precision>
std::ostream& operator<<(std::ostream& os, const Complex<Precision>& c) {
    os << c.r;
    if(c.i >= 0) {
        os << "+i" << c.i;
    } else {
        os << "-i" << -c.i;
    }

    return os;
}
