import math

def calculateCompoundFutureValue(base, rate, duration):
    """From the PV (base), calculate the FV with compound interest."""
    coumpounding = base
    for _ in range(duration):
        coumpounding *= 1.0 + rate / 100.0
    return coumpounding

def calculateCompoundPastValue(target, rate, duration):
    discounting = 1.0
    for _ in range(duration):
        discounting *= 1.0 + rate / 100.0

    # base
    return target / discounting

def calculateCompoundPastValueBasedOnBenefit(benefit, rate, duration):
    discounting = 1.0
    for _ in range(duration):
        discounting *= 1.0 + rate / 100.0

    # base + benefit
    return benefit / (discounting - 1)

def calculateAnnuityFutureValue(base, payment, rate, duration, pay_at_begin = False):
    saving = base
    for _ in range(duration):
        if pay_at_begin:
            saving += payment

        saving *= 1.0 + rate / 100.0

        if not pay_at_begin:
            # nomorally next payment is after a period since base put in.
            saving += payment
        
    return saving

def calculateAnnuityPayment(target, base, rate, duration):
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

def calculateAnnuityDuration(base, rate, payment, target, pay_at_begin = False):
    duration = 0
    val = base
    while (val < target):
        if pay_at_begin:
            val += payment

        val *= 1 + rate / 100.0

        if not pay_at_begin:
            val += payment

        duration += 1
    
    # In the course the answer is actually - 1, maybe because pay at begin.
    return duration

def calculateNominalRateOfReturn(start_value, end_value, duration):
    # startValue*(1+r)^d=endValue
    val = end_value / start_value
    rate = pow(val, 1.0 / duration) - 1
    return rate * 100.0

def calculateRealRateOfReturnFromNominalRate(nominal_rate, inflation):
    # 1+rr = (1+nr)/(1+in)
    realRate = (1 + nominal_rate / 100) / (1 + inflation / 100.0) - 1
    return realRate * 100.0

def calculateRealRateOfReturn(start_value, end_value, inflation, duration):
    nominal_rate = calculateNominalRateOfReturn(start_value, end_value, duration)
    return calculateRealRateOfReturnFromNominalRate(nominal_rate, inflation)

def calculateMortgagePaymentTotal(principal, yearly_rate, term):
    return principal * (1 + yearly_rate / 12.0 / 100.0 * 30)

def calculateCompoundFutureValueWithTax(base, rate, tax_rate, duration):
    """From the PV (base), calculate the FV with compound interest."""
    coumpounding = base
    for _ in range(duration):
        gross_earning = coumpounding * rate / 100.0
        net_earning = gross_earning * (1.0 - tax_rate / 100.0)
        coumpounding += net_earning
    return coumpounding

def calculateAnnuityFutureValueWithTax(base, payment, rate, tax_rate, duration):
    saving = base
    for _ in range(duration):
        saving += payment

        gross_earning = saving * rate / 100.0
        net_earning = gross_earning * (1.0 - tax_rate / 100.0)

        saving += net_earning

    return saving

def compoundTaxInfluence(payment, interest, fee, tax, duration):
    """With fee and tax based on earning, what is the final amount."""
    total = 0
    for _ in range(duration):
        total += payment
        earning = total * interest / 100.0
        total += earning - earning * fee / 100.0

        # If pay the tax every year for the earning
        # total -= earning * tax / 100.0

    # if pay the tax at the end
    total -= (total - payment * duration) * tax / 100.0

    return total

def compoundFeeInfluence(payment, interest, fee, duration):
    """With fee based on payment, what is the final amount."""
    total = 0
    for _ in range(duration):
        total += payment - payment * fee / 100.0
        total += total * interest / 100.0

    return total