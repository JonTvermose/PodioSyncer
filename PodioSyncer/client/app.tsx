import * as React from 'react';
import './app.css';
import './custom.scss';
import { PodioApps } from "./pages/podioapps";
import { CreatePodioApp } from "./pages/createpodioapp";


const App: React.FC = () => {
  return (
      <div className="App">
          <PodioApps />
          <CreatePodioApp onCreated={() => alert("Created")} onCancel={() => alert("Cancelled")} />
    </div>
  );
}

export default App;
