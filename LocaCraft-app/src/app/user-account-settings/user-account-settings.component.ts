import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-user-account-settings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-account-settings.component.html',
  styleUrl: './user-account-settings.component.css'
})
export class UserAccountSettingsComponent implements OnInit {
  // Profile picture
  currentProfilePicture: string | null = null;
  currentEmail = '';
  isUpdatingPicture = false;
  pictureError = '';
  pictureSuccess = '';

  // Email
  newEmail = '';
  isChangingEmail = false;
  emailError = '';
  emailSuccess = '';

  // Password
  currentPassword = '';
  newPassword = '';
  confirmNewPassword = '';
  isChangingPassword = false;
  passwordError = '';
  passwordSuccess = '';

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.getMe().subscribe({
      next: (profile) => {
        this.currentEmail = profile.email;
        this.currentProfilePicture = profile.profilePictureBase64;
      }
    });
  }

  onProfilePictureSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    const reader = new FileReader();
    reader.onload = () => {
      const dataUrl = reader.result as string;
      const base64 = dataUrl.split(',')[1];
      this.isUpdatingPicture = true;
      this.pictureError = '';
      this.pictureSuccess = '';
      this.authService.updateProfilePicture(base64).subscribe({
        next: () => {
          this.currentProfilePicture = base64;
          this.isUpdatingPicture = false;
          this.pictureSuccess = 'Photo de profil mise à jour.';
        },
        error: () => {
          this.isUpdatingPicture = false;
          this.pictureError = 'Erreur lors de la mise à jour de la photo.';
        }
      });
    };
    reader.readAsDataURL(file);
  }

  get profilePictureSrc(): string {
    return `data:image/jpeg;base64,${this.currentProfilePicture}`;
  }

  onChangeEmail(): void {
    this.emailError = '';
    this.emailSuccess = '';
    this.isChangingEmail = true;
    this.authService.changeEmail(this.newEmail).subscribe({
      next: () => {
        this.currentEmail = this.newEmail;
        this.newEmail = '';
        this.isChangingEmail = false;
        this.emailSuccess = 'Adresse e-mail mise à jour.';
      },
      error: (err) => {
        this.isChangingEmail = false;
        const errors = err.error;
        if (Array.isArray(errors) && errors.length > 0) {
          this.emailError = errors.map((e: { description: string }) => e.description).join(' ');
        } else {
          this.emailError = `Une erreur est survenue (${err.status}).`;
        }
      }
    });
  }

  onChangePassword(): void {
    this.passwordError = '';
    this.passwordSuccess = '';
    if (this.newPassword !== this.confirmNewPassword) {
      this.passwordError = 'Les nouveaux mots de passe ne correspondent pas.';
      return;
    }
    this.isChangingPassword = true;
    this.authService.changePassword(this.currentPassword, this.newPassword).subscribe({
      next: () => {
        this.currentPassword = '';
        this.newPassword = '';
        this.confirmNewPassword = '';
        this.isChangingPassword = false;
        this.passwordSuccess = 'Mot de passe mis à jour.';
      },
      error: (err) => {
        this.isChangingPassword = false;
        const errors = err.error;
        if (Array.isArray(errors) && errors.length > 0) {
          this.passwordError = errors.map((e: { description: string }) => e.description).join(' ');
        } else {
          this.passwordError = `Une erreur est survenue (${err.status}).`;
        }
      }
    });
  }
}
