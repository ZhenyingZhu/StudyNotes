## Explaination

Map:
- Key: file location
- Value: file content

Reduce:
- Key, value: OutputCollector returned by Map, which values are already sorted, and only for this key.


```
void Map::map(Key key, Value value, OutputCollector<ReduceKey, ReduceValue> output);

void Reduce::reduce(ReduceKey key, Iterator<ReduceValue> values, OutputCollector<OutputKey, OutputValue> output);
```

## Examples

### Word Count
http://www.jiuzhang.com/solutions/word-count/

http://www.jiuzhang.com/solutions/inverted-index-map-reduce/

http://www.jiuzhang.com/solutions/anagram-map-reduce/

