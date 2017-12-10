USE taggedfs;

SELECT file.id, file.name
FROM file, filetag, tag
WHERE filetag.file=file.id
AND filetag.tag=tag.id
AND (tag.name="diploma")
GROUP BY file.id
ORDER BY file.id
