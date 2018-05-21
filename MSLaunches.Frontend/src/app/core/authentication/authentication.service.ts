import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { concat } from 'rxjs/observable/concat';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs/observable/of';
import { map, catchError } from 'rxjs/operators';
import * as auth0 from 'auth0-js';

import { auth0Config } from '@env/environment';
import { Observer } from 'rxjs/Observer';
import { merge } from 'rxjs/observable/merge';

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

  private _credentials: Credentials | null;
  private _isLogged = false;
  private _loggedSubscriber: Observable<boolean> = new Observable();

  constructor(private httpClient: HttpClient) {
    this._loggedSubscriber = Observable.create((observer: Observer<boolean>) => {
      if (!this.setupCredentials()) {
        if (this.handleHash()) {
          this.setCredentials();
        }
      }
      observer.next(this._isLogged);
    });
  }

  /**
   * Authenticates the user.
   */
  login() {
    auth0Config.authorize();
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
    if (!!this.credentials) {
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

  get isLogged(): boolean {
    return this._isLogged;
  }

  get loggedSubscriber(): Observable<boolean> {
    return this._loggedSubscriber;
  }


  private handleHash(): boolean {
    if (!this._isLogged && !this.isAuthenticated()) {
      auth0Config.parseHash((err: auth0.Auth0Error, authResult: auth0.Auth0DecodedHash) => {
        if (authResult && authResult.accessToken && authResult.idToken) {
          window.location.hash = '';
          this._isLogged = this.setCredentials(this.mapCredentials(authResult));
        } else {
          this._isLogged = false;
        }
      });
    }
    return this._isLogged;
  }

  private setupCredentials(): boolean {
    const savedCredentials = sessionStorage.getItem(credentialsKey) || localStorage.getItem(credentialsKey);
    if (savedCredentials) {
      this._credentials = JSON.parse(savedCredentials);
      this._isLogged = true;
    }

    return this._isLogged;
  }

  /**
   * Sets the user credentials.
   * The credentials may be persisted across sessions by setting the `remember` parameter to true.
   * Otherwise, the credentials are only persisted for the current session.
   * @param {Credentials=} credentials The user credentials.
   * @param {boolean=} remember True to remember credentials across sessions.
   */
  private setCredentials(credentials?: Credentials, remember?: boolean): boolean {
    this._credentials = credentials || null;

    if (credentials) {
      const storage = remember ? localStorage : sessionStorage;
      storage.setItem(credentialsKey, JSON.stringify(credentials));
      return true;
    } else {
      sessionStorage.removeItem(credentialsKey);
      localStorage.removeItem(credentialsKey);
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
