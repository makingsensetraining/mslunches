import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { concat } from 'rxjs/observable/concat';
import { of } from 'rxjs/observable/of';
import { map, catchError, timeout, timeInterval } from 'rxjs/operators';
import * as auth0 from 'auth0-js';

import { auth0Config } from '@env/environment';
import { Observer } from 'rxjs/Observer';
import { merge } from 'rxjs/observable/merge';
import { promise } from 'protractor';
import { Subject } from 'rxjs/Subject';

export interface Credentials {
  // Customize received credentials here
  username: string;
  image: string;
  accessToken: string;
  idToken: string;
  expiresAt: Number;
  tokenPayload: string;
  scopes: string;
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

  private _hashHandled: Subject<boolean> = new Subject<boolean>();
  private _timeout: NodeJS.Timer;
  constructor() {
  }

  /**
   * Authenticates the user.
   */
  login() {
    auth0Config.authorize();
  }

  /**
   * Checks is the user is authenticated.
   * @return {boolean} True if the user is authenticated.
   */
  isAuthenticated(): boolean {
    if (!!this.credentials) {
      return new Date().getTime() < this.credentials.expiresAt;
    }

    return false;
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
   * gets the auth0 hash, and saves it into a session storage
   */
  handleHash() {
    if (!this.isAuthenticated()) {
      auth0Config.parseHash((err: auth0.Auth0Error, authResult: auth0.Auth0DecodedHash) => {
        if (authResult && authResult.accessToken && authResult.idToken) {
          window.location.hash = '';
          this.setCredentials(this.mapCredentials(authResult));
        }
        this._hashHandled.next(true);
      });
    }
  }

  setUpTimeout() {
    this._timeout = setTimeout(() => {
      this._hashHandled.next(true);
    }, 2000);
  }

  cleanTimeout() {
    clearTimeout(this._timeout);
  }

  /**
   * Gets the user credentials.
   * @return {Credentials} The user credentials or null if the user is not authenticated.
   */
  get credentials(): Credentials | null {
    return JSON.parse(sessionStorage.getItem(credentialsKey));
  }

  /**
   * returns an event representing whether the hash was handled or not
   */
  get hashHandled(): Subject<boolean> {
    return this._hashHandled;
  }

  /**
   * Sets the user credentials.
   * The credentials may be persisted across sessions by setting the `remember` parameter to true.
   * Otherwise, the credentials are only persisted for the current session.
   * @param {Credentials=} credentials The user credentials.
   * @param {boolean=} remember True to remember credentials across sessions.
   */
  private setCredentials(credentials?: Credentials): boolean {
    if (credentials) {
      const storage = sessionStorage;
      storage.setItem(credentialsKey, JSON.stringify(credentials));
      this._hashHandled.next(true);
      return true;
    } else {
      sessionStorage.removeItem(credentialsKey);
      localStorage.removeItem(credentialsKey);
      this._hashHandled.next(true);
      return false;
    }
  }

  private mapCredentials(auth0Result: auth0.Auth0DecodedHash): Credentials {
    const creds: Credentials = {
      username: auth0Result.idTokenPayload.nickname,
      image: auth0Result.idTokenPayload.picture,
      idToken: auth0Result.idToken,
      expiresAt: (auth0Result.expiresIn * 1000) + new Date().getTime(),
      accessToken: auth0Result.accessToken,
      tokenPayload: auth0Result.idTokenPayload,
      scopes: auth0Result.scope
    };
    return creds;
  }
}
