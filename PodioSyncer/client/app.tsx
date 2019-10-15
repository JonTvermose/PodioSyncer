import * as React from 'react';
import './app.css';
import './custom.scss';
import { PodioApps } from "./pages/podioapps";

const App: React.FC = () => {
  return (
      <div className="App">
          <PodioApps />
    </div>
  );
}

export default App;
