﻿import React, { FunctionComponent, useState, useEffect } from 'react';
import styled from 'styled-components';
import { PodioRow } from "../components/podio-row/podio-row";
import { PodioAppModel } from "../models/PodioAppModel";


type PodioAppProps = {
}

export const PodioApps: FunctionComponent<PodioAppProps> = (podioProps) => {
    const [podioApps, setPodioApps] = useState<PodioAppModel[]>([]);

    useEffect(() => {
        fetch("https://localhost:44391/api/podio/getpodioapps")
            .then(res => res.json())
            .then(res => setPodioApps(res));
    }, []);

    const handleOnEdit = (appId: number): void => {
        console.log("Edit clicked for Podio App Id: " + appId);
    };

    const handleOnDelete = (appId: number): void => {
        console.log("Delete clicked for Podio App Id: " + appId);
    };

    return (
        <div className="container">
            <h3>List of podio apps</h3>
            {podioApps.map((app, index) =>
                <PodioRow key={index}
                    appId={app.podioAppId}
                    name={app.name}
                    verified={app.verified}
                    status={app.active}
                    webhookUrl=""
                    onEdit={handleOnEdit}
                    onDelete={handleOnDelete} />)}
        </div>)
}