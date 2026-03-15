import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';

import { AppComponent } from './app.component';
import { AuthService } from './Services/auth.service';

describe('AppComponent', () => {
  let authServiceSpy: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['isLoggedIn', 'logout']);
    authServiceSpy.isLoggedIn.and.returnValue(false);

    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [
        provideRouter([]),
        provideLocationMocks(),
        { provide: AuthService, useValue: authServiceSpy },
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it(`should have the 'LocaCraft-app' title`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    expect(fixture.componentInstance.title).toEqual('LocaCraft-app');
  });

  it('should render the LocaCraft brand link in the sidebar', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('.sidebar-brand')?.textContent?.trim()).toBe('LocaCraft');
  });

  it('should initialise accountMenuOpen to false', () => {
    const fixture = TestBed.createComponent(AppComponent);
    expect(fixture.componentInstance.accountMenuOpen).toBeFalse();
  });

  it('should toggle accountMenuOpen on toggleAccountMenu()', () => {
    const app = TestBed.createComponent(AppComponent).componentInstance;
    app.toggleAccountMenu();
    expect(app.accountMenuOpen).toBeTrue();
    app.toggleAccountMenu();
    expect(app.accountMenuOpen).toBeFalse();
  });

  it('should set accountMenuOpen to false on closeAccountMenu()', () => {
    const app = TestBed.createComponent(AppComponent).componentInstance;
    app.accountMenuOpen = true;
    app.closeAccountMenu();
    expect(app.accountMenuOpen).toBeFalse();
  });

  it('should delegate isLoggedIn to AuthService', () => {
    const app = TestBed.createComponent(AppComponent).componentInstance;
    authServiceSpy.isLoggedIn.and.returnValue(true);
    expect(app.isLoggedIn).toBeTrue();
    authServiceSpy.isLoggedIn.and.returnValue(false);
    expect(app.isLoggedIn).toBeFalse();
  });

  it('should call authService.logout() and navigate to /login on logout()', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    fixture.componentInstance.logout();
    expect(authServiceSpy.logout).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should show a login link when the user is not logged in', () => {
    authServiceSpy.isLoggedIn.and.returnValue(false);
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('a.account-btn')).toBeTruthy();
  });

  it('should show the account button when the user is logged in', () => {
    authServiceSpy.isLoggedIn.and.returnValue(true);
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('button.account-btn')).toBeTruthy();
  });
});
