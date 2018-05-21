import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { Logger } from '../logger.service';
import { AuthenticationService } from './authentication.service';
import { of } from 'rxjs/observable/of';
import { Observer } from 'rxjs/Observer';

const log = new Logger('AuthenticationGuard');

@Injectable()
export class AuthenticationGuard implements CanActivate {

  constructor(private router: Router,
              private authenticationService: AuthenticationService) { }

  canActivate(): Observable<boolean> {
    return Observable.create((observer: Observer<boolean>) => {
      this.authenticationService.loggedSubscriber.subscribe((res) => {
        if (!res) {
          log.debug('Not authenticated, redirecting...');
          this.router.navigate(['/login'], { replaceUrl: true });
        }

        observer.next(res);
      });
    });
  }
}
