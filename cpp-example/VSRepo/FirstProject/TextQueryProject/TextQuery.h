#pragma once

#include <memory>
#include <string>
#include <vector>
#include <map>
#include <set>
#include <fstream>
#include "QueryResult.h"

/* this version of the query classes includes two
* members not covered in the book:
*   cleanup_str: which removes punctuation and
*                converst all text to lowercase
*   display_map: a debugging routine that will print the contents
*                of the lookup mape
*/

class QueryResult; // declaration needed for return type in the query function

class TextQuery {
public:
	typedef std::vector<std::string>::size_type line_no;

	TextQuery(std::ifstream&);

	QueryResult query(const std::string&) const;

	void display_map();        // debugging aid: print the map
private:
	std::shared_ptr<std::vector<std::string>> file; // input file
													// maps each word to the set of the lines in which that word appears
	
	std::map<std::string, std::shared_ptr<std::set<line_no>>> wm;

	// canonicalizes text: removes punctuation and makes everything lower case
	static std::string cleanup_str(const std::string&);
};
