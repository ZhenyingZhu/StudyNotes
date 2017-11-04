USE taggedfs;

SELECT file.name, tag.name
FROM file, filetag, tag
WHERE filetag.file=file.id
AND filetag.tag=tag.id
AND tag.name="h1b"
