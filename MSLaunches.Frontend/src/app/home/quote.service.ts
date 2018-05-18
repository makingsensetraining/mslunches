import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { map, catchError } from 'rxjs/operators';

const routes = {
  quote: (c: RandomQuoteContext) => `/users`
};

export interface RandomQuoteContext {
  // The quote's category: 'dev', 'explicit'...
  category: string;
}

@Injectable()
export class QuoteService {

  constructor(private httpClient: HttpClient) { }

  getRandomQuote(context: RandomQuoteContext): Observable<string> {
    let creds = JSON.parse(sessionStorage.getItem('credentials')) || JSON.parse(localStorage.getItem('credentials'));

    return this.httpClient
      .cache()
      .get('/users', { headers : new HttpHeaders().append("Authorization", "Bearer " + creds.accessToken)})
      .pipe(
        map((body: any) => body.value),
        catchError(() => of('Error, could not load joke :-('))
      );
  }

}
