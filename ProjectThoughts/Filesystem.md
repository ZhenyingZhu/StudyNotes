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
- TODO: save versions of a file? or just use backup softwares?

## DB
File table:
- id: 6 digits. Enough for 1,000,000 files.
- path: should be compatiable with windows and linux. should encode in utf8. should be relative path. Something similar to `file:///E:/Documents/`
- description: a short description, and better with a link to a onenote link (need cross reference). Need support utf8. Might need able to search in description?
- hard copy location: the place where the origin paper stored
- create time + file size to track file rename?
- will it needs to track file move? it should not because there is no multi-level struct?


Tag table:
- id
- tag name

File tag relation
- file id
- tag id
- primary key is file + tag id

## What to write in description
tag should be enough? just a link to oneonte?

## Control
- a UI to write to db?
- a UI to easy search?
