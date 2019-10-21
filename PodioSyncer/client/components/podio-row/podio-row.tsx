import React, { FunctionComponent } from 'react';
import styled from 'styled-components';

type PodioRowProps = {
    name: string,
    appId: string,
    webhookUrl: string,
    status: boolean,
    verified: boolean,
    onEdit(appId: string): void,
    onDelete(appId: string): void
}

const StatusDiv = styled.div<{ active: boolean }>`
background-color: ${(props) => props.active ? "green" : "red"};
width: 20px;
height: 20px;
border-radius: 10px;
`;

export const PodioRow: FunctionComponent<PodioRowProps> = (podioProps) => {
    
    return (
        <div className="row">
            <div className="col-3">{podioProps.name}</div>
            <div className="col-2">{podioProps.appId}</div>
            <div className="col-3">{podioProps.webhookUrl}</div>
            <div className="col-1"><StatusDiv active={podioProps.status} /></div>
            <div className="col-1"><StatusDiv active={podioProps.verified} /></div>
            <div className="col-1" onClick={() => podioProps.onEdit(podioProps.appId)}><button className="btn btn-sm btn-primary">Edit</button></div>
            <div className="col-1" onClick={() => podioProps.onDelete(podioProps.appId)}><button className="btn btn-sm btn-danger">Delete</button></div>
        </div>)
}
