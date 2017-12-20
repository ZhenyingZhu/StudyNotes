class Question {
	public static void main(String[] args) {
		double price=26.9;
		double taxP=0.15;
		double fin=(double)(Math.round(price*taxP*100)/100.0);
		System.out.println(fin);
	}
}