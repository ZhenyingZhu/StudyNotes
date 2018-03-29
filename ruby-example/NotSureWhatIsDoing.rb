MAX = 99999999

def answer(a, b)
        l = Array.new
        return cal(a, b, l)
end
def cal(a, b, l)
        if a > b
                return MAX
        elsif a == b
                l[b-a] = 0
        else
                # for case where a < b
                return l[b-a] unless l[b-a].nil?

                t0 = cal(a, b-1, l) + 1
                t1 = t2 = MAX
                if( b%2 == 0 && b/2 >= a)
                        t1 = cal(a, b/2, l) + 1
                end

                if( (b+1)%2 == 0 && (b+1)/2 >=a )
                        t2 = cal(a, (b+1)/2, l) +2
                end
                l[b-a] = [t0, t1, t2].min
        end

        return l[b-a]
end

puts answer(10, 18)
