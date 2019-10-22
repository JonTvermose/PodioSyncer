import * as React from 'react';
import './app.css';
import './custom.scss';
import { PodioApps } from "./pages/podioapps";
import { CreatePodioApp } from "./pages/createpodioapp";
var App = function () {
    return (React.createElement("div", { className: "App" },
        React.createElement(PodioApps, null),
        React.createElement(CreatePodioApp, { onCreated: function () { return alert("Created"); }, onCancel: function () { return alert("Cancelled"); } })));
};
export default App;
