f=3.0e8;um0=4.0*pi*1.0e-7;ep0=1.0e-9/(36.0*pi);
bs0=2.0*pi;
bc0=1.0;a0=0.2;
q0=bs0*a0;
r0=200.0*bc0;qr0=bs0*r0;

nx=181;
delt=pi/(nx-1);
for m=1:nx;
   
    t=(m-1)*delt;
   
    st=0;sp=0;
for n=1;20;
    jn0=sqrt(pi/q0/2)*besselj(n+0.5,q0);
    hn0=sqrt(pi/q0/2)*besselh(n+0.5,1,q0);
    jn01=sqrt(pi/q0/2)*(besselj(n-0.5,q0)-(n+1)*besselj(n+0.5,q0)/q0);
    djn0=jn0+q0*jn01;
     hn01=sqrt(pi/q0/2)*(besselh(n-0.5,1,q0)-(n+1)*besselh(n+0.5,1,q0)/q0);
     dhn0=hn0+q0*hn01;
     
     cn=(2*n+1)*(-1i)^n/(n*(n+1));
     an=-jn0*cn/hn0;
     bn=-1i*djn0*cn/dhn0;

     c=legendre(n,cos(t));
     if n==1
         c3=0;
     else
         c3=c(3);
     end
     pn1=c(2)/sin(t);
     pn2=c3+cos(t)*c(2)/sin(t);
     
     st=st+(an*pn1+1i*bn*pn2)*(-1i)^(n+1);
     sp=sp+(an*pn2+1i*bn*pn1)*(-1i)^(n+1);
end
rcst(m)=10*log10(4.0*pi*r0*r0*abs(st*cos(0)/qr0)^2);
rcsx(m)=10*log10(4.0*pi*r0*r0*abs(st*cos(0)/qr0)^2)+0.1;
rcsy(m)=10*log10(4.0*pi*r0*r0*abs(sp*sin(pi/2)/qr0)^2)+0.1;
rcsp(m)=10*log10(4.0*pi*r0*r0*abs(sp*sin(pi/2)/qr0)^2);
end
plot(rcst(1:180),'b','LineWidth',2);hold on;
plot(rcsp(1:180),'r','LineWidth',2);hold on;
plot(rcsx(1:180),'m','LineWidth',2);hold on;
plot(rcsy(1:180),'c','LineWidth',2);hold on;

 %m=0.25:0.25:180;
%n=1.5:0.25:180;
%rcsm=rcs524newball1(1:144,1:5)';
  % rcsm=reshape(rcsm,720,1);
%rcsn=rcs524newball2(146:288,1:5)';
 %   rcsn=reshape(rcsn,715,1);
  % plot(m,rcsm,'m','LineWidth',2); hold on;
%plot(n,rcsn,'c','LineWidth',2); hold on;
    %  rcsx=rcs008x1(1:36,1:5)';
   %rcsx=reshape(rcsx,180,1);
%rcsy=rcs008y1(38:72,1:5)';
 %   rcsy=reshape(rcsy,175,1);
  %  plot(rcsx,'-.k','LineWidth',2); hold on;
   %plot(rcsy,':k','LineWidth',2); hold on;


% Create xlabel
xlabel('\theta (Degrees)','FontSize',11);

% Create ylabel
ylabel('Bistatic Radar Cross Section (dB)','FontSize',11);


 h=legend('VV, Exact','HH, Exact','VV, MoM ','HH, MoM',0);