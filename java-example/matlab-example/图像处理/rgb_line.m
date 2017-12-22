%function plot_rgb_line(n)
%Image_rgb
x = imread('E:\Downloads\gromit.BMP');             %读入一个彩色图像；
%[width,height,component]=size(x);        %计算彩色图像的尺寸以及维数；
%y=zeros(width,height);           
xx=im2double(x);                    %将整形转变为双精度形；
%y=0.299*xx(:,:,1)+0.587*xx(:,:,2)+0.114*xx(:,:,3);  
%n=input('please input a number:');
R=xx(:,:,1);
G=xx(:,:,2);
B=xx(:,:,3); 

subplot(3,1,1);
plot(R(n,:));
subplot(3,1,2);
plot(G(n,:));
subplot(3,1,3);
plot(B(n,:));
%axis([0,800,0,1]);
