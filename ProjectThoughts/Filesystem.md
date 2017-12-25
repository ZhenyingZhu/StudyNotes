# Purpose
Arrange documents.

# Problems to solve
- some files belong to multiple categories, and one folder cannot hold them
- hard to find some files
- backup copies create duplicates but cannot tell what
- file names exceed windows limit so need to be change

# design

## Folder struct
- don't create a lot of levels of folders, but still good to have folders
- single file should use file id and a d as the prefix of name, e.g. `000000_d_myfile.txt`. extension name is needed. filename might also need, so that when db is not available, there is still way to find.
- folder name should be `000000_f_myfolder`, so that when the program traverse folders it can stop go deeper.
- first store files that are not often change
- tag should be all lower case, and no special character, except chinese or japanese.
- TODO: save versions of a file? or just use backup softwares?


## DB
Files table:
- id: 6 digits. Enough for 1,000,000 files.
- path: should be compatiable with windows and linux. should encode in utf8. should be relative path. Something similar to `file:///E:/Documents/`
- name: file name. maybe it is not necessary. can be same for two files, since the real filename has the id.
- description: a short description, and better with a link to a onenote link (need cross reference). Need support utf8. Might need able to search in description?
- hardcopy: the place where the origin paper stored
- create time: TODO
- file size: TODO combine with create time to track file rename?
- will it needs to track file move? it should not because there is no multi-level struct?

Tag table:
- id
- name

TagRel table:
- parent tag id
- child tag id
- primary key is the combination

FileTag table:
- file id
- tag id
- primary key is file + tag id

## What to write in description
tag should be enough? just a link to oneonte?

# UI
## Write
- passing a file, with text fields, store it in the db correctly.
- can see similiar files in the db.

## Search
- a list of tags on the screen
- a search bar
- show the result in list with details
- open the file
