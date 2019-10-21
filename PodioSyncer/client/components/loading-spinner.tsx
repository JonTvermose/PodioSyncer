import React, { FunctionComponent } from 'react';
import styled from 'styled-components';
import posed from "react-pose";
import { PulseLoader } from "react-spinners";
import { css } from "@emotion/core";

type LoadingProps = {
    isLoading: boolean;
}

const PosedDiv = posed.div({
    hidden: { opacity: 0, zIndex: -1 },
    visible: { opacity: 1, zIndex: 9 }
});


const override = css`
z-index: 99;
display: inline-block;
margin-left: 50vw;
margin-top: 50vh;
`;

export const LoadingSpinner: FunctionComponent<LoadingProps> = (props) => {

    return (
        <PosedDiv style={{ position: "absolute", top: 0, left: 0, width: "100vw", height: "100vh", zIndex: 9, backgroundColor: "rgba(0,0,0,0.25)" }} pose={props.isLoading ? "visible" : "hidden"}>
            <PulseLoader css={override} loading={props.isLoading} />
        </PosedDiv>)
}
