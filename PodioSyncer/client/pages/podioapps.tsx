import React, { FunctionComponent, useState, useEffect, useRef } from 'react';
import styled from 'styled-components';
import posed from 'react-pose';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { Link } from 'react-router-dom';

import { LoadingSpinner } from "../components/loading-spinner";
import { PodioRow } from "../components/podio-row/podio-row";
import { PodioAppModel } from "../models/PodioAppModel";

declare const jsonRoutes: any;

type PodioAppProps = {
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

export const PodioApps: FunctionComponent<PodioAppProps> = (podioProps) => {
    const [podioApps, setPodioApps] = useState<PodioAppModel[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [showSync, setShowSync] = useState(false);
    const [syncAppId, setSyncAppId] = useState(0);
    const [syncItemUrl, setSyncItemUrl] = useState("");
    const [syncedItems, setSyncedItems] = useState<string[]>([]);
    const inputRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        fetch(jsonRoutes["getpodioapps"])
            .then(res => res.json())
            .then(res => setPodioApps(res));
    }, []);

    useEffect(() => {
        if (showSync && inputRef.current) {
            inputRef.current.focus();
        }
    }, [showSync]);


    const handleOnEdit = (appId: string): void => {
        console.log("Edit clicked for Podio App Id: " + appId);
    };

    const handleOnDelete = (appId: string): void => {
        setIsLoading(true);
        fetch(jsonRoutes["deletepodioapp"] + "/" + appId, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then(res => {
            if (res.ok === true) {
                setPodioApps(podioApps.filter(x => x.podioAppId != appId));
                setIsLoading(false);
            } else {
                window.alert(res.statusText);
                setIsLoading(false);
            }
        });
    };

    const handleOnSync = (appId: string): void => {
        setShowSync(true);
        setSyncAppId(+appId);
        setSyncItemUrl("");
    }

    const handleSyncItemToAzure = () => {
        setIsLoading(true);        
        fetch(jsonRoutes["syncpodioitem"], {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ PodioAppId: syncAppId, PodioItemUrl: syncItemUrl })
        }).then(res => {
            if (res.ok === true) {
                setIsLoading(false);
                return res.json();
            } else {
                toast.error(res.statusText)
                setIsLoading(false);
            }
        }).then(data => {
            if (data.ok === true) {
                toast.success("Sync complete");
                let items = JSON.parse(JSON.stringify(syncedItems));
                items.push(data.url);
                setSyncedItems(items);
                if (inputRef.current) {
                    inputRef.current.focus();
                }
            } else {
                toast.error("Item is allready synced");
            }
            setIsLoading(false);
        });        
    }

    return (
        <div className="container">
            <h3>List of podio apps <Link to="/create"><button className="btn btn-primary float-right">Create new</button></Link></h3>
            <PodioRow 
                rows={podioApps}
                onEdit={handleOnEdit}
                onDelete={handleOnDelete}
                onSync={handleOnSync} />

            <PosedDiv style={{ position: "absolute", top: 0, left: 0, width: "100vw", height: "100vh", zIndex: 9, backgroundColor: "rgba(0,0,0,0.5)", textAlign: "center" }} pose={showSync ? "visible" : "hidden"}>
                
                <SyncDiv>
                    <div>
                        <form>
                            <div className="form-group">
                                <label>Podio item url</label>
                                <input ref={inputRef} type="text" className="form-control" value={syncItemUrl} onChange={e => setSyncItemUrl(e.target.value)} />
                                <small className="form-text text-muted">The full url of the Podio Item.</small>
                            </div>
                        </form>
                        <div className="row">
                            <div className="col-12">
                                <button className="btn btn-sm ml-2 float-right btn-secondary" onClick={() => setShowSync(false)}>Cancel</button>
                                <button className="btn btn-sm float-right btn-primary" onClick={() => handleSyncItemToAzure()}>Sync to Azure</button>
                            </div>
                        </div>
                        <form>
                            <div className="form-group mt-3">
                                {syncedItems.length !== 0 && <label className="mb-0">Synced in this session:</label>}
                                {syncedItems.map((value: string, index: number) => {
                                    return (<small key={index +1} className="form-text text-muted ml-2">{value}</small>)
                                })}
                            </div>                    
                        </form>
                    </div>
                </SyncDiv>
                </PosedDiv>
            <LoadingSpinner isLoading={isLoading} />
            <ToastContainer />
        </div>)
}
