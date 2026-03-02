import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { InseeIndexService } from '../Services/insee-index.service';
import { InseeIndex } from '../models/insee-index';

@Component({
  selector: 'app-insee-index-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './insee-index-list.component.html',
  styleUrl: './insee-index-list.component.css'
})
export class InseeIndexListComponent implements OnInit {
  indexes: InseeIndex[] = [];
  errorMessage: string | null = null;

  constructor(private inseeIndexService: InseeIndexService) {}

  ngOnInit(): void {
    this.loadIndexes();
  }

  private loadIndexes(): void {
    this.inseeIndexService.getInseeIndexes().subscribe({
      next: (data) => {
        this.indexes = data;
      },
      error: (err) => {
        console.error('Error loading Insee indexes:', err);
        this.errorMessage = `Error occured (${err.status})`;
      },
    });
  }

  getPeriodYear(period: string): string {
    if (!period) return '';
    const normalized = period.replace('/', '-');
    const parts = normalized.split('-');
    return parts[0] ?? '';
  }

  getPeriodMonth(period: string): string {
    if (!period) return '';
    const normalized = period.replace('/', '-');
    const parts = normalized.split('-');
    if (parts.length >= 2) {
      const monthNumber = Number(parts[1]);
      if (!Number.isFinite(monthNumber) || monthNumber < 1 || monthNumber > 12) {
        return parts[1] ?? '';
      }
      return this.getMonthLabel(monthNumber);
    }
    return '';
  }

  private getMonthLabel(monthNumber: number): string {
    const labels = [
      'JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN',
      'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'
    ];
    return labels[monthNumber - 1] ?? '';
  }
}
