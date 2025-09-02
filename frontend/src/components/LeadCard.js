// src/components/LeadCard.js
import React from "react";
import api from "../services/api";
import "./LeadCard.css";

const LeadCard = ({ lead, onUpdate, showContact = false }) => {
  const handleAccept = async () => {
    try {
      await api.put(`/leads/${lead.id}/accept`);
      onUpdate();
    } catch (e) {
      console.error("Erro ao aceitar lead:", e);
    }
  };

  const handleDecline = async () => {
    try {
      await api.put(`/leads/${lead.id}/decline`);
      onUpdate();
    } catch (e) {
      console.error("Erro ao recusar lead:", e);
    }
  };

  const initials = `${(lead.contactFirstName || "?")[0] || "?"}${
    (lead.contactLastName || "")[0] || ""
  }`.toUpperCase();

  const fullName = `${lead.contactFirstName || ""} ${lead.contactLastName || ""}`.trim();

  return (
    <div className="lead-card">
      <div className="lead-header">
        <div className="lead-avatar">{initials}</div>
        <div className="lead-titlebox">
          <div className="lead-name">{fullName || "—"}</div>
          <div className="lead-date">
            {lead.dateCreated ? new Date(lead.dateCreated).toLocaleString() : "—"}
          </div>
        </div>
      </div>

      <div className="lead-body">
        <div className="lead-line">
          <span className="lead-badge">{lead.suburb || "—"}</span>
          <span className="lead-dot">•</span>
          <span className="lead-category">{lead.category || "—"}</span>
        </div>

        <div className="lead-description">{lead.description || "—"}</div>

        {showContact && (
          <>
            <div className="lead-sep" />
            <div className="lead-contact">
              <div className="contact-row">
                <span className="contact-label">Contact:</span>
                <span className="contact-value">{fullName || "—"}</span>
              </div>
              <div className="contact-row">
                <span className="contact-label">Phone:</span>
                {lead.phoneNumber ? (
                  <a className="contact-link" href={`tel:${lead.phoneNumber}`}>
                    {lead.phoneNumber}
                  </a>
                ) : (
                  <span className="contact-value">—</span>
                )}
              </div>
              <div className="contact-row">
                <span className="contact-label">Email:</span>
                {lead.email ? (
                  <a className="contact-link" href={`mailto:${lead.email}`}>
                    {lead.email}
                  </a>
                ) : (
                  <span className="contact-value">—</span>
                )}
              </div>
            </div>
          </>
        )}
      </div>

      <div className="lead-footer">
        <div className="lead-price">
          ${Number(lead.price || 0).toFixed(2)}
        </div>

        {lead.status === "Pendente" ? (
          <div className="lead-actions">
            <button className="btn accept" onClick={handleAccept}>
              Accept
            </button>
            <button className="btn decline" onClick={handleDecline}>
              Decline
            </button>
          </div>
        ) : (
          <div className={`status-pill ${lead.status === "Aceito" ? "ok" : "no"}`}>
            {lead.status}
          </div>
        )}
      </div>
    </div>
  );
};

export default LeadCard;
