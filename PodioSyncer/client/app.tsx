import * as React from 'react';
import { Route, Link, BrowserRouter as Router, Switch, useHistory } from "react-router-dom";
import './app.css';
import './custom.scss';
import { PodioApps } from "./pages/podioapps";
import { CreatePodioApp } from "./pages/createpodioapp";
import { SyncEvents } from "./pages/syncevents";



const App: React.FC = () => {

    return (
        <Router>
            <header>
                <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                    <div className="container">
                        <a className="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">PodioSyncer</a>
                        <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                            aria-expanded="false" aria-label="Toggle navigation">
                            <span className="navbar-toggler-icon"></span>
                        </button>
                        <div className="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
                            <ul className="navbar-nav flex-grow-1">
                                <li className="nav-item">
                                    <Link to="/" className="nav-link text-dark">Home</Link>
                                </li>
                                <li className="nav-item">
                                    <Link to="/syncevents" className="nav-link text-dark">Sync Events</Link>
                                </li>
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>
            <div className="container">
                <main role="main" className="pb-3">
                    <div className="App">
                        <Switch>
                            <Route exact path="/" component={PodioApps} />
                            <Route exact path="/create" component={CreatePodioApp} />
                            <Route exact path="/syncevents" component={SyncEvents} />
                        </Switch>
                    </div>
            </main>
            </div>

        </Router>
    );
}

export default App;
