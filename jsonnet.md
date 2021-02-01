# Jsonnet

## Resource

[Tutorial](https://jsonnet.org/learning/tutorial.html)

## Tutorial

### Syntax

Field start with no quote. But if there is a space, then need to put a quote.

### Variable

- Normal case: `local house_rum = 'Banks Rum';`
- next to a field: `local pour = 1.5, Daiquiri: {...}`

### References

- `self`: current object. `local xyz = self` to save this object into var `xyz`
- `$`: outer-most object
- `['foo']`: looks up a field. `$['foo']` means find the `foo` field in the whole object.
- `.f`: the field name is an id. So `self.f` means `f` field in the same object.

### Arithmetic

- strings can be compared with `<`
- `obj: { a: 1, b: 2 } + { b: 3, c: 4 },` makes `"obj": {"a": 1, "b": 3, "c": 4}`

### Functions

- `local my_function(x, y=10) = x + y;`
- Can use functions in [std](https://jsonnet.org/ref/stdlib.html)
- Notice the function statement is not placed in `{}`. Like python the multi-line func is actually using indent. The `{}` is actually the output.

### Conditionals

`local factor = if large then 2 else 1;`

### Computed Field Names

- self or object locals cannot be accessed when field names are being computed, since the object is not yet constructed.
- If a field name evaluates to null during object construction, the field is omitted. This works nicely with the default false branch of a conditional

### Array and Object Comprehension

- `local arr = std.range(5, 8);`
- `higher: [x + 3 for x in arr],`

### Imports

**HERE**: continue
