import React, { useEffect, useState } from "react";
import api from "../api";
import LeadCard from "../components/LeadCard.js";
import "./LeadsPage.css";

useEffect(() => {
  fetchLeads();
}, []);

useEffect(() => {
  console.log("Leads recebidos:", leads);
}, [leads]);

const LeadsPage = () => {
  const [leads, setLeads] = useState([]);
  const [activeTab, setActiveTab] = useState("invited");

  const fetchLeads = async () => {
    const res = await api.get("/");
    setLeads(res.data);
  };

  useEffect(() => {
    fetchLeads();
  }, []);

  const handleAccept = async (id) => {
    await api.put(`/${id}/accept`);
    fetchLeads();
  };

  const handleDecline = async (id) => {
    await api.put(`/${id}/decline`);
    fetchLeads();
  };

  // Corrigindo status case-insensitive
  const invited = leads.filter((l) => l.status?.toLowerCase() === "invited");
  const accepted = leads.filter((l) => l.status?.toLowerCase() === "accepted");

  return (
    <div className="leads-page">
      <h1>Lista de Leads</h1>

      {/* Abas */}
      <div className="tabs">
        <button
          className={activeTab === "invited" ? "active" : ""}
          onClick={() => setActiveTab("invited")}
        >
          Invited ({invited.length})
        </button>
        <button
          className={activeTab === "accepted" ? "active" : ""}
          onClick={() => setActiveTab("accepted")}
        >
          Accepted ({accepted.length})
        </button>
      </div>

      {/* Conteúdo da aba */}
      <div className="tab-content">
        {activeTab === "invited" && (
          <>
            {invited.length === 0 && <p>Nenhum lead convidado.</p>}
            {invited.map((lead) => (
              <LeadCard
                key={lead.id}
                lead={lead}
                onAccept={handleAccept}
                onDecline={handleDecline}
              />
            ))}
          </>
        )}

        {activeTab === "accepted" && (
          <>
            {accepted.length === 0 && <p>Nenhum lead aceito.</p>}
            {accepted.map((lead) => (
              <LeadCard key={lead.id} lead={lead} />
            ))}
          </>
        )}
      </div>
    </div>
  );
};

console.log("Leads no estado:", leads);

export default LeadsPage;
