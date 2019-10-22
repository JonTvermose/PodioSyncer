import React, { useState } from "react";
import { LoadingSpinner } from "../components/loading-spinner";
export var CreatePodioApp = function (props) {
    var _a = useState(""), name = _a[0], setName = _a[1];
    var _b = useState(""), podioAppId = _b[0], setPodioAppId = _b[1];
    var _c = useState(""), podioAppToken = _c[0], setPodioAppToken = _c[1];
    var _d = useState(false), isLoading = _d[0], setIsLoading = _d[1];
    var handleSubmitClick = function (e) {
        e.preventDefault();
        setIsLoading(true);
        fetch(jsonRoutes["createpodioapp"], {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ name: name, podioAppId: podioAppId, appToken: podioAppToken })
        }).then(function (res) {
            setIsLoading(false);
            setName("");
            setPodioAppId("");
            setPodioAppToken("");
            props.onCreated();
        });
    };
    return (React.createElement("div", { className: "container" },
        React.createElement("h3", null, "Create podio app"),
        React.createElement("form", null,
            React.createElement("div", { className: "form-group" },
                React.createElement("label", null, "Name"),
                React.createElement("input", { type: "text", className: "form-control", value: name, onChange: function (e) { return setName(e.target.value); } }),
                React.createElement("small", { className: "form-text text-muted" }, "The name of the Podio App.")),
            React.createElement("div", { className: "form-group" },
                React.createElement("label", null, "Podio App Id"),
                React.createElement("input", { type: "text", className: "form-control", value: podioAppId, onChange: function (e) { return setPodioAppId(e.target.value); } }),
                React.createElement("small", { className: "form-text text-muted" }, "The Podio App Id.")),
            React.createElement("div", { className: "form-group" },
                React.createElement("label", null, "Podio Apptoken"),
                React.createElement("input", { type: "text", className: "form-control", value: podioAppToken, onChange: function (e) { return setPodioAppToken(e.target.value); } }),
                React.createElement("small", { className: "form-text text-muted" }, "The Podio App Id.")),
            React.createElement("button", { type: "submit", className: "btn btn-primary", onClick: function (e) { return handleSubmitClick(e); } }, "Submit"),
            React.createElement("button", { className: "btn btn-secondary ml-3", onClick: props.onCancel }, "Cancel")),
        React.createElement(LoadingSpinner, { isLoading: isLoading })));
};
