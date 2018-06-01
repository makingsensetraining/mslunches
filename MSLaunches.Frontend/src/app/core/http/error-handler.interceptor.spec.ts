import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';

import { ErrorHandlerInterceptor } from './error-handler.interceptor';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthenticationService, MockAuthenticationService } from '@app/core';
import { catchError } from 'rxjs/operators';

describe('ErrorHandlerInterceptor', () => {
  let http: HttpClient;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule
      ],
      providers: [{
        provide: HTTP_INTERCEPTORS,
        useClass: ErrorHandlerInterceptor,
        multi: true
      },
      {
        provide: AuthenticationService,
        useClass: MockAuthenticationService
      }]
    });
  });

  beforeEach(inject([
    HttpClient,
    HttpTestingController
  ], (_http: HttpClient,
    _httpMock: HttpTestingController) => {

      http = _http;
      httpMock = _httpMock;
    }));

  afterEach(() => {
    httpMock.verify();
  });

  it('should catch error and call error handler', () => {
    // Arrange
    // Note: here we spy on private method since target is customization here,
    // but you should replace it by actual behavior in your app
    spyOn(ErrorHandlerInterceptor.prototype as any, 'errorHandler');
    // Act
    http.get('/toto').subscribe(
      () => { },
      // Assert|
      () => expect(ErrorHandlerInterceptor.prototype['errorHandler']).toHaveBeenCalled()
    );
    const req = httpMock.expectOne({});
    req.error(null, { status: 404 });

  });
});
