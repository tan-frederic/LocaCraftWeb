import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.css'
})
export class LoginFormComponent {
  isLoginMode = false;

  email = '';
  password = '';
  confirmPassword = '';

  errorMessage = '';
  successMessage = '';
  isSubmitting = false;

  constructor(private authService: AuthService, private router: Router) {}

  toggleMode(): void {
    this.isLoginMode = !this.isLoginMode;
    this.errorMessage = '';
    this.successMessage = '';
    this.email = '';
    this.password = '';
    this.confirmPassword = '';
  }

  onSubmit(): void {
    if (this.isSubmitting) return;

    this.errorMessage = '';
    this.successMessage = '';

    if (!this.isLoginMode && this.password !== this.confirmPassword) {
      this.errorMessage = 'Les mots de passe ne correspondent pas.';
      return;
    }

    this.isSubmitting = true;

    if (this.isLoginMode) {
      this.authService.login({ email: this.email, password: this.password }).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.isSubmitting = false;
          this.errorMessage = err.status === 401
            ? 'Email ou mot de passe incorrect.'
            : `Une erreur est survenue (${err.status}).`;
        }
      });
    } else {
      this.authService.register({ email: this.email, password: this.password }).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.successMessage = 'Compte créé avec succès. Vous pouvez maintenant vous connecter.';
          this.isLoginMode = true;
          this.email = '';
          this.password = '';
          this.confirmPassword = '';
        },
        error: (err) => {
          this.isSubmitting = false;
          const errors = err.error;
          if (Array.isArray(errors) && errors.length > 0) {
            this.errorMessage = errors.map((e: { description: string }) => e.description).join(' ');
          } else {
            this.errorMessage = `Une erreur est survenue (${err.status}).`;
          }
        }
      });
    }
  }
}
