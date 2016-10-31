
# [Source]: http://www.lintcode.com/en/problem/url-parser/

import re

html_page = """
<html>
  <body>
    <div>
      <a href="http://www.google.com" class="text-lg">Google</a>
      <a href="http://www.facebook.com" style="display:none">Facebook</a>
    </div>
    <div>
      <a href="https://www.linkedin.com">Linkedin</a>
      <a href =  "http://github.io">LintCode</a>
    </div>
  </body>
</html>
"""

class HtmlParser:
    # @param {string} content source code
    # @return {string[]} a list of links
    def parseUrls(self, content):
        # re.findall: return all match pattern from left to right
        # r or R prefix: valid escape sequences will not be converted
        # \s: any non-whitespace char
        # *: 0 or more repetition of prev
        # (?iLmsux): set flags like i means re.I for the comming string. here means href ignore case
        # 
        # re.I: ignore case
        links = re.findall(r'\s*(?i)href\s*=\s*"?\'?([^"\'>\s]*)', content, re.I)

        return links


parser = HtmlParser()
urls = parser.parseUrls(html_page)
for url in urls:
    print url
