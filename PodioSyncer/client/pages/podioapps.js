import React, { useState, useEffect } from 'react';
import { PodioRow } from "../components/podio-row/podio-row";
export var PodioApps = function (podioProps) {
    var _a = useState([]), podioApps = _a[0], setPodioApps = _a[1];
    useEffect(function () {
        fetch(jsonRoutes["getpodioapps"])
            .then(function (res) { return res.json(); })
            .then(function (res) { return setPodioApps(res); });
    }, []);
    var handleOnEdit = function (appId) {
        console.log("Edit clicked for Podio App Id: " + appId);
    };
    var handleOnDelete = function (appId) {
        console.log("Delete clicked for Podio App Id: " + appId);
    };
    return (React.createElement("div", { className: "container" },
        React.createElement("h3", null, "List of podio apps"),
        podioApps.map(function (app, index) {
            return React.createElement(PodioRow, { key: index, appId: app.podioAppId, name: app.name, verified: app.verified, status: app.active, webhookUrl: "", onEdit: handleOnEdit, onDelete: handleOnDelete });
        })));
};
