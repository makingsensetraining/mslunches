import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient, HttpInterceptor } from '@angular/common/http';

import { HttpService } from './http.service';
import { ErrorHandlerInterceptor } from './error-handler.interceptor';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthenticationService, MockAuthenticationService } from '@app/core';

describe('HttpService', () => {
  let http: HttpClient;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientTestingModule
      ],
      providers: [
        ErrorHandlerInterceptor,
        {
          provide: HttpClient,
          useClass: HttpService
        },
        {
          provide: AuthenticationService,
          useClass: MockAuthenticationService
        }
      ]
    });
  });

  beforeEach(inject([
    HttpClient,
    HttpTestingController,
  ], (_http: HttpClient,
    _httpMock: HttpTestingController) => {

      http = _http;
      httpMock = _httpMock;
    }));

  afterEach(() => {
    httpMock.verify();
  });

  it('should use error handler and no cache by default', () => {
    // Arrange
    let interceptors: HttpInterceptor[];
    const realRequest = http.request;
    spyOn(HttpService.prototype, 'request').and.callFake(function (this: any) {
      interceptors = this.interceptors;
      return realRequest.apply(this, arguments);
    });

    // Act
    const request = http.get('/toto');

    // Assert
    request.subscribe(() => {
      expect(http.request).toHaveBeenCalled();
      expect(interceptors.some(i => i instanceof ErrorHandlerInterceptor)).toBeTruthy();
    });
    httpMock.expectOne({}).flush({});
  });

  it('should skip error handler', () => {
    // Arrange
    let interceptors: HttpInterceptor[];
    const realRequest = http.request;
    http = http.skipErrorHandler();
    spyOn(HttpService.prototype, 'request').and.callFake(function (this: any) {
      interceptors = this.interceptors;
      return realRequest.apply(this, arguments);
    });

    // Act
    const request = http
      .skipErrorHandler()
      .get('/toto');

    // Assert
    request.subscribe(() => {
      expect(interceptors.some(i => i instanceof ErrorHandlerInterceptor)).toBeFalsy();
    });
    httpMock.expectOne({}).flush({});
  });
});
