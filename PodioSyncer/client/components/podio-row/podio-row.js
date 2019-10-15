var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
import React from 'react';
import styled from 'styled-components';
var StatusDiv = styled.div(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\nbackground-color: ", ";\nwidth: 20px;\nheight: 20px;\nborder-radius: 10px;\n"], ["\nbackground-color: ", ";\nwidth: 20px;\nheight: 20px;\nborder-radius: 10px;\n"])), function (props) { return props.active ? "green" : "red"; });
export var PodioRow = function (podioProps) {
    return (React.createElement("div", { className: "row" },
        React.createElement("div", { className: "col-3" }, podioProps.name),
        React.createElement("div", { className: "col-2" }, podioProps.appId),
        React.createElement("div", { className: "col-3" }, podioProps.webhookUrl),
        React.createElement("div", { className: "col-1" },
            React.createElement(StatusDiv, { active: podioProps.status })),
        React.createElement("div", { className: "col-1" },
            React.createElement(StatusDiv, { active: podioProps.verified })),
        React.createElement("div", { className: "col-1", onClick: function () { return podioProps.onEdit(podioProps.appId); } }, "Edit"),
        React.createElement("div", { className: "col-1", onClick: function () { return podioProps.onDelete(podioProps.appId); } }, "Delete")));
};
var templateObject_1;
