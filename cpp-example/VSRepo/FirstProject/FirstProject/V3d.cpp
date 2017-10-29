#include "V3d.h"
#include <iostream>

using namespace std;

namespace TestFriend {
	ostream& operator<<(ostream& s, V3d& v) {
		s << "{" << v.x << "," << v.y << "," << v.z << "}" << endl;
		return s;
	}

	V3d operator-(const V3d& v1, const V3d& v2) {
		V3d v;
		v.x = v1.x - v2.x;
		v.y = v1.y - v2.y;
		v.z = v1.z - v2.z;
		return v;
	}

	double dot(const V3d& v1, const V3d& v2) {
		double d;
		d = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
		return d;
	}

	int V3d::number = 0;
}