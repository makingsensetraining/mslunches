import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '@env/environment';

/**
 * Prefixes all requests with `environment.serverUrl`.
 */
@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor {

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const credentials = localStorage.getItem('credentials');
    let clonedRequest = request;
    if (credentials) {
      clonedRequest = request.clone({
        headers: request.headers.set('Authorization', 'bearer ' + JSON.parse(credentials).accessToken)
      });
    }
    return next.handle(clonedRequest);
  }
}
