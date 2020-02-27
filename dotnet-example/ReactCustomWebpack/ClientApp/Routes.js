import React, {Component} from 'react';

import PropTypes from 'prop-types';
import {BrowserRouter as Router} from 'react-router-dom';
import {Route, Switch} from 'react-router-dom';

class Routes extends Component {
    render() {
        return (
            <Router>
                <div>
                    <Switch>
                        <Route exact path="/" render={() => (<h1>Home Page</h1>)} />
                        <Route exact path="/route1" render={() => (<h1>This is Route1</h1>)} />
                        <Route exact path="/route2" render={() => (<h1>This is Route2</h1>)} />
                        <Route render={() => (<h1>Route Not Found</h1>)} />
                    </Switch>
                </div>
            </Router>
        );
    }
}

Routes.defaultProps = {};

export default Routes;

