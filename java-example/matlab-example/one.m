A=zeros(6,6);
for j=1:1:6
    for i=1:1:6
        A(j,i)=j^(i-1);
    end
end
C=[22;57;132;268;489;822;];
B=zeros(6,1);

X=[9.0000,7.2500,3.3750,2.2500,0.1250];
Y=zeros(6,1);
for i=1:1:6
    for j=1:1:5
        Y(i)=Y(i)+X(j)*i^(j-1);
    end
end

