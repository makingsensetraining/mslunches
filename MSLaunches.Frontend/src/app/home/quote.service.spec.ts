import { TestBed, inject, async } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { CoreModule } from '@app/core';
import { QuoteService } from './quote.service';
import { environment } from '@env/environment';

const credentials: any = {
  username: 'test',
  idToken: '123',
  accessToken: 'asd',
  expiresAt: 1000,
  tokenPayload: '',
  scopes: '',
  image: ''
};

describe('QuoteService', () => {
  let quoteService: QuoteService;
  let httpMock: HttpTestingController;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        CoreModule,
        HttpClientTestingModule
      ],
      providers: [
        QuoteService
      ]
    });
  }));

  beforeEach(inject([
    QuoteService,
    HttpTestingController
  ], (_quoteService: QuoteService,
      _httpMock: HttpTestingController) => {
    sessionStorage.setItem('credentials', JSON.stringify(credentials));
    quoteService = _quoteService;
    httpMock = _httpMock;
  }));

  afterEach(() => {
    httpMock.verify();
  });

  describe('getRandomQuote', () => {
    it('should return a random Chuck Norris quote', () => {
      // Arrange
      const mockQuote = { value: 'a random quote' };

      // Act
      const randomQuoteSubscription = quoteService.getRandomQuote({ category: 'toto' });

      // Assert
      randomQuoteSubscription.subscribe((quote: string) => {
        expect(quote).toEqual(mockQuote.value);
      });
      httpMock.expectOne({}).flush(mockQuote, {status: 200});
    });

    it('should return a string in case of error', () => {
      // Act
      const randomQuoteSubscription = quoteService.getRandomQuote({ category: 'toto' });
      // Assert
      randomQuoteSubscription.subscribe((quote: string) => {
        expect(typeof quote).toEqual('string');
        expect(quote).toContain('Error');
      });
      const req = httpMock.expectOne(`${environment.serverUrl}/users`);
      req.flush(null, {
        status: 500,
        statusText: 'error'
      });
    });
  });
});
