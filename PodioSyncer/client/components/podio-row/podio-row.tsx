import React, { FunctionComponent, useState } from 'react';
import styled from 'styled-components';
import posed from 'react-pose';
import { PodioAppModel } from "../../models/PodioAppModel";
import { LinkModel } from "../../models/LinkModel";

import { LoadingSpinner } from "../loading-spinner";


declare const jsonRoutes: any;

type PodioRowProps = {
    rows: PodioAppModel[];
    onEdit(appId: string): void,
    onDelete(appId: string): void
    onSync(appId: string): void
    onLink(appId: string): void
}

const PosedDiv = posed.div({
    hidden: { opacity: 0, zIndex: -1 },
    visible: { opacity: 1, zIndex: 9 }
});

const SyncDiv = styled.div`
margin: 200px;
background-color: white;
padding: 40px;
border-radius: 5px;
text-align: left;
max-width: 900px;
min-width: 900px;
display: inline-block;
`;

export const PodioRow: FunctionComponent<PodioRowProps> = (podioProps) => {     
    const [showSync, setShowSync] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [syncedItems, setSyncedItems] = useState<LinkModel[]>([]);

    const handleSyncExitClick = (e: any) => {
        if (e.target.id === "overlay") {
            setShowSync(false);
        }
    }

    const handleSyncedClick = (id: any) => {
        setIsLoading(true);
        setShowSync(true);
        setSyncedItems([]);
        fetch(jsonRoutes["getlinks"] + "?id=" + id, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
        }).then(res => {
            if (res.ok === true) {
                setIsLoading(false);
                return res.json();
            } 
        }).then(data => {
            setSyncedItems(data);
            setIsLoading(false);
        }); 
    }

    return (
        <div className="mt-3">
            <div className="row" style={{paddingBottom: "10px", paddingTop: "10px", fontWeight: "bold" }}>
                <div className="col-3">Name</div>
                <div className="col-2">Podio App Id</div>
                <div className="col-3">Webhook Url</div>
                <div className="col-2"></div>
            </div>


            {podioProps.rows.map((app, index) =>
                <div key={index} className="row" style={{ paddingBottom: "10px", paddingTop: "10px", backgroundColor: index % 2 == 0 ? "white" : "" }}>
                    <div className="col-2">{app.name}</div>
                    <div className="col-2">{app.podioAppId}</div>
                    <div className="col-3"><span style={{fontSize: "12px"}}>{app.webhookUrl}</span></div>
                    <div className="col-5">
                        <button className="btn btn-sm btn-danger ml-2 float-right" onClick={() => podioProps.onDelete(app.podioAppId)}>Delete</button>
                        <button className="btn btn-sm btn-primary ml-2 float-right" onClick={() => podioProps.onEdit(app.podioAppId)}>Edit</button>
                        <button className="btn btn-sm btn-secondary ml-2 float-right" onClick={() => podioProps.onSync(app.podioAppId)}>Sync item</button>
                        <button className="btn btn-sm btn-outline-secondary ml-2 float-right" onClick={() => handleSyncedClick(app.podioAppId)}>Synced item</button>
                        <button className="btn btn-sm btn-outline-primary float-right" onClick={() => podioProps.onLink(app.podioAppId)}>Link item</button>
                    </div>
                </div>
            )}

            <PosedDiv id="overlay" onClick={handleSyncExitClick} style={{ position: "absolute", top: 0, left: 0, width: "100vw", height: "100vh", zIndex: 9, backgroundColor: "rgba(0,0,0,0.5)", textAlign: "center" }} pose={showSync ? "visible" : "hidden"}>
                <SyncDiv>
                    <div>
                        <div className="row" style={{ paddingBottom: "10px", paddingTop: "10px", fontWeight: "bold" }}>
                            <div className="col-5">Podio Url</div>
                            <div className="col-5">Azure Url</div>
                            <div className="col-2">First sync</div>
                        </div>
                        {syncedItems.map((app, index) =>
                            <div key={index} className="row" style={{ paddingBottom: "10px", paddingTop: "10px", backgroundColor: index % 2 == 1 ? "white" : "" }}>
                                <div className="col-5"><a href={app.podioUrl} target="_blank"><span style={{ fontSize: "12px" }}>{app.podioUrl}</span></a></div>
                                <div className="col-5"><a href={app.azureUrl} target="_blank"><span style={{ fontSize: "12px" }}>{app.azureUrl}</span></a></div>
                                <div className="col-2"><span style={{ fontSize: "12px" }}>{app.syncedDate}</span></div>
                            </div>
                            )}
                        <div className="row">
                            <div className="col-12">
                                <button className="btn btn-sm ml-2 float-right btn-secondary" onClick={() => setShowSync(false)}>Close</button>
                            </div>
                        </div>

                    </div>
                </SyncDiv>
            </PosedDiv>
            <LoadingSpinner isLoading={isLoading} />

        </div>)
}
