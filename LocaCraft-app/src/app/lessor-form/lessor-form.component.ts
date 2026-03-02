import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LessorService } from '../Services/lessor.service';
import { Lessor } from '../models/lessor';

@Component({
  selector: 'app-lessor-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './lessor-form.component.html',
  styleUrl: './lessor-form.component.css'
})
export class LessorFormComponent {
  @Input() lessor: Lessor = this.getEmptyLessor();
  @Input() showActions: boolean = true;

  @Output() formSubmitted = new EventEmitter<Lessor>();
  @Output() formError = new EventEmitter<string>();
  @Output() formCancelled = new EventEmitter<void>();

  errorMessage: string = '';
  isSubmitting: boolean = false;

  constructor(private lessorService: LessorService) {}

  onSubmit(): void {
    if (this.isSubmitting) return;

    this.isSubmitting = true;
    this.errorMessage = '';

    this.lessorService.createLessor(this.lessor).subscribe({
      next: (result) => {
        this.isSubmitting = false;
        this.formSubmitted.emit(result);
      },
      error: (err) => {
        console.error('Error creating Lessor', err);
        this.errorMessage = `Error occured (${err.status})`;
        this.isSubmitting = false;
        this.formError.emit(this.errorMessage);
      },
    });
  }

  onCancel(): void {
    this.formCancelled.emit();
  }

  private getEmptyLessor(): Lessor {
    return {
      id: 0,
      name: '',
      address: '',
      city: '',
      postalCode: '',
      country: '',
      phone: '',
      email: '',
      leases: [],
    };
  }
}
