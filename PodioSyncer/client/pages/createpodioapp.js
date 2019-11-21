import React, { useState } from "react";
import { LoadingSpinner } from "../components/loading-spinner";
import { useHistory } from "react-router-dom";
export var CreatePodioApp = function (props) {
    var _a = useState(""), name = _a[0], setName = _a[1];
    var _b = useState(), podioAppId = _b[0], setPodioAppId = _b[1];
    var _c = useState(""), podioAppToken = _c[0], setPodioAppToken = _c[1];
    var _d = useState(""), podioType = _d[0], setPodioType = _d[1];
    var _e = useState(false), isLoading = _e[0], setIsLoading = _e[1];
    var history = useHistory();
    var handleSubmitClick = function (e) {
        e.preventDefault();
        setIsLoading(true);
        fetch(jsonRoutes["createpodioapp"], {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ name: name, podioAppId: +podioAppId, appToken: podioAppToken, podioTypeExternalId: podioType })
        }).then(function (res) {
            if (res.ok === true) {
                setIsLoading(false);
                history.push("/");
            }
            else {
                window.alert("Error creating app. Check values and try again.");
                setIsLoading(false);
            }
        });
    };
    var handleCancel = function () {
        history.push("/");
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
                React.createElement("input", { type: "text", className: "form-control", value: podioAppId, onChange: function (e) { return setPodioAppId(+e.target.value); } }),
                React.createElement("small", { className: "form-text text-muted" }, "The Podio App Id.")),
            React.createElement("div", { className: "form-group" },
                React.createElement("label", null, "Podio Apptoken"),
                React.createElement("input", { type: "text", className: "form-control", value: podioAppToken, onChange: function (e) { return setPodioAppToken(e.target.value); } }),
                React.createElement("small", { className: "form-text text-muted" }, "The Podio App Id.")),
            React.createElement("div", { className: "form-group" },
                React.createElement("label", null, "Podio Type ExernalId"),
                React.createElement("input", { type: "text", className: "form-control", value: podioType, onChange: function (e) { return setPodioType(e.target.value); } }),
                React.createElement("small", { className: "form-text text-muted" }, "The Podio field: Type ExternalId.")),
            React.createElement("button", { type: "submit", className: "btn btn-primary", onClick: function (e) { return handleSubmitClick(e); } }, "Submit"),
            React.createElement("button", { className: "btn btn-secondary ml-3", onClick: handleCancel }, "Cancel")),
        React.createElement(LoadingSpinner, { isLoading: isLoading })));
};
