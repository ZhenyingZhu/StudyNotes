require 'thread'

class MemoryUsageMonitor
  attr_reader :peak_memory

  def initialize(frequency=0.005)
    @frequency = frequency
    @peak_memory = 0
  end

  def start
    @thread = Thread.new do
      while true do
        memory = `ps -o rss -p #{Process::pid}`.chomp.split("\n").last.strip.to_i
        @peak_memory = [memory, @peak_memory].max
        sleep @frequency
      end
    end
  end

  def stop
    Thread.kill(@thread)
  end
end


mm = MemoryUsageMonitor.new
mm.start

sum = 0
items = []
5_000_000.times do |n|
  sum += n
  items << n.to_s if rand > 0.8
end

mm.stop
puts "Peak memory: #{mm.peak_memory} KB"
