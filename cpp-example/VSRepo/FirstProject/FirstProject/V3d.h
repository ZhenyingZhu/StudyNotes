#pragma once
#include <iostream>

namespace TestFriend {

	class V3d {
	private:
		double x, y, z;
		static int number;
	
	public:
		V3d(double x = 0, double y = 0, double z = 0) { number++; }
		
		~V3d() { number--; }
		
		static int getCount() { return number; }
		
		friend std::ostream& operator<<(std::ostream& s, V3d& v);
		friend V3d operator-(const V3d& v1, const V3d& v2);
		friend double dot(const V3d& v1, const V3d& v2);
	};

}