
public class FourMin {
	public static void main(String[] args){
		String str=null;
		String[] alp={"a","b","c","d","e","f","g"};
		for(int a=0;a<7;a++){
			for(int b=a+1;b<7;b++){
				for(int c=b+1;c<7;c++){
					for(int d=c+1;d<7;d++){
						String mint=alp[a]+" and "+alp[b]+" and "+alp[c]+" and "+alp[d];
						str=str+" or "+mint;
					}
				}
			}
		}
		System.out.println(str);
	}	

}
