import java.util.ArrayList;
import background.*;

public class Test {
	public static void main(String[] args) {
		/*
		 * String subject = args[0] + ".txt"; ArrayList<String> artList = new
		 * ArrayList<String>(); for (int i = 1; i < args.length; i++) {
		 * artList.add(args[i] + "_List.txt"); new CreateList(args[i] + ".txt");
		 * } new MeaningList(subject, artList);
		 */

		ArrayList<String> results = new ArrayList<String>();
		char[] w = { 'f', 'i', 'a', 'r', 'a', 's' };
		char[] res = new char[6];
		for (int a = 0; a < 6; a++) {
			res[0] = w[a];
			for (int b = 0; b < 6; b++) {
				if (b == a)
					continue;
				res[1] = w[b];
				for (int c = 0; c < 6; c++) {
					if (c == a || c == b)
						continue;
					res[2] = w[c];
					for (int d = 0; d < 6; d++) {
						if (d == a || d == b || d == c)
							continue;
						res[3] = w[d];
						for (int e = 0; e < 6; e++) {
							if (e == a || e == b || e == c || e == d)
								continue;
							res[4] = w[e];
							for (int f = 0; f < 6; f++) {
								if (f == a || f == b || f == c || f == d
										|| f == e)
									continue;
								res[5] = w[f];
								results.add(new String(res));
							}
						}
					}
				}
			}
		}

		/*
		 * for (int a = 0; a < 7; a++) { res[0] = w[a]; for (int b = 0; b < 7;
		 * b++) { if (b == a) continue; res[1] = w[b]; for (int c = 0; c < 7;
		 * c++) { if (c == a || c == b) continue; res[2] = w[c]; for (int d = 0;
		 * d < 7; d++) { if (d == a || d == b || d == c) continue; res[3] =
		 * w[d]; for (int e = 0; e < 7; e++) { if (e == a || e == b || e == c ||
		 * e == d) continue; res[4] = w[e]; for (int f = 0; f < 7; f++) { if (f
		 * == a || f == b || f == c || f == d || f == e) continue; res[5] =
		 * w[f]; for (int g = 0; g < 7; g++) { if (g == a || g == b || g == c ||
		 * g == d || g == e || g == f) continue; res[6] = w[g]; results.add(new
		 * String(res)); } } } } } } }
		 */

		/*
		 * for (int a = 0; a < 8; a++) { res[0] = w[a]; for (int b = 0; b < 8;
		 * b++) { if (b == a) continue; res[1] = w[b]; for (int c = 0; c < 8;
		 * c++) { if (c == a || c == b) continue; res[2] = w[c]; for (int d = 0;
		 * d < 8; d++) { if (d == a || d == b || d == c) continue; res[3] =
		 * w[d]; for (int e = 0; e < 8; e++) { if (e == a || e == b || e == c ||
		 * e == d) continue; res[4] = w[e]; for (int f = 0; f < 8; f++) { if (f
		 * == a || f == b || f == c || f == d || f == e) continue; res[5] =
		 * w[f]; for (int g = 0; g < 8; g++) { if (g == a || g == b || g == c ||
		 * g == d || g == e || g == f) continue; res[6] = w[g]; for (int h = 0;
		 * h < 8; h++) { if (h == a || h == b || h == c || h == d || h == e || h
		 * == f || h==g) continue; res[7] = w[h]; results.add(new String(res));
		 * } } } } } } } }
		 */

		new MeaningList(results);
		System.out.println(results.size());
	}

}
