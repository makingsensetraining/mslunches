import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';

import { Credentials, LoginContext } from './authentication.service';

export class MockAuthenticationService {

  credentials: Credentials | null = {
    username: 'test',
    idToken: '123',
    accessToken: 'asd',
    expiresAt: 1000,
    tokenPayload: ""
  };

  login(context: LoginContext): Observable<Credentials> {
    return of({
      username: context.username,
      idToken: '123',
      accessToken: 'asd',
      expiresAt: 1000,
      tokenPayload : ""
    });
  }

  logout(): Observable<boolean> {
    this.credentials = null;
    return of(true);
  }

  isAuthenticated(): boolean {
    return !!this.credentials;
  }

}
