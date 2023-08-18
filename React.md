# React

## Resources

**TODO**: [Babel Text editor](https://babeljs.io/docs/en/editors/)

[Practice](https://reactjs.org/tutorial/tutorial.html)

[Develop tool](https://github.com/facebook/react/tree/master/packages/react-devtools)

- `npm install -g react-devtools`

- Components: split UI into indipendent, reusable and isolated parts.
- Component can either be a JS function, or a ES6 class extends `React.Component`, which has a `render()` method.
- Input: Props; Output: react elements.

[React Hands on](https://reactjs.org/tutorial/tutorial.html)

## Official Documents

<https://react.dev/>

<https://reactjs.org/docs/getting-started.html>

[React concepts](https://reactjs.org/docs/hello-world.html)

Smallest React component:

```javascript
ReactDOM.render(
  <h1>Hello, world!</h1>,
  document.getElementById('root')
);
```

[JSX](https://reactjs.org/docs/introducing-jsx.html)

- React element: `const name = "a"; const element = (<h1>Hello, {name}!</h1>);`
- In curly braces can be any JS expression.
- Parentheses are not necessary, but if need to split JSX over multiple lines, add parentheses can avoid auto semicolon insertion.
- JSX prevents Injection attack by letting React DOM escape any values embedded before rendering.
- Babel is the compiler. It compiles JSX HTML syntax down to `React.createElement()`.

[Rendering elements](https://reactjs.org/docs/rendering-elements.html)

- root DOM: `<div id="root"></div>`
- React elements are immutable. Once you create an element, you can’t change its children or attributes.
- it represents the UI at a certain point in time.

[Components](https://reactjs.org/docs/components-and-props.html)

- Components are like JS functions. input props (the attribute), output React elements.
- element can represent both DOM tags (like div) and user-defined component (`const element = <Welcome name="a" />;`)
- Component name must start with Upper case
- A component can have components inside it. Typically, new React apps have a single `App` component at the very top.
- props are read-only

Function component

```javascript
function Welcome(props) {
  return <h1>Hello {props.name}</h1>;
}
```

ES6 class component, which is equivalent to the function component.

```javascript
class Welcome extends React.Component {
  render() {
    return <h1>Hello {this.props.name}</h1>;
  }
}
```

How props pass down

```javascript
function Component(props) {
  return (
    <div className="UserInfo">
      <Avatar user={props.author} />
    </div>
  );
}

function Avatar(props) {
  return (
    <img className="Avatar"
      src={props.user.avatarUrl}
      alt={props.user.name}
    />
  );
}
```

[State](https://reactjs.org/docs/state-and-lifecycle.html)

- State is similar to props, but it is private and fully controlled by the component.
- The class that inherits `React.Component` has a method `render()`. It will be called each time an update happens.
- In the class, properties `this.props` and `this.state` are defined.
- it’s very important to free up resources taken by the components when they are destroyed.
- mounting: whenever a Component is rendered to the DOM for the first time, set up a resource like a timer.
- unmounting: clear resources when a Component is removed.
- lifecycle methods: deal with mounting and unmounting. `componentDidMount()` and `componentWillUnmount()`
- State can only be directly updated in the construtor. Other updates should call `setState()`.
- State updates may be asynchronous because React batch multiple `setState()` calls into a single update.
- So use `setState(func)` to set state if the next state needs to be calculated based on the current state and props.
- Don't need to set all the states fields at the same time.
- Can pass down the props and lower level components won't know where does the props pass in. It is called a “top-down” or “unidirectional” data flow.

```javascript
class Clock extends React.Component {
  constructor(props) { // this is always required, even it props is not used.
    super(props);
    this.state = {date: new Date();};
  }

  componentDidMount() {
    this.timerID = setInterval(
      () => this.tick(),
      1000
    );
  }

  componentWillUnmount() {
    clearInterval(this.TimerID);
  }

  tick() {
    this.SetState({
      date: new Date()
    });
  }

  render() {
    return (
      <div>
      <h2>It is {this.state.date.toLocaleTimeString()}.</h2>
      </div>
    );
  }
}

React.DOM.render(
  <Clock />,
  document.getElementById('root')
);
```

Set next state use the current state and pros

```javascript
this.setState((state, props) => ({
  counter: this.state.counter + this.props.increment
}));
```

[Handling Events](https://reactjs.org/docs/handling-events.html)

- the events are synthetic events: cross-browser compatibility.
- In JSX callbacks, class methods are not bound to `this` by default. To bind them, need call `this.handleClick = this.handleClick.bind(this);`
- This is Javascript logic, `this.method` doesn't work unless bind it to `this`.

Handling event: the default behavior of this link is to open a new page, but `preventDefault()` stops it.

```javascript
function ActionLink() {
  function handleClick(e) {
    e.preventDefault();
    console.log('The link was clicked.');
  }

  return (
    <a href="#" onClick={handleClick}>
      Click me
    </a>
  );
}
```

Use ES6 class:

```javascript
class Toggle extends React.Component {
  constructor(props) {
    super(props);
    this.state = {isToggleOn: true};

    // This binding is necessary to make `this` work in the callback
    this.handleClick = this.handleClick.bind(this);
  }

  handleClick() {
    this.setState(state => ({
      isToggleOn: !state.isToggleOn
    }));
  }

  render() {
    return (
      <button onClick={this.handleClick}>
        {this.state.isToggleOn ? 'ON' : 'OFF'}
      </button>
    );
  }
}

ReactDOM.render(
  <Toggle />,
  document.getElementById('root')
);
```

Passing arguments to the event handler (two ways with same results, but the first one generate a different callback every time)

```javascript
<button onClick={(e) => this.deleteRow(id, e)}>Delete Row</button>
<button onClick={this.deleteRow.bind(this, id)}>Delete Row</button>
```

Return null of a Component can make it hide from rendering.

```javascript
function WarningBanner(props) {
  if (!props.warn) {
    return null;
  }

  return (
    <div className="warning">
      Warning!
    </div>
  );
}
```

[Lists and keys](https://reactjs.org/docs/lists-and-keys.html)

- Keys help React identify which items have changed, are added, or are removed.
- The best way to pick a key is to use a string that uniquely identifies a list item among its siblings.
- Keys only make sense in the context of the surrounding array.

Render a list of elements

```javascript
function NumberList(props) {
  const numbers = props.numbers;
  const listItems = numbers.map((number) =>
    <li key={number.toString()}>
      {number}
    </li>
  );
  return (
    <ul>{listItems}</ul>
  );
}

const numbers = [1, 2, 3, 4, 5];
ReactDOM.render(
  <NumberList numbers={numbers} />,
  document.getElementById('root')
);
```

controlled component

- the React component that renders a form also controls what happens in that form on subsequent user input.
- the react `state` is the source of truth.
- the controlled components `<input type="text">`, `<textarea>`, and `<select>` are reformatted in a similar way in React. [Examples](https://reactjs.org/docs/forms.html#controlled-components)
- `<input type="file" />` is an uncontrolled component.

A form

```javascript
class NameForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {textvalue: ''};

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event) {
    const target = event.target;
    const value = null;
    const name = target.name;
    // Use name to distinguish which controlled component is sending event when there are multiple inputs.
    if (target.name === "myText") {
      // every keystroke sends an event, and update the value.
      value = target.value;
    }
    // with square brace the name is converted from a string to a property name.
    this.setState({[name]: value});
  }

  handleSubmit(event) {
    alert('A name was submitted: ' + this.state.textvalue);
    event.preventDefault();
  }

  render() {
    return (
      <form onSubmit={this.handleSubmit}>
        <label>
          Name:
          <input name="myText" type="text" value={this.state.textvalue} onChange={this.handleChange} />
        </label>
        <input type="submit" value="Submit" />
      </form>
    );
  }
}
```

[Lifting State Up](https://reactjs.org/docs/lifting-state-up.html)

- sharing state is accomplished by moving it up to the closest common ancestor of the components that need it.

[Containment](https://reactjs.org/docs/composition-vs-inheritance.html)

- `props.children` is the sub nodes of the component: `<MyComponent><MyChild1 /><MyChild2 /></MyComponent>`

[A good example](https://reactjs.org/docs/thinking-in-react.html)

[Tester](https://codepen.io/pen?&editable=true&editors=0010)
