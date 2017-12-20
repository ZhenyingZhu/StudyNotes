n=[1:10000]; % 每个数字 1000 个采样点表示 
d0=sin(0.7217*n)+sin(1.0247*n); % 对应行频列频叠加 
space=zeros(1,100); %100 个 0 模拟静音信号 
global NUM 
phone=[NUM,d0]; 
NUM=[phone,space]; % 存储连续的拨号音信号 
wavplay(d0,8192); % 产生拨号音