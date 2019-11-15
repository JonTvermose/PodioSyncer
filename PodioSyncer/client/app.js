import * as React from 'react';
import { Route, BrowserRouter as Router, Switch } from "react-router-dom";
import './app.css';
import './custom.scss';
import { PodioApps } from "./pages/podioapps";
import { CreatePodioApp } from "./pages/createpodioapp";
var App = function () {
    var createPodioApp = function () {
        return (React.createElement(CreatePodioApp, { onCreated: function () { return alert("Created"); }, onCancel: function () { return alert("Cancelled"); } }));
    };
    return (React.createElement(Router, null,
        React.createElement("div", { className: "App" },
            React.createElement(Switch, null,
                React.createElement(Route, { exact: true, path: "/", component: PodioApps }),
                React.createElement(Route, { exact: true, path: "/create", component: createPodioApp })))));
};
export default App;
