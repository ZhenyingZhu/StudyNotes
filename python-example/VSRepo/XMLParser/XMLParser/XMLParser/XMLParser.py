import xml.etree.ElementTree

xmlfile = r'C:\Users\Me\example.xml'
namespace = '{http://somenamespace}'

e = xml.etree.ElementTree.parse(xmlfile).getroot()

for level_one_object in e.findall('.//' + namespace + 'level_one_object'):
    for level_two_object in level_one_object.findall('.//' + namespace + 'level_two_object'):
        for level_three_object in level_two_object.findall('.//' + namespace + 'level_three_object'):
            print(level_one_object.get('value_key'))
            level_three_object = int(level_three_object.text) # this is a number
            print("{0:b}".format(level_three_object)) # show binary of a number
            
