var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
import React, { useState, useEffect, useRef } from 'react';
import styled from 'styled-components';
import posed from 'react-pose';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { Link } from 'react-router-dom';
import { LoadingSpinner } from "../components/loading-spinner";
import { PodioRow } from "../components/podio-row/podio-row";
var PosedDiv = posed.div({
    hidden: { opacity: 0, zIndex: -1 },
    visible: { opacity: 1, zIndex: 9 }
});
var SyncDiv = styled.div(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\nmargin: 200px;\nbackground-color: white;\npadding: 40px;\n"], ["\nmargin: 200px;\nbackground-color: white;\npadding: 40px;\n"])));
export var PodioApps = function (podioProps) {
    var _a = useState([]), podioApps = _a[0], setPodioApps = _a[1];
    var _b = useState(false), isLoading = _b[0], setIsLoading = _b[1];
    var _c = useState(false), showSync = _c[0], setShowSync = _c[1];
    var _d = useState(0), syncAppId = _d[0], setSyncAppId = _d[1];
    var _e = useState(""), syncItemUrl = _e[0], setSyncItemUrl = _e[1];
    var _f = useState([]), syncedItems = _f[0], setSyncedItems = _f[1];
    var inputRef = useRef(null);
    useEffect(function () {
        fetch(jsonRoutes["getpodioapps"])
            .then(function (res) { return res.json(); })
            .then(function (res) { return setPodioApps(res); });
    }, []);
    useEffect(function () {
        if (showSync && inputRef.current) {
            inputRef.current.focus();
        }
    }, [showSync]);
    var handleOnEdit = function (appId) {
        console.log("Edit clicked for Podio App Id: " + appId);
    };
    var handleOnDelete = function (appId) {
        setIsLoading(true);
        fetch(jsonRoutes["deletepodioapp"] + "/" + appId, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then(function (res) {
            if (res.ok === true) {
                setPodioApps(podioApps.filter(function (x) { return x.podioAppId != appId; }));
                setIsLoading(false);
            }
            else {
                window.alert(res.statusText);
                setIsLoading(false);
            }
        });
    };
    var handleOnSync = function (appId) {
        setShowSync(true);
        setSyncAppId(+appId);
        setSyncItemUrl("");
    };
    var handleSyncItemToAzure = function () {
        setIsLoading(true);
        fetch(jsonRoutes["syncpodioitem"], {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ PodioAppId: syncAppId, PodioItemUrl: syncItemUrl })
        }).then(function (res) {
            if (res.ok === true) {
                setIsLoading(false);
                return res.json();
            }
            else {
                toast.error(res.statusText);
                setIsLoading(false);
            }
        }).then(function (data) {
            if (data.ok === true) {
                toast.success("Sync complete");
                var items = JSON.parse(JSON.stringify(syncedItems));
                items.push(data.url);
                setSyncedItems(items);
                if (inputRef.current) {
                    inputRef.current.focus();
                }
            }
            else {
                toast.error("Item is allready synced");
            }
            setIsLoading(false);
        });
    };
    return (React.createElement("div", { className: "container" },
        React.createElement("h3", null,
            "List of podio apps ",
            React.createElement(Link, { to: "/create" },
                React.createElement("button", { className: "btn btn-primary float-right" }, "Create new"))),
        React.createElement(PodioRow, { rows: podioApps, onEdit: handleOnEdit, onDelete: handleOnDelete, onSync: handleOnSync }),
        React.createElement(PosedDiv, { style: { position: "absolute", top: 0, left: 0, width: "100vw", height: "100vh", zIndex: 9, backgroundColor: "rgba(0,0,0,0.25)" }, pose: showSync ? "visible" : "hidden" },
            React.createElement(SyncDiv, null,
                React.createElement("form", null,
                    React.createElement("div", { className: "form-group" },
                        React.createElement("label", null, "Podio item url"),
                        React.createElement("input", { ref: inputRef, type: "text", className: "form-control", value: syncItemUrl, onChange: function (e) { return setSyncItemUrl(e.target.value); } }),
                        React.createElement("small", { className: "form-text text-muted" }, "The full url of the Podio Item."))),
                React.createElement("div", { className: "row" },
                    React.createElement("div", { className: "col-12" },
                        React.createElement("button", { className: "btn btn-sm ml-2 float-right btn-secondary", onClick: function () { return setShowSync(false); } }, "Cancel"),
                        React.createElement("button", { className: "btn btn-sm float-right btn-primary", onClick: function () { return handleSyncItemToAzure(); } }, "Sync to Azure"))),
                React.createElement("form", null,
                    React.createElement("div", { className: "form-group mt-3" },
                        syncedItems.length !== 0 && React.createElement("label", { className: "mb-0" }, "Synced in this session:"),
                        syncedItems.map(function (value, index) {
                            return (React.createElement("small", { key: index + 1, className: "form-text text-muted ml-2" }, value));
                        }))))),
        React.createElement(LoadingSpinner, { isLoading: isLoading }),
        React.createElement(ToastContainer, null)));
};
var templateObject_1;
