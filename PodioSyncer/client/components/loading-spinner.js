var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
import React from 'react';
import posed from "react-pose";
import { PulseLoader } from "react-spinners";
import { css } from "@emotion/core";
var PosedDiv = posed.div({
    hidden: { opacity: 0, zIndex: -1 },
    visible: { opacity: 1, zIndex: 9 }
});
var override = css(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\nz-index: 99;\ndisplay: inline-block;\nmargin-left: 50vw;\nmargin-top: 50vh;\n"], ["\nz-index: 99;\ndisplay: inline-block;\nmargin-left: 50vw;\nmargin-top: 50vh;\n"])));
export var LoadingSpinner = function (props) {
    return (React.createElement(PosedDiv, { style: { position: "absolute", top: 0, left: 0, width: "100vw", height: "100vh", zIndex: 9, backgroundColor: "rgba(0,0,0,0.25)" }, pose: props.isLoading ? "visible" : "hidden" },
        React.createElement(PulseLoader, { css: override, loading: props.isLoading })));
};
var templateObject_1;
