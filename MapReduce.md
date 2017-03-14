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

### Simple example
http://www.jiuzhang.com/solutions/word-count/

http://www.jiuzhang.com/solutions/inverted-index-map-reduce/

http://www.jiuzhang.com/solutions/anagram-map-reduce/

### BFS
http://www.johnandcailin.com/blog/cailin/breadth-first-graph-search-using-iterative-map-reduce-algorithm

```
void map(int nodeID, string nodeStruct, OutputCollector<int, string> output) {
    Node node = parseStruct(nodeStruct); // edges|dist|status
    if (node.status == visiting) {
        for (int neiID : node.edges) {
            Node nei = {NULL, node.dist + 1, visiting};
            output.collect(neiID, nei.to_string());
        }
        node.status= visited;
    }
    output.collect(nodeID, node.to_string());
}

void reducer(int nodeID, Iterator<string> nodeStructs, OutputCollector<int, string> output) {
    Node node;
    for (string structStr : nodeStructs) {
        Node nodeStruct = parseStruct(structStr);
        if (nodeStruct.edges != NULL)
            node.edges = nodeStruct.edges;
        if (nodeStruct.dist < node.dist) // default is INT_MAX
            node.dist = nodeStruct.dist;
        if (nodeStruct.status > node.status) // visited > visiting > none
            node.status = nodeStruct.status;
    }
    output.collect(nodeID, node.to_string());
}
```
