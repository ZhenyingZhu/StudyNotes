public class TestOr {
	public static boolean returnTrue() {
		System.out.println("First function to return true.");
		return true;
	}

	public static boolean returnFalse() {
		System.out.println("First function to return false.");
		return false;
	}

	public static void main(String[] args) {
		boolean para = false;
		if (para || returnTrue() || returnFalse()) {
			/*
			 * If the first parameter is true, the second parameter doesn't run.
			 */
			System.out.println("Finally return true.");
		}
		System.out.println("end");
	}
}
