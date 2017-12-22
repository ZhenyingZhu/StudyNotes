for n = 1:5000
   d5(n) = sin(0.5906*n) + sin(1.0245*n);
end;
sound(d5, 8192)
d6=d5/2; %avoid to be clipped
wavwrite(d6,8192,16,'d5.wav');