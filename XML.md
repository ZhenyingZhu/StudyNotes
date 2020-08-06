# XML

## Source

- [XML Wiki](https://en.wikipedia.org/wiki/XML)
- [XSD Wiki](https://en.wikipedia.org/wiki/XML_Schema_(W3C))

## XML Intro

[wikipedia](https://en.wikipedia.org/wiki/XML)

### Key terminology

- Processor: parser.
- Markup: To distinguish the syntax. `<>`, `&xxx;`.
- Tag: in `<>`. Start tag, end tag or empty-element tag.
- Element: a logical document component in between start and end tag.
- Attribute: within the tag. An XML attribute can only have a single value and each attribute can appear at most once on each element.
- XML declaration: `<?xml version="1.0" encoding="UTF-8"?>`
- Comment: `<!-- comment -->`

### Schema and Validation

- document type definition (DTD): an example of a schema or grammar.
- Schema: successor of DTDs. refer in `xs:schema` tag

### Programming interfaces

- Document Object Model (DOM): navigate the entire document as a tree.
- Data binding: binding of XML documents to a hierarchy of custom and strongly typed objects

## XSD

[wikipedia](https://en.wikipedia.org/wiki/XML_Schema_(W3C))

XSD was designed with the intent that determination of a document's validity would produce a collection of information adhering to specific data types. Such a post-validation infoset can be useful in the development of XML document processing software.

### Schema and schema documents

- schema: an abstract collection of metadata, consisting of a set of schema components.
- namespace: in `xs:schema`, define the current namespace in `targetNamespace`, use other namespaces like `xmlns:xs="http://www.w3.org/2001/XMLSchema"`. Import use `xs:import`.

### Schema components

- Element declarations: may be global or local
  - element name and target namespace
  - type of the element: constrains what attributes and children the element can have.
  - a substitution group of another element: the parent element rules apply to this element
  - integrity constraints: uniqueness, referential
- Attribute declarations:
  - name and target namespace
  - constrains values that the attribute may take
  - can define a default value or a fixed value
  - `use`: represents the relationship of a complex type and an attribute declaration, whether this attribute is required for the type
- element particle: represents the relationship of a complex type and an element declaration. like `minOccurs`
- Simple Type: constrain the textual values that may appear in an element or attribute
  - 19 primitive data types (anyURI, base64Binary, boolean, date, dateTime, decimal, double, duration, float, hexBinary, gDay, gMonth, gMonthDay, gYear, gYearMonth, NOTATION, QName, string, and time)
  - restriction: limit the value types. The type can be another simple or complex type. With `xs:enumeration`, can restrict the values as well.
  - list: allowing a sequence of values
  - union: allowing a choice of values from several types
- Complex Type: describe the permitted content of an element, including its element and text children and its attributes. consists of a set of attribute uses and a content model.

### Post-Schema-Validation Infoset

Rhe XML Schema data model includes:

- The vocabulary (element and attribute names)
- The content model (relationships and structure)
- The data types

`XmlSchemaFacet`: the xml element restrictions. See those facet: [XmlSchemaLengthFacet](https://docs.microsoft.com/en-us/dotnet/api/system.xml.schema.xmlschemalengthfacet?view=netcore-3.1), [XmlSchemaMaxExclusiveFacet](https://docs.microsoft.com/en-us/dotnet/api/system.xml.schema.xmlschemamaxexclusivefacet?view=netcore-3.1), etc.

## WSDL

[Wikipedia](https://en.wikipedia.org/wiki/Web_Services_Description_Language)

an XML-based interface description language that is used for describing the functionality offered by a web service.
