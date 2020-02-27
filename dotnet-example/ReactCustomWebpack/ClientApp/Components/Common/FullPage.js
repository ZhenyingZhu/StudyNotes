import React, {Component} from 'react';

import {BrowserRouter as Router} from 'react-router-dom';
import {Route, Switch} from 'react-router-dom';

import Routes from '../../Routes'

class FullPage extends Component {
    render() {
        return (
            <div>
                <h2>Page Top</h2>
                {/* zhenying: need use curly braces to wrap comment. */}
                <Routes />
            </div>
        );
    }
}

FullPage.defaultProps = {};

export default FullPage;