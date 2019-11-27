import React, { FunctionComponent, useState, useEffect, useRef } from 'react';
import styled from 'styled-components';
import posed from 'react-pose';

import { LoadingSpinner } from "../components/loading-spinner";
import { LinkModel } from "../models/LinkModel";

declare const jsonRoutes: any;

type SyncEventsProps = {
}

export const SyncEvents: FunctionComponent<SyncEventsProps> = (podioProps) => {
    const [syncEvents, setSyncEvents] = useState<LinkModel[]>([]);
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true);
        fetch(jsonRoutes["getsyncevents"])
            .then(res => res.json())
            .then(res => {
                setSyncEvents(res);
                setIsLoading(false);
            });
    }, []);

    return (
        <div className="container">
            <h3>List of Sync Events</h3>
            <div className="row" style={{ paddingBottom: "10px", paddingTop: "10px", fontWeight: "bold" }}>
                <div className="col-2">
                    Date
                </div>
                <div className="col-1">
                    Initiator
                </div>
                <div className="col-5">
                    Podio Url
                </div>
                <div className="col-4">
                    Azure Url
                </div>
            </div>
            {syncEvents.map((link: LinkModel, index: number) =>
                <div key={index} className="row" style={{ paddingBottom: "10px", paddingTop: "10px", backgroundColor: index % 2 == 0 ? "white" : "" }}>
                    <div className="col-2">
                        {link.syncedDate}
                    </div>
                    <div className="col-1">
                        {link.initiator}
                    </div>
                    <div className="col-5">
                        <a href={link.podioUrl} target="_blank"><span style={{ fontSize: "12px" }}>{link.podioUrl}</span></a>                        
                    </div>
                    <div className="col-4">
                        <a href={link.azureUrl} target="_blank"><span style={{ fontSize: "12px" }}>{link.azureUrl}</span></a>                        
                    </div>
                </div>)}
            <LoadingSpinner isLoading={isLoading} />
        </div>)
}
