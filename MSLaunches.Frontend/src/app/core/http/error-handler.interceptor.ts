import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { catchError } from 'rxjs/operators';

import { environment } from '@env/environment';
import { Logger } from '../logger.service';
import { AuthenticationService } from '@app/core';
import { Router } from '@angular/router';

const log = new Logger('ErrorHandlerInterceptor');

/**
 * Adds a default error handler to all requests.
 */
@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {
  private router: Router;
  private authenticationService: AuthenticationService;

  // circular dependency fix
  constructor(injector: Injector) {
    this.router = injector.get(Router);
    this.authenticationService = injector.get(AuthenticationService);
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(error => this.errorHandler(error)));
  }

  // Customize the default error handler here if needed
  private errorHandler(response: any): Observable<HttpEvent<any>> {
    if (!environment.production) {
      // Do something with the error
      log.error('Request error', response);
    }

    if (response instanceof HttpErrorResponse && response.status === 401) {
        this.authenticationService.logout();
        this.router.navigate(['/login'], {replaceUrl: true });
    }

    throw response;
  }

}
