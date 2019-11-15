import math

def compound(base, rate, duration):
    coumpounding = base
    for _ in range(duration):
        coumpounding *= 1.0 + rate / 100.0
    return coumpounding

def discount(target, rate, duration):
    discounting = 1.0
    for _ in range(duration):
        discounting *= 1.0 + rate / 100.0

    # base
    return target / discounting

def annuity(base, payment, rate, duration):
    saving = base
    for _ in range(duration):
        saving *= 1.0 + rate / 100.0
        saving += payment # add after interest because often next payment is after a period since base put in.
    return saving

def annuityBeginOfYear(base, payment, rate, duration):
    saving = base
    for _ in range(duration):
        saving += payment
        saving *= 1.0 + rate / 100.0
    return saving

def backward_annuity(target, base, rate, duration):
    # target = b*(1+r)^d + p*sum((1+r)^x, x=0..d-1) = b*(1+r)^d + p*((1+r)^d-1)/r

    # v1 = (1+r)^d
    val1 = 1.0
    for _ in range(duration):
        val1 *= 1.0 + rate / 100.0
    
    # v2 = b*(1+r)^d
    val2 = val1 * base

    # v3 = p*((1+r)^d-1)
    val3 = (target - val2) * rate / 100.0

    # v4 = (1+r)^d-1
    val4 = val1 - 1
    
    # payment
    return val3 / val4

def nominalRateOfReturn(start_value, end_value, duration):
    # startValue*(1+r)^d=endValue
    val = end_value / start_value
    rate = pow(val, 1.0 / duration) - 1
    return rate * 100.0

def realRateOfReturn(start_value, end_value, inflation, duration):
    nominal_rate = nominalRateOfReturn(start_value, end_value, duration) / 100.0
    # 1+rr = (1+nr)/(1+in)
    realRate = (1 + nominal_rate) / (1 + inflation / 100.0) - 1
    return realRate * 100.0

def compoundDuration(base, rate, payment, target):
    duration = 0
    val = base
    while (val < target):
        val *= 1 + rate / 100.0
        val += payment
        duration += 1
    
    return duration