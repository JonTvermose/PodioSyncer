import React, { FunctionComponent, useState } from "react";
import { LoadingSpinner } from "../components/loading-spinner";
import styled from 'styled-components';
import { useHistory } from "react-router-dom";

declare const jsonRoutes: any;

type PodioAppProps = {
}

export const CreatePodioApp: FunctionComponent<PodioAppProps> = (props) => {
    const [name, setName] = useState("");
    const [podioAppId, setPodioAppId] = useState();
    const [podioAppToken, setPodioAppToken] = useState("");
    const [podioType, setPodioType] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const history = useHistory();

    const handleSubmitClick = (e: any) => {
        e.preventDefault();

        setIsLoading(true);

        fetch(jsonRoutes["createpodioapp"], {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ name: name, podioAppId: +podioAppId, appToken: podioAppToken, podioTypeExternalId: podioType })
        }).then(res => {
            if (res.ok === true) {
                setIsLoading(false);
                history.push("/");
            } else {
                window.alert("Error creating app. Check values and try again.");
                setIsLoading(false);
            }
        });
    };

    const handleCancel = function () {
        history.push("/");
    }

    return (
        <div className="container">
            <h3>Create podio app</h3>   
            <form>
                <div className="form-group">
                    <label>Name</label>
                    <input type="text" className="form-control" value={name} onChange={e => setName(e.target.value)} />
                    <small className="form-text text-muted">The name of the Podio App.</small>
                </div>
                <div className="form-group">
                    <label>Podio App Id</label>
                    <input type="text" className="form-control" value={podioAppId} onChange={e => setPodioAppId(+e.target.value)} />
                    <small className="form-text text-muted">The Podio App Id.</small>
                </div>
                <div className="form-group">
                    <label>Podio Apptoken</label>
                    <input type="text" className="form-control" value={podioAppToken} onChange={e => setPodioAppToken(e.target.value)} />
                    <small className="form-text text-muted">The Podio App Id.</small>
                </div>
                <div className="form-group">
                    <label>Podio Type ExernalId</label>
                    <input type="text" className="form-control" value={podioType} onChange={e => setPodioType(e.target.value)} />
                    <small className="form-text text-muted">The Podio field: Type ExternalId.</small>
                </div>

                <button type="submit" className="btn btn-primary" onClick={e => handleSubmitClick(e)}>Submit</button>
                <button className="btn btn-secondary ml-3" onClick={handleCancel}>Cancel</button>

            </form>
            <LoadingSpinner isLoading={isLoading}/>
        </div>)
}
