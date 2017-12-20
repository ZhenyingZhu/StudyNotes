n=1:256;%输入信号取值
wn=0.2;%滤波器截止频率
N=256;%采样点
Nf=64;%滤波器阶数
m=4;%滤波器分组

x=sin(pi*n/5)+sin(pi*n/1.5);%输入信号
X=abs(fft(x,N));
figure(1);
subplot(2,1,1);plot(x);
title('输入信号时域波形');
subplot(2,1,2);stem(X);
title('输入信号频域波形')

xx=reshape(x,4,64);
x1=xx(1,:);
x2=xx(2,:);
x3=xx(3,:);
x4=xx(4,:);
figure(2);
subplot(2,2,1);plot(x1);
title('第一路输入信号');
subplot(2,2,2);plot(x2);
title('第二路输入信号');
subplot(2,2,3);plot(x3);
title('第三路输入信号');
subplot(2,2,4);plot(x4);
title('第四路输入信号');

hn=fir1(Nf-1,wn,'low');%N-1为滤波器阶数
hh=reshape(hn,m,16);
h1=hh(1,:);
h2=hh(2,:);
h3=hh(3,:);
h4=hh(4,:);
figure(3);
freqz(hn);
title('滤波器的频域特性');

%每个x和每个h都卷积得A（i，j）
A=zeros(1000);
xxx=[x1;x2;x3;x4];
hhh=[h1;h2;h3;h4];

for i=1:1:4;
    for k=1:1:4;
        A(i,k)=conv(xxx(i,:),hhh(k,:));
    end;
end;

%i+k相同的结果相加得yy
for l=2:1:8;
    for i=1:1:4;
        for k=1:1:4;
           if i+k==l;
               y(l-1)=sum(A(i,k),2);
           end;
        end;
    end;
end;


figure(3);
subplot(2,1,1);plot(y);
subplot(2,1,2);plot(abs(fft(y,128)));
   
