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
}
