#include <iostream>

int main()
{
    int v1, v2; 
    std::cout << "Enter two numbers: " << std::endl; 
    std::cin >> v1 >> v2; 

    int lower = v1, larger = v2; 
    if (v1 <= v2) {
        lower = v1; 
        larger = v2; 
    } else {
        lower = v2; 
        larger = v1; 
    }

    int sum = 0; 
    for (int val = lower; val <= larger; ++val) {
        sum += val; 
    }

    std::cout << "The sum: " << sum << std::endl; 
    
    return 0;
}

