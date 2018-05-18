import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs/observable/of';
import { map, catchError } from 'rxjs/operators';
import * as auth0 from 'auth0-js';

import { auth0Config } from '@env/environment';

export interface Credentials {
  // Customize received credentials here
  username: string;
  accessToken: string;
  idToken: string;
  expiresAt: Number;
  tokenPayload: string;
}

export interface LoginContext {
  username: string;
  password: string;
  remember?: boolean;
}

const credentialsKey = 'credentials';

const routes = {
  login : '/users/token'
};

/**
 * Provides a base for authentication workflow.
 * The Credentials interface as well as login/logout methods should be replaced with proper implementation.
 */
@Injectable()
export class AuthenticationService {

  private _credentials: Credentials | null;

  constructor(private httpClient: HttpClient) {
    const savedCredentials = sessionStorage.getItem(credentialsKey) || localStorage.getItem(credentialsKey);
    if (savedCredentials) {
      this._credentials = JSON.parse(savedCredentials);
    }
  }

  /**
   * Authenticates the user.
   * @param {LoginContext} context The login parameters.
   * @return {Observable<Credentials>} The user credentials.
   */
  login() {
    auth0Config.authorize();
  }

  handleHash():void{
    auth0Config.parseHash((err, authResult) => {
      if (authResult && authResult.accessToken && authResult.idToken) {
        window.location.hash = '';
        this.setCredentials(this.mapCredentials(authResult));
      } else if (err) {
        console.log(err);
      }
    });
  }
  
  /**
   * Logs out the user and clear credentials.
   * @return {Observable<boolean>} True if the user was logged out successfully.
   */
  logout(): Observable<boolean> {
    // Customize credentials invalidation here
    this.setCredentials();
    return of(true);
  }

  /**
   * Checks is the user is authenticated.
   * @return {boolean} True if the user is authenticated.
   */
  isAuthenticated(): boolean {
    if(!!this.credentials){
      return new Date().getTime() < this.credentials.expiresAt;
    }

    return false;
  }

  /**
   * Gets the user credentials.
   * @return {Credentials} The user credentials or null if the user is not authenticated.
   */
  get credentials(): Credentials | null {
    return this._credentials;
  }

  /**
   * Sets the user credentials.
   * The credentials may be persisted across sessions by setting the `remember` parameter to true.
   * Otherwise, the credentials are only persisted for the current session.
   * @param {Credentials=} credentials The user credentials.
   * @param {boolean=} remember True to remember credentials across sessions.
   */
  private setCredentials(credentials?: Credentials, remember?: boolean) {
    this._credentials = credentials || null;

    if (credentials) {
      const storage = remember ? localStorage : sessionStorage;
      storage.setItem(credentialsKey, JSON.stringify(credentials));
    } else {
      sessionStorage.removeItem(credentialsKey);
      localStorage.removeItem(credentialsKey);
    }
  }

  private mapCredentials(auth0Result:auth0.Auth0DecodedHash):Credentials{
    let creds : Credentials = {
      username: "username",
      idToken: auth0Result.idToken,
      expiresAt: (auth0Result.expiresIn * 1000) + new Date().getTime(),
      accessToken: auth0Result.accessToken,
      tokenPayload: auth0Result.idTokenPayload
    };
    return creds;
  }
}
