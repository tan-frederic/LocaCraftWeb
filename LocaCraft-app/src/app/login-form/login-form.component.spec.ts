import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { of, throwError } from 'rxjs';

import { LoginFormComponent } from './login-form.component';
import { AuthService } from '../Services/auth.service';

describe('LoginFormComponent', () => {
  let authSpy: jasmine.SpyObj<AuthService>;

  function setup(isLoggedIn = false) {
    authSpy.isLoggedIn.and.returnValue(isLoggedIn);
    const fixture = TestBed.createComponent(LoginFormComponent);
    return { fixture, comp: fixture.componentInstance };
  }

  beforeEach(async () => {
    authSpy = jasmine.createSpyObj('AuthService', ['isLoggedIn', 'login', 'register']);
    authSpy.isLoggedIn.and.returnValue(false);
    authSpy.login.and.returnValue(of({ token: 'tok', expiration: '' }));
    authSpy.register.and.returnValue(of({ id: '1', email: 'a@b.c' }));

    await TestBed.configureTestingModule({
      imports: [LoginFormComponent],
      providers: [
        provideRouter([]),
        provideLocationMocks(),
        { provide: AuthService, useValue: authSpy },
      ],
    }).compileComponents();
  });

  it('should create', () => {
    expect(setup().comp).toBeTruthy();
  });

  it('should redirect to /home in constructor when already logged in', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    setup(true);
    expect(router.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should not redirect when not logged in', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    setup(false);
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should start in register mode (isLoginMode = false)', () => {
    expect(setup().comp.isLoginMode).toBeFalse();
  });

  it('toggleMode should switch mode and clear fields and messages', () => {
    const { comp } = setup();
    comp.email = 'test@test.com';
    comp.errorMessage = 'err';
    comp.successMessage = 'ok';
    comp.toggleMode();
    expect(comp.isLoginMode).toBeTrue();
    expect(comp.email).toBe('');
    expect(comp.errorMessage).toBe('');
    expect(comp.successMessage).toBe('');
  });

  it('onSubmit should do nothing if isSubmitting is true', () => {
    const { comp } = setup();
    comp.isSubmitting = true;
    comp.onSubmit();
    expect(authSpy.login).not.toHaveBeenCalled();
    expect(authSpy.register).not.toHaveBeenCalled();
  });

  it('onSubmit should set errorMessage when passwords do not match in register mode', () => {
    const { comp } = setup();
    comp.password = 'abc';
    comp.confirmPassword = 'xyz';
    comp.onSubmit();
    expect(comp.errorMessage).toContain('correspondent pas');
    expect(authSpy.register).not.toHaveBeenCalled();
  });

  describe('login mode', () => {
    it('should call authService.login with email and password', () => {
      const { comp } = setup();
      comp.isLoginMode = true;
      comp.email = 'a@b.c';
      comp.password = 'pass';
      comp.onSubmit();
      expect(authSpy.login).toHaveBeenCalledWith({ email: 'a@b.c', password: 'pass' });
    });

    it('should navigate to /home on successful login', () => {
      const router = TestBed.inject(Router);
      spyOn(router, 'navigate');
      const { comp } = setup();
      comp.isLoginMode = true;
      comp.onSubmit();
      expect(router.navigate).toHaveBeenCalledWith(['/home']);
    });

    it('should set 401 error message on 401 response', () => {
      authSpy.login.and.returnValue(throwError(() => ({ status: 401 })));
      const { comp } = setup();
      comp.isLoginMode = true;
      comp.onSubmit();
      expect(comp.errorMessage).toContain('incorrect');
      expect(comp.isSubmitting).toBeFalse();
    });

    it('should set generic error message for non-401 errors', () => {
      authSpy.login.and.returnValue(throwError(() => ({ status: 503 })));
      const { comp } = setup();
      comp.isLoginMode = true;
      comp.onSubmit();
      expect(comp.errorMessage).toContain('503');
    });
  });

  describe('register mode', () => {
    it('should call authService.register with email and password', () => {
      const { comp } = setup();
      comp.email = 'new@user.com';
      comp.password = 'secret';
      comp.confirmPassword = 'secret';
      comp.onSubmit();
      expect(authSpy.register).toHaveBeenCalledWith({ email: 'new@user.com', password: 'secret' });
    });

    it('should switch to login mode and show success message after registration', () => {
      const { comp } = setup();
      comp.password = 'p';
      comp.confirmPassword = 'p';
      comp.onSubmit();
      expect(comp.isLoginMode).toBeTrue();
      expect(comp.successMessage).toContain('succès');
      expect(comp.email).toBe('');
      expect(comp.password).toBe('');
    });

    it('should join validation error descriptions on register failure', () => {
      authSpy.register.and.returnValue(
        throwError(() => ({
          status: 400,
          error: [{ description: 'Erreur A' }, { description: 'Erreur B' }],
        }))
      );
      const { comp } = setup();
      comp.password = 'p';
      comp.confirmPassword = 'p';
      comp.onSubmit();
      expect(comp.errorMessage).toContain('Erreur A');
      expect(comp.errorMessage).toContain('Erreur B');
    });

    it('should set generic error when error body is not an array', () => {
      authSpy.register.and.returnValue(throwError(() => ({ status: 500, error: 'oops' })));
      const { comp } = setup();
      comp.password = 'p';
      comp.confirmPassword = 'p';
      comp.onSubmit();
      expect(comp.errorMessage).toContain('500');
    });
  });
});
