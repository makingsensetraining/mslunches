import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';

import { Credentials, LoginContext } from './authentication.service';

export class MockAuthenticationService {

  hashHandled: Observable<boolean>;
  credentials: Credentials | null = {
    username: 'test',
    idToken: '123',
    accessToken: 'asd',
    expiresAt: 1000,
    tokenPayload: '',
    scopes: '',
    image: ''
  };

  login() {
    this.credentials = {
      accessToken: '',
      expiresAt: 1,
      idToken: '',
      image: '',
      scopes: '',
      tokenPayload: '',
      username: '',
    };
  }

  logout(): Observable<boolean> {
    this.credentials = null;
    return of(true);
  }

  isAuthenticated(): boolean {
    return !!this.credentials;
  }

}
