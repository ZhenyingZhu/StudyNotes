# JSON

## Source

<http://www.json.org/>

JavaScript.md

## Concept

- Value: 2 structured types: Object, Array, 4 primitive types: String, Number, Boolean, Null
- Object: like hash map `{key1:value,key2:value}`
- Array: `[value1, value2]`
- String: wrapped in double quotes, using backslash escapesi
- Number
- Boolean
- Null

## Convention

[Google](https://google.github.io/styleguide/jsoncstyleguide.xml)

[JSON-RPC 2.0](http://www.jsonrpc.org/specification#response_object)

Request Object (from client to server) memebers: (key and value pairs):

- always has a member named "jsonrpc" with a String value of "2.0"
- method: A String containing the name of the method to be invoked. "rpc,xxx" are rpc-internal methods.
- params: parameter values
- id: An identifier established by the Client. If it is not included it is assumed to be a notification. The Server MUST reply with the same value in the Response object if included.

Response object (from server to client) members:

- jsonrpc: "2.0"
- result: is REQUIRED on success, MUST NOT exist if there was an error.
- error: must be an Error Object. see below
- id: if error happens, MUST be Null.

Error object

- code: MUST be an integer. See [JSONRPC 2.0 error code](http://www.jsonrpc.org/specification#error_object)
- message: A String providing a short description of the error.
- data: contains additional information about the error.

## Batch

Content is **skiped**
