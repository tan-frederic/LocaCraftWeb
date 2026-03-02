import { Injectable } from '@angular/core';
import {jsPDF} from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Lease } from '../models/lease';
import { Tenant } from '../models/tenant';
import { Lessor } from '../models/lessor';

export interface RentReceiptLine {
  date: string;
  amount: number;
  wording: string;
}

export interface RentReceiptData {
  tenant: Tenant;
  lessor: Lessor;
  lease: Lease;
  rentReceiptLines: RentReceiptLine[];
  city: string;
  dateCreation: string;
}


@Injectable({
  providedIn: 'root'
})


export class RentReceiptService {

  constructor() { }

  generateRentReceiptPDF(data: RentReceiptData): void {
    const doc = new jsPDF({ orientation: 'portrait', unit: 'mm', format: 'a4' });

    const marginLeft = 20;
    const marginRight = 190;
    const pageWidth = 210;

    const fontColor = '#000000';

    doc.setFontSize(11);
    doc.setFont('helvetica', 'bold');
    doc.text(data.lessor.name, marginLeft, 20);

    doc.setFont('helvetica', 'normal');
    doc.setFontSize(10);
    doc.text(data.lessor.address, marginLeft, 26);
    doc.text(data.lessor.postalCode, marginLeft, 31);
    doc.text(`tel : ${data.lessor.phone}`, marginLeft, 36);
    doc.text(data.lessor.email, marginLeft, 41);

    doc.setFontSize(18);
    doc.setFont('helvetica', 'bold');
    doc.text('QUITTANCE', pageWidth / 2, 55, { align: 'center' });

    doc.setFontSize(10);
    doc.setFont('helvetica', 'normal');
    doc.text(`Locataire : ${data.tenant.name}`, marginLeft, 68);
    doc.text(data.tenant.address, marginLeft, 73);
    doc.text(data.tenant.postalCode, marginLeft, 78);

    const colDroite = 120;
    doc.setFont('helvetica', 'bold');
    doc.text(`$${data.tenant.name}`, colDroite, 68);
    doc.setFont('helvetica', 'normal');
    doc.text(data.tenant.address, colDroite, 74);
    doc.text(data.tenant.postalCode, colDroite, 80);

    doc.text(
      `${data.city}, le ${data.dateCreation}`,
      marginRight,
      92,
      { align: 'right' }
    );

    doc.setFont('helvetica', 'normal');
    doc.setFontSize(10);
    doc.text('Monsieur,', marginLeft, 100);

    const introTexte = [
      "Veuillez trouver la quittance demandée qui ne libère que pour la(es) période(s)",
      "indiquée(s). Elle est délivrée sous toutes réserves de droit et sous réserve de",
      "l'encaissement des chèques ou effets.",
      "",
      "Elle annule tous les reçus qui auraient pu être donnés pour acompte versé sur le(s)",
      "présent(s) terme(s), même si ces reçus portent une date postérieure à la date ci-contre.",
      "Le paiement de la présente quittance n'emporte pas présomption de paiement des",
      "termes antérieurs.",
    ];

    let y = 107;
    for (const ligne of introTexte) {
      doc.text(ligne, marginLeft, y);
      y += 5.5;
    }

     const totalMontant = data.rentReceiptLines.reduce((sum, l) => sum + l.amount, 0);

    const tableBody = data.rentReceiptLines.map(l => [
      l.date,
      l.wording,
      '',
      this.formatAmount(l.amount)
    ]);

    autoTable(doc, {
      startY: y + 4,
      head: [['Date', 'Libellé', 'Débit', 'Crédit']],
      body: tableBody,
      foot: [['', 'Total quittance', '', this.formatAmount(totalMontant)]],
      theme: 'grid',
      headStyles: {
        fillColor: [255, 255, 255],
        textColor: [0, 0, 0],
        fontStyle: 'bold',
        lineColor: [0, 0, 0],
        lineWidth: 0.3,
      },
      footStyles: {
        fillColor: [255, 255, 255],
        textColor: [0, 0, 0],
        fontStyle: 'bold',
        lineColor: [0, 0, 0],
        lineWidth: 0.3,
      },
      bodyStyles: {
        textColor: [0, 0, 0],
        lineColor: [0, 0, 0],
        lineWidth: 0.3,
      },
      alternateRowStyles: {
        fillColor: [196, 196, 196],
      },
      columnStyles: {
        0: { cellWidth: 30 },
        1: { cellWidth: 100 },
        2: { cellWidth: 25, halign: 'right' },
        3: { cellWidth: 25, halign: 'right' },
      },
      margin: { left: marginLeft, right: 20 },
    });

    // ── Formule de politesse finale ────────────────────────
    // @ts-ignore (jspdf-autotable ajoute lastAutoTable sur le doc)
    const afterTable = (doc as any).lastAutoTable.finalY + 10;

    const civiliteAccord = 'Monsieur';
    doc.text(
      `Nous vous prions d'agréer, ${civiliteAccord}, l'assurance de nos sentiments distingués.`,
      marginLeft,
      afterTable
    );

    // ── Nom du bailleur en bas ─────────────────────────────
    doc.setFont('helvetica', 'bold');
    doc.text(data.lessor.name, marginLeft, afterTable + 15);

    // ── Téléchargement ─────────────────────────────────────
    const nomFichier = this.genererNomFichier(data);
    doc.save(nomFichier);
  }

   private formatAmount(montant: number): string {
    return montant.toLocaleString('fr-FR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }) + ' €';
  }

  private genererNomFichier(data: RentReceiptData): string {
    // ex: "quittance_DE-BRITO_02-2026.pdf"
    const nomNormalise = data.tenant.name.replace(/\s+/g, '-').toUpperCase();
    const date = new Date();
    const mois = String(date.getMonth() + 1).padStart(2, '0');
    const annee = date.getFullYear();
    return `quittance_${nomNormalise}_${mois}-${annee}.pdf`;
  }
}
