import React, { FunctionComponent } from 'react';
import styled from 'styled-components';
import { PodioAppModel } from "../../models/PodioAppModel";

type PodioRowProps = {
    rows: PodioAppModel[];
    onEdit(appId: string): void,
    onDelete(appId: string): void
    onSync(appId: string): void
}

export const PodioRow: FunctionComponent<PodioRowProps> = (podioProps) => {     

    return (
        <div>
            <div className="row" style={{ borderBottom: "1px solid #afafaf", paddingBottom: "5px", paddingTop: "5px", fontWeight: "bold" }}>
                <div className="col-3">Name</div>
                <div className="col-2">Podio App Id</div>
                <div className="col-3">Webhook Url</div>
                <div className="col-2"></div>
            </div>
            {podioProps.rows.map((app, index) =>
                <div key={index} className="row" style={{ borderBottom: "1px solid #afafaf", paddingBottom: "5px", paddingTop: "5px" }}>
                    <div className="col-3">{app.name}</div>
                    <div className="col-2">{app.podioAppId}</div>
                    <div className="col-3"><span style={{fontSize: "12px"}}>{app.webhookUrl}</span></div>
                    <div className="col-4">
                        <button className="btn btn-sm btn-danger ml-2 float-right" onClick={() => podioProps.onDelete(app.podioAppId)}>Delete</button>
                        <button className="btn btn-sm btn-primary ml-2 float-right" onClick={() => podioProps.onEdit(app.podioAppId)}>Edit</button>
                        <button className="btn btn-sm btn-secondary float-right" onClick={() => podioProps.onSync(app.podioAppId)}>Sync item</button>
                    </div>
                </div>
                )}
        </div>)
}
