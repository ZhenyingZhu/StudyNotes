package background;

import java.net.*;
import java.util.*;
import java.io.*;

public class SearchMean {
	private boolean ifDndataTrue = false;

	public ArrayList<String[]> searchWord(String sWord) {
		ArrayList<String[]> result = this.getMean(this.getHTML(sWord));
		if (result.size() == 0) {
			String[] nilString = { "null", "null" };
			result.add(nilString);
		}
		return result;
	}

	// Get the meaning from dictionary.reference.com in HTML
	// If not a word, return "Not found"
	public StringBuilder getHTML(String targetWord) {
		String encode = "utf-8";
		String pageURL = "http://dictionary.reference.com/browse/" + targetWord
				+ "?s=t";
		StringBuilder pageContent = new StringBuilder();
		try {
			URL url = new URL(pageURL);
			HttpURLConnection connection = (HttpURLConnection) url
					.openConnection();
			connection.setRequestProperty("User-Agent", "MSIE 7.0");
			BufferedReader br = new BufferedReader(new InputStreamReader(
					connection.getInputStream(), encode));
			String line = null;
			while ((line = br.readLine()) != null) {
				if (line.equals("<div class=\"lunatext results_content frstluna\">"))
					break;
			}
			if (line == null) {
				pageContent.append("Not found");
				return pageContent;
			}
			while ((line = br.readLine()) != null) {
				if (line.equals("</div>"))
					break;
				pageContent.append(line);
				pageContent.append("\r\n");
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return pageContent;
	}

	public ArrayList<String[]> getMean(StringBuilder pageContent) {
		ArrayList<String[]> wordMean = new ArrayList<String[]>();
		if (pageContent.toString().equals("Not found")) {
			String[] meaning = { "null", "null" };
			wordMean.add(meaning);
			return wordMean;
		}
		String[] tr = pageContent.toString().split(" ");
		int pointer = 0;
		while (pointer < tr.length) {
			if (tr[pointer].equals("class=\"pbk\">")) {
				StringBuffer aMeaning = new StringBuffer();
				while (!tr[pointer + 1].equals("</div></div><div")) {
					if (pointer == tr.length - 2)
						break;
					pointer++;
					aMeaning.append(tr[pointer] + " ");
				}
				aMeaning.append("</div></div>");
				// get a String tuple with class and meaning.
				String[] meaning = this.decodeHTML(aMeaning);
				wordMean.add(meaning);
			}
			pointer++;
		}
		return wordMean;
	}

	public String[] decodeHTML(StringBuffer formatHTML) {
		String[] tuple = new String[2];
		StringBuffer wordClass = new StringBuffer();
		StringBuffer wordMeaning = new StringBuffer();
		int current = 17;
		// Get word class
		while (formatHTML.charAt(current) != ',') {
			if (formatHTML.charAt(current) == '<')
				break;
			wordClass.append(formatHTML.charAt(current));
			current++;
		}
		// Get explanation
		while (current < formatHTML.length()) {
			if (formatHTML.charAt(current) == 'd') {
				current = this.ifDndata(formatHTML, current);
				// grab only one explain.
				if (ifDndataTrue) {
					while (current < formatHTML.length()) {
						if (formatHTML.charAt(current) == ':')
							break;
						if (formatHTML.charAt(current) == '\r')
							break;
						if (formatHTML.charAt(current) == '\n')
							break;
						if (formatHTML.charAt(current) == '<') {
							while (current < formatHTML.length()) {
								if (formatHTML.charAt(current) == '>') {
									break;
								}
								current++;
							}
						}
						if (formatHTML.charAt(current) != '>') {
							wordMeaning.append(formatHTML.charAt(current));
						}
						current++;
					}
					ifDndataTrue = false;
				}
			}
			current++;
		}
		tuple[0] = wordClass.toString();
		tuple[1] = wordMeaning.toString();
		for (int i = 0; i < tuple.length; i++) {
			if (tuple[i] == null) {
				tuple[i] = "null";
			}
		}
		return tuple;

	}

	// The explanation is append after <"dndata">
	public int ifDndata(StringBuffer sb, int pointer) {
		pointer++;
		if (sb.charAt(pointer) == 'n') {
			pointer++;
			if (sb.charAt(pointer) == 'd') {
				pointer++;
				if (sb.charAt(pointer) == 'a') {
					pointer++;
					if (sb.charAt(pointer) == 't') {
						pointer++;
						if (sb.charAt(pointer) == 'a') {
							pointer += 3;
							ifDndataTrue = true;
						}
					}
				}
			}
		}
		return pointer;
	}
}
