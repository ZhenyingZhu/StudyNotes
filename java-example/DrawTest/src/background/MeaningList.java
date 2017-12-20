package background;

import java.io.*;
import java.util.*;

public class MeaningList {
	ArrayList<String> artList = new ArrayList<String>();
	String subjectName = new String();
	private HashMap<String, Integer> wordMap = new HashMap<String, Integer>();

	public MeaningList(ArrayList<String> words) {
	//public MeaningList(String subjectName, ArrayList<String> artList) {
		/*this.subjectName = subjectName;
		this.artList = artList;
		for (int i = 0; i < artList.size(); i++) {
			this.readList(artList.get(i));
		}*/
		subjectName="test.txt";
		this.readList(words);
		writeFile(subjectName);
	}

	// Combine lists from articles.
	public void readList(ArrayList<String> words) {
		for(int i=0; i<words.size(); i++){
			wordMap.put(words.get(i), 1);
		}
		
	//public void readList(String filePath) {
		/*File file = new File(filePath);
		BufferedReader reader = null;
		try {
			reader = new BufferedReader(new FileReader(file));
			String line;
			while ((line = reader.readLine()) != null) {
				String[] words = line.split(" ");
				String tempWord = words[0];
				int freq = Integer.parseInt(words[1]);
				if (wordMap.containsKey(tempWord)) {
					wordMap.put(tempWord, wordMap.get(tempWord).intValue()
							+ freq);
				} else {
					wordMap.put(tempWord, freq);
				}
			}
			reader.close();
		} catch (IOException e) {
			e.printStackTrace();
		}*/
	}

	// Write to a subject list.
	public void writeFile(String destination) {
		try {
			File file = new File(destination);
			if (file.exists()) {
				System.out.println("update");
			} else {
				if (!file.createNewFile()) {
					System.out.println("cannot create");
				}
			}
			BufferedWriter writeLine = new BufferedWriter(new FileWriter(file));
			SearchMean sm = new SearchMean();
			Set<String> set = wordMap.keySet();
			int indicator = 0;
			for (String s : set) {
				System.out.println(++indicator);
				writeLine.write(indicator+" "+s + " " + wordMap.get(s).intValue() + " ");
				writeLine.newLine();
				ArrayList<String[]> classExplain = sm.searchWord(s);
				for (int i = 0; i < classExplain.size(); i++) {
					writeLine.write(": \t\t" + classExplain.get(i)[0] + "\t"
							+ classExplain.get(i)[1]);
					writeLine.newLine();
				}
				writeLine.flush();
			}
			writeLine.close();
			System.out.println("!!!Done!!!");
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
}
