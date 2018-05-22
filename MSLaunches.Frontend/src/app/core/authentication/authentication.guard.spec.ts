import { TestBed, inject } from '@angular/core/testing';
import { Router } from '@angular/router';

import { AuthenticationService } from './authentication.service';
import { MockAuthenticationService } from './authentication.service.mock';
import { AuthenticationGuard } from './authentication.guard';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';

describe('AuthenticationGuard', () => {
  let authenticationGuard: AuthenticationGuard;
  let authenticationService: MockAuthenticationService;
  let mockRouter: any;

  beforeEach(() => {
    mockRouter = {
      navigate: jasmine.createSpy('navigate')
    };
    TestBed.configureTestingModule({
      providers: [
        AuthenticationGuard,
        { provide: AuthenticationService, useClass: MockAuthenticationService },
        { provide: Router, useValue: mockRouter },
      ]
    });
  });

  beforeEach(inject([
    AuthenticationGuard,
    AuthenticationService
  ], (_authenticationGuard: AuthenticationGuard,
      _authenticationService: MockAuthenticationService) => {

    authenticationGuard = _authenticationGuard;
    authenticationService = _authenticationService;
  }));

  it('should have a canActivate method', () => {
    expect(typeof authenticationGuard.canActivate).toBe('function');
  });

  it('should return true if user is authenticated', () => {
    authenticationService.hashHandled = Observable.create((obs: Observer<boolean>) => obs.next(true));
    authenticationGuard.canActivate().subscribe(res => {
      expect(res).toBe(true);
    });
  });

  it('should return false and redirect to login if user is not authenticated', () => {
    // Arrange
    authenticationService.hashHandled = Observable.create((obs: Observer<boolean>) => obs.next(true));
    authenticationService.credentials = null;

    // Act
    authenticationGuard.canActivate().subscribe(result => {
      // Assert
      expect(mockRouter.navigate).toHaveBeenCalledWith(['/login'], {replaceUrl: true});
      expect(result).toBe(false);
    });
  });
});
