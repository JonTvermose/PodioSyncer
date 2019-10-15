import * as React from 'react';
import './app.css';
import './custom.scss';
import { PodioApps } from "./pages/podioapps";
var App = function () {
    return (React.createElement("div", { className: "App" },
        React.createElement(PodioApps, null)));
};
export default App;
