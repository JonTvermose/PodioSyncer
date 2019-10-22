import React, { FunctionComponent, useState, useEffect } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

import { PodioRow } from "../components/podio-row/podio-row";
import { PodioAppModel } from "../models/PodioAppModel";

declare const jsonRoutes: any;

type PodioAppProps = {
}

export const PodioApps: FunctionComponent<PodioAppProps> = (podioProps) => {
    const [podioApps, setPodioApps] = useState<PodioAppModel[]>([]);

    useEffect(() => {
        fetch(jsonRoutes["getpodioapps"])
            .then(res => res.json())
            .then(res => setPodioApps(res));
    }, []);

    const handleOnEdit = (appId: string): void => {
        console.log("Edit clicked for Podio App Id: " + appId);
    };

    const handleOnDelete = (appId: string): void => {
        console.log("Delete clicked for Podio App Id: " + appId);
    };

    return (
        <div className="container">
            <h3>List of podio apps <Link to="/create"><button className="btn btn-primary float-right">Create new</button></Link></h3>
            <PodioRow 
                rows={podioApps}
                onEdit={handleOnEdit}
                onDelete={handleOnDelete} />
        </div>)
}
