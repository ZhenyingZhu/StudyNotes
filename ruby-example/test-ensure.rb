i = 0
while (i < 10)
    i += 1
    begin
        if (i == 5)
            break
        else
            puts i
        end
    ensure
        puts "ensure"
    end
end
