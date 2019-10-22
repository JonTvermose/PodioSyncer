import * as React from 'react';
import { Route, Link, BrowserRouter as Router, Switch, useHistory } from "react-router-dom";
import './app.css';
import './custom.scss';
import { PodioApps } from "./pages/podioapps";
import { CreatePodioApp } from "./pages/createpodioapp";


const App: React.FC = () => {

    const createPodioApp = () => {
        return (<CreatePodioApp onCreated={() => alert("Created")} onCancel={() =>alert("Cancelled")} />);
    }

    return (
        <Router>
            <div className="App">
                <Switch>
                    <Route exact path="/" component={PodioApps} />
                    <Route exact path="/create" component={createPodioApp} />
                </Switch>

            </div>
        </Router>
    );
}

export default App;
