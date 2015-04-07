ts = []

for i in 1..10 do
    ts << Thread.new(i) do |i| 
        x = i
        puts "#{x} \n"
    end
end

ts.each do |t|
    t.join()
end

