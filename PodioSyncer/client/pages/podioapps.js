import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
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
        React.createElement("h3", null,
            "List of podio apps ",
            React.createElement(Link, { to: "/create" },
                React.createElement("button", { className: "btn btn-primary float-right" }, "Create new"))),
        React.createElement(PodioRow, { rows: podioApps, onEdit: handleOnEdit, onDelete: handleOnDelete })));
};
