// src/pages/Accepted.js
import React, { useEffect, useState } from "react";
import api from "../services/api";
import LeadCard from "../components/LeadCard";

const Accepted = () => {
  const [leads, setLeads] = useState([]);
  const [loading, setLoading] = useState(true);

  const loadLeads = async () => {
    try {
      setLoading(true);
      const res = await api.get("/leads?status=Aceito");
      setLeads(res.data || []);
    } catch (e) {
      console.error("Erro ao carregar Accepted:", e);
      setLeads([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadLeads();
  }, []);

  return (
    <div>
      {loading ? (
        <p className="muted">Carregando...</p>
      ) : leads.length === 0 ? (
        <p className="muted">Nenhum lead aceito.</p>
      ) : (
        leads.map((lead) => (
          <LeadCard
            key={lead.id}
            lead={lead}
            onUpdate={loadLeads}
            showContact={true}
          />
        ))
      )}
    </div>
  );
};

export default Accepted;
