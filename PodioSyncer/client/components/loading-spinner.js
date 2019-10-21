var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
import React from 'react';
import posed from "react-pose";
import { BeatLoader } from "react-spinners";
import { css } from "@emotion/core";
var OverLayDiv = posed.div({
    hidden: { opacity: 0 },
    visible: { opacity: 1 },
    position: "fixed",
    top: 0,
    left: 0,
    width: "100vw",
    height: "100vh",
    zIndex: 9
});
var override = css(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\nz-index: 99;\nmargin: 50vh auto;\n"], ["\nz-index: 99;\nmargin: 50vh auto;\n"])));
export var LoadingSpinner = function (props) {
    return (React.createElement(OverLayDiv, { pose: props.isLoading ? "visible" : "hidden" },
        React.createElement(BeatLoader, { css: override, loading: props.isLoading, color: "50E3C2" })));
};
var templateObject_1;
