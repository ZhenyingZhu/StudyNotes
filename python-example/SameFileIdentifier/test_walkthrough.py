#!/usr/bin/env python

import os
import hashlib


def get_md5(file_path):
    hash_md5 = hashlib.md5()
    with open(file_path, "rb") as f:
        for chunk in iter(lambda: f.read(4096), b""):
            hash_md5.update(chunk)
    return hash_md5.hexdigest()


def generate_map(target_dir):
    md5_to_path = {}
    for (dir_path, dir_names, file_names) in os.walk(target_dir):
        for file_name in file_names:
            file_path = os.sep.join([dir_path, file_name])
            file_md5 = get_md5(file_path)

            if file_md5 not in md5_to_path:
                md5_to_path[file_md5] = []
            md5_to_path[file_md5].append(file_path)

    return md5_to_path


def promote_delete(path_list):
    for idx, path in enumerate(path_list):
        print(idx, path)

    keep_idx = int(input('Keep which file?'))
    confirm = input('Keep ' + path_list[keep_idx])
    if confirm == 'Y':
        for idx, path in enumerate(path_list):
            if idx != keep_idx:
                print('Removing ' + path)
                os.remove(path)


def main():
    home = os.path.expanduser('~')
    path = os.path.join(home, 'Downloads', 'Compare')

    md5_to_path = generate_map(path)
    for file_md5 in md5_to_path.keys():
        if len(md5_to_path[file_md5]) > 1:
            promote_delete(md5_to_path[file_md5])


if __name__ == '__main__':
  main()