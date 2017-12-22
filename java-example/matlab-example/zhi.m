A2=A;
C2=C;
for j=2:1:6
    for i=1:1:6
        A2(j,i)=A2(j,i)-A2(1,i);
        C2(j,1)=C2(j,1)-C2(1,1);
    end
end
for j=3:1:6
    k=A2(j,2)/A2(2,2);
    for i=2:1:6
        A2(j,i)=A2(j,i)-k*A2(2,i);
        C2(j,1)=C2(j,1)-k*C2(2,1);
    end
end
for j=4:1:6
    k=A2(j,3)/A2(3,3);
    for i=3:1:6
        A2(j,i)=A2(j,i)-k*A2(3,i);
        C2(j,1)=C2(j,1)-k*C2(3,1);
    end
end
for j=5:1:6
    k=A2(j,4)/A2(4,4);
    for i=4:1:6
        A2(j,i)=A2(j,i)-k*A2(4,i);
        C2(j,1)=C2(j,1)-k*C2(4,1);
    end
end
for j=6:1:6
    k=A2(j,5)/A2(5,5);
    for i=5:1:6
        A2(j,i)=A2(j,i)-k*A2(5,i);
        C2(j,1)=C2(j,1)-k*C2(5,1);
    end
end
