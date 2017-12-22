class TryShift{
	public static void main(String[] args){
	/* If shift 32 times, it became the original one. 
	If the number is bigger than 0, >> always insert 0 at first. 
	If the number is smaller than 0, >> always insert 1 at first. 
	<< Always add 0 at last. 
	*/
		for(int i=32; i<65;i++){
			System.out.println(i+" "+Integer.toBinaryString((-5<<i))+" "+(-5<<i)); 
		}
	}
}