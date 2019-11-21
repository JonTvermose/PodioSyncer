import React from 'react';
export var PodioRow = function (podioProps) {
    return (React.createElement("div", null,
        React.createElement("div", { className: "row", style: { borderBottom: "1px solid #afafaf", paddingBottom: "5px", paddingTop: "5px", fontWeight: "bold" } },
            React.createElement("div", { className: "col-3" }, "Name"),
            React.createElement("div", { className: "col-2" }, "Podio App Id"),
            React.createElement("div", { className: "col-3" }, "Webhook Url"),
            React.createElement("div", { className: "col-2" })),
        podioProps.rows.map(function (app, index) {
            return React.createElement("div", { key: index, className: "row", style: { borderBottom: "1px solid #afafaf", paddingBottom: "5px", paddingTop: "5px" } },
                React.createElement("div", { className: "col-3" }, app.name),
                React.createElement("div", { className: "col-2" }, app.podioAppId),
                React.createElement("div", { className: "col-3" },
                    React.createElement("span", { style: { fontSize: "12px" } }, app.webhookUrl)),
                React.createElement("div", { className: "col-4" },
                    React.createElement("button", { className: "btn btn-sm btn-danger ml-2 float-right", onClick: function () { return podioProps.onDelete(app.podioAppId); } }, "Delete"),
                    React.createElement("button", { className: "btn btn-sm btn-primary ml-2 float-right", onClick: function () { return podioProps.onEdit(app.podioAppId); } }, "Edit"),
                    React.createElement("button", { className: "btn btn-sm btn-secondary float-right", onClick: function () { return podioProps.onSync(app.podioAppId); } }, "Sync item")));
        })));
};
