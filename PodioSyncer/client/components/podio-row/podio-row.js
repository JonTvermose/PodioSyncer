var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
import React from 'react';
import styled from 'styled-components';
var StatusDiv = styled.div(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\nbackground-color: ", ";\nwidth: 10px;\nheight: 10px;\nborder-radius: 5px;\nmargin-left: 40%;\nmargin-top: 10px;\n"], ["\nbackground-color: ", ";\nwidth: 10px;\nheight: 10px;\nborder-radius: 5px;\nmargin-left: 40%;\nmargin-top: 10px;\n"])), function (props) { return props.active ? "green" : "red"; });
export var PodioRow = function (podioProps) {
    return (React.createElement("div", null,
        React.createElement("div", { className: "row", style: { borderBottom: "1px solid #afafaf", paddingBottom: "5px", paddingTop: "5px", fontWeight: "bold" } },
            React.createElement("div", { className: "col-3" }, "Name"),
            React.createElement("div", { className: "col-2" }, "Podio App Id"),
            React.createElement("div", { className: "col-3" }, "Webhook Url"),
            React.createElement("div", { className: "col-1" }, "Active"),
            React.createElement("div", { className: "col-1" }, "Verified"),
            React.createElement("div", { className: "col-2" })),
        podioProps.rows.map(function (app, index) {
            return React.createElement("div", { key: index, className: "row", style: { borderBottom: "1px solid #afafaf", paddingBottom: "5px", paddingTop: "5px" } },
                React.createElement("div", { className: "col-3" }, app.name),
                React.createElement("div", { className: "col-2" }, app.podioAppId),
                React.createElement("div", { className: "col-3" }, app.webhookUrl),
                React.createElement("div", { className: "col-1" },
                    React.createElement(StatusDiv, { active: app.active })),
                React.createElement("div", { className: "col-1" },
                    React.createElement(StatusDiv, { active: app.verified })),
                React.createElement("div", { className: "col-2" },
                    React.createElement("button", { className: "btn btn-sm btn-danger ml-2 float-right", onClick: function () { return podioProps.onDelete(app.podioAppId); } }, "Delete"),
                    React.createElement("button", { className: "btn btn-sm btn-primary float-right", onClick: function () { return podioProps.onEdit(app.podioAppId); } }, "Edit")));
        })));
};
var templateObject_1;
