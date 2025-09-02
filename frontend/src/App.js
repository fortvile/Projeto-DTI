// src/App.js
import React, { useState } from "react";
import Invited from "./pages/Invited";
import Accepted from "./pages/Accepted";
import Tabs from "./components/Tabs";
import "./App.css";

function App() {
  const [tab, setTab] = useState("invited");

  return (
    <div className="layout">
      <header className="topbar">
        <h1 className="brand">Leads</h1>
      </header>

      <main className="content">
        <div className="panel">
          <Tabs active={tab} onChange={setTab} />
          {tab === "invited" ? <Invited /> : <Accepted />}
        </div>
      </main>
    </div>
  );
}

export default App;
