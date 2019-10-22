import React, { FunctionComponent } from 'react';
import styled from 'styled-components';
import { PodioAppModel } from "../../models/PodioAppModel";

type PodioRowProps = {
    rows: PodioAppModel[];
    onEdit(appId: string): void,
    onDelete(appId: string): void
}

const StatusDiv = styled.div<{ active: boolean }>`
background-color: ${(props) => props.active ? "green" : "red"};
width: 10px;
height: 10px;
border-radius: 5px;
margin-left: 40%;
margin-top: 10px;
`;

export const PodioRow: FunctionComponent<PodioRowProps> = (podioProps) => {
       

    return (
        <div>
            <div className="row" style={{ borderBottom: "1px solid #afafaf", paddingBottom: "5px", paddingTop: "5px", fontWeight: "bold" }}>
                <div className="col-3">Name</div>
                <div className="col-2">Podio App Id</div>
                <div className="col-3">Webhook Url</div>
                <div className="col-1">Active</div>
                <div className="col-1">Verified</div>
                <div className="col-2"></div>
            </div>
            {podioProps.rows.map((app, index) =>
                <div key={index} className="row" style={{ borderBottom: "1px solid #afafaf", paddingBottom: "5px", paddingTop: "5px" }}>
                    <div className="col-3">{app.name}</div>
                    <div className="col-2">{app.podioAppId}</div>
                    <div className="col-3">{app.webhookUrl}</div>
                    <div className="col-1"><StatusDiv active={app.active} /></div>
                    <div className="col-1"><StatusDiv active={app.verified} /></div>
                    <div className="col-2">
                        <button className="btn btn-sm btn-danger ml-2 float-right" onClick={() => podioProps.onDelete(app.podioAppId)}>Delete</button>
                        <button className="btn btn-sm btn-primary float-right" onClick={() => podioProps.onEdit(app.podioAppId)}>Edit</button>
                    </div>
                </div>
                )}
        </div>
)
}
