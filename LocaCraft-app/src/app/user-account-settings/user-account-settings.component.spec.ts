import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

import { UserAccountSettingsComponent } from './user-account-settings.component';
import { AuthService, UserProfile } from '../Services/auth.service';

const PROFILE: UserProfile = { email: 'user@example.com', profilePictureBase64: 'abc123' };

describe('UserAccountSettingsComponent', () => {
  let authSpy: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    authSpy = jasmine.createSpyObj('AuthService', [
      'getMe', 'changeEmail', 'changePassword', 'updateProfilePicture',
    ]);
    authSpy.getMe.and.returnValue(of(PROFILE));
    authSpy.changeEmail.and.returnValue(of(undefined));
    authSpy.changePassword.and.returnValue(of(undefined));
    authSpy.updateProfilePicture.and.returnValue(of(undefined));

    await TestBed.configureTestingModule({
      imports: [UserAccountSettingsComponent],
      providers: [{ provide: AuthService, useValue: authSpy }],
    }).compileComponents();
  });

  it('should create', () => {
    expect(TestBed.createComponent(UserAccountSettingsComponent).componentInstance).toBeTruthy();
  });

  it('should load user profile on ngOnInit and populate fields', () => {
    const fixture = TestBed.createComponent(UserAccountSettingsComponent);
    fixture.detectChanges();
    const comp = fixture.componentInstance;
    expect(authSpy.getMe).toHaveBeenCalled();
    expect(comp.currentEmail).toBe('user@example.com');
    expect(comp.currentProfilePicture).toBe('abc123');
  });

  it('profilePictureSrc getter should return a valid data URL', () => {
    const fixture = TestBed.createComponent(UserAccountSettingsComponent);
    fixture.detectChanges();
    expect(fixture.componentInstance.profilePictureSrc).toBe('data:image/jpeg;base64,abc123');
  });

  describe('onChangeEmail', () => {
    it('should call authService.changeEmail with newEmail', () => {
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.newEmail = 'new@email.com';
      comp.onChangeEmail();
      expect(authSpy.changeEmail).toHaveBeenCalledWith('new@email.com');
    });

    it('should update currentEmail, clear newEmail and show success on success', () => {
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.newEmail = 'new@email.com';
      comp.onChangeEmail();
      expect(comp.currentEmail).toBe('new@email.com');
      expect(comp.newEmail).toBe('');
      expect(comp.emailSuccess).toContain('mise à jour');
      expect(comp.isChangingEmail).toBeFalse();
    });

    it('should join error descriptions on failure', () => {
      authSpy.changeEmail.and.returnValue(
        throwError(() => ({ status: 400, error: [{ description: 'Email invalide' }] }))
      );
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.onChangeEmail();
      expect(comp.emailError).toContain('Email invalide');
      expect(comp.isChangingEmail).toBeFalse();
    });

    it('should set generic emailError for non-array error body', () => {
      authSpy.changeEmail.and.returnValue(throwError(() => ({ status: 500, error: null })));
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.onChangeEmail();
      expect(comp.emailError).toContain('500');
    });
  });

  describe('onChangePassword', () => {
    it('should set passwordError when new passwords do not match', () => {
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.newPassword = 'abc';
      comp.confirmNewPassword = 'xyz';
      comp.onChangePassword();
      expect(comp.passwordError).toContain('correspondent pas');
      expect(authSpy.changePassword).not.toHaveBeenCalled();
    });

    it('should call authService.changePassword with current and new password', () => {
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.currentPassword = 'old';
      comp.newPassword = 'new';
      comp.confirmNewPassword = 'new';
      comp.onChangePassword();
      expect(authSpy.changePassword).toHaveBeenCalledWith('old', 'new');
    });

    it('should show success and clear fields on success', () => {
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.currentPassword = 'old';
      comp.newPassword = 'new';
      comp.confirmNewPassword = 'new';
      comp.onChangePassword();
      expect(comp.passwordSuccess).toContain('mis à jour');
      expect(comp.currentPassword).toBe('');
      expect(comp.newPassword).toBe('');
      expect(comp.confirmNewPassword).toBe('');
      expect(comp.isChangingPassword).toBeFalse();
    });

    it('should join error descriptions on failure', () => {
      authSpy.changePassword.and.returnValue(
        throwError(() => ({
          status: 400,
          error: [{ description: 'Mot de passe trop court' }],
        }))
      );
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.newPassword = 'x';
      comp.confirmNewPassword = 'x';
      comp.onChangePassword();
      expect(comp.passwordError).toContain('Mot de passe trop court');
    });

    it('should set generic passwordError for non-array error body', () => {
      authSpy.changePassword.and.returnValue(throwError(() => ({ status: 500, error: null })));
      const comp = TestBed.createComponent(UserAccountSettingsComponent).componentInstance;
      comp.newPassword = 'x';
      comp.confirmNewPassword = 'x';
      comp.onChangePassword();
      expect(comp.passwordError).toContain('500');
    });
  });
});
