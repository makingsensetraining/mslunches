import { TestBed, inject, fakeAsync, tick } from '@angular/core/testing';

import { AuthenticationService, Credentials} from './authentication.service';
import { auth0Config } from '@env/environment';

const credentialsKey = 'credentials';
const aHash =
  'access_token=eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1VUTFSVVF4TkRsQ01FTkRPRU'
+ 'pETVVSRVEwUkZNVGRCT1RnMVJFRTRNRVEwUXpWRU56VTBPUSJ9'
+ '.eyJpc3MiOiJodHRwczovL21zbGF1bmNoZXMuYXV0aDAuY29tLyIsInN1YiI6Imdvb2dsZS1vYXV0aDJ8MT'
+ 'AwNzg4MzgwOTgyNDMwMjE1ODk2IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NDk5My9hcGkiLCJodHRwc'
+ 'zovL21zbGF1bmNoZXMuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTUyNjkyMjQ0NCwiZXhwIjoxNTI2'
+ 'OTI5NjQ0LCJhenAiOiJmN0ZtZEFpd0xhak13Y2VQRkxmQlE2UVdPM3E2VXZHcCIsInNjb3BlIjoib3Blbmlk'
+ 'IHByb2ZpbGUgZW1haWwifQ'
+ '.FJLBE-RcqZH4_EGy0VV4ONWkaAS9JtFiWcTRJTa0_0z2nnSQCdAJ_4c8_6N4tn78Oud6ooITXT5H3_VX_pi'
+ 'waEw34yfHjk573K0CyEzxxoyFRTPX-x3Olh7c0NeU_Vvi3jDCN53ZU8M096QzzCD4d2lY67u2ERkk1RkcMBX'
+ 'hsVwvtum75bw0mikc8iGC7o2G53-Zk_aBa011D1BS8Odgr3eEZuzL3qm5zCOsl2ZM1ichjFYKTIR36X9Pczf'
+ 'SoaSAKOaff_4hZD5SB66uRfMGMtoOPRWJQoIX9mXYBNjhdkNqX0w0LppMPcCn6xoBy1m5tppha3dYNaiqelX'
+ 'cex-jEQ'
+ '&expires_in=7200'
+ '&token_type=Bearer'
+ '&state=UTDs3i8q5ArkN_QM~NZ4NKNn6hQciov2'
+ '&id_token=eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik1VUTFSVVF4TkRsQ01FTkRPRUpETVV'
+ 'SRVEwUkZNVGRCT1RnMVJFRTRNRVEwUXpWRU56VTBPUSJ9'
+ '.eyJnaXZlbl9uYW1lIjoiR2FzdG9uIiwiZmFtaWx5X25hbWUiOiJDZXJpb25pIiwibmlja25hbWUiOiJkb25n'
+ 'YXRhc29mdCIsIm5hbWUiOiJHYXN0b24gQ2VyaW9uaSIsInBpY3R1cmUiOiJodHRwczovL2xoMy5nb29nbGV1c'
+ '2VyY29udGVudC5jb20vLWs5MmpUa0RoaFI0L0FBQUFBQUFBQUFJL0FBQUFBQUFBQUMwL25zSllMbndKTGZZL3'
+ 'Bob3RvLmpwZyIsImdlbmRlciI6Im1hbGUiLCJsb2NhbGUiOiJlcy00MTkiLCJ1cGRhdGVkX2F0IjoiMjAxOC0'
+ 'wNS0yMVQxNzowNzoyNC43MjZaIiwiZW1haWwiOiJkb25nYXRhc29mdEBnbWFpbC5jb20iLCJlbWFpbF92ZXJp'
+ 'ZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9tc2xhdW5jaGVzLmF1dGgwLmNvbS8iLCJzdWIiOiJnb29nbGUtb'
+ '2F1dGgyfDEwMDc4ODM4MDk4MjQzMDIxNTg5NiIsImF1ZCI6ImY3Rm1kQWl3TGFqTXdjZVBGTGZCUTZRV08zcT'
+ 'ZVdkdwIiwiaWF0IjoxNTI2OTIyNDQ0LCJleHAiOjE1MjY5NTg0NDQsImF0X2hhc2giOiJ0bUtCODZrcEhORnc0'
+ 'RzRyRVBFc2RBIiwibm9uY2UiOiJNOWlYcWtTdFN4amNQU2xYRFVySC05S1JsdDhESEFObSJ9'
+ '.uj4mTpxhubRGUzidxv8JXwyNI5EHxu2kA6APGwcdb9K2nMGtLVULyEdzBTx0jhBawQggiuNeDSQfR6OmBM3LM'
+ 'JguUCAwMGD5B8AnPvkihTcWM1OK7_hABrux7vN91ZgXFrYDIW4bADnx3WJ3POvzrf3qlLBGGon8rTD8YT7D42n'
+ 'PYpXC8zb7zjyxhXyBtNIAp4O68KrFqDF0JITXf6lvfBOYjkwwi-NJJ9RCl7i9VOBQuvPP6z-LL8K8ci8Jboz9x'
+ 'seyPU3SWSOHCRXzxgOooNImfFJDqbGoYdnqXt8pWzGSZMWshs1DuIhACK0i-IjGOcoyEkMm-zrxWGqaLZ_kDA';


describe('AuthenticationService', () => {
  let authenticationService: AuthenticationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthenticationService]
    });
  });

  beforeEach(inject([
    AuthenticationService
  ], (_authenticationService: AuthenticationService) => {
    authenticationService = _authenticationService;
  }));

  describe('login', () => {
    it('should call auth0 config', fakeAsync(() => {
      // Arrenge
      let numberOfCalls = 0;
      auth0Config.authorize = function() { numberOfCalls++; };

      authenticationService.login();
      // Assert
      expect(numberOfCalls)
        .toBe(1, 'authorize method was called once');
    }));
  });

  describe('logout', () => {
    it('should clear user authentication', fakeAsync(() => {
      // Arrange
      authenticationService = new AuthenticationService();
      window.location.hash = aHash;
      const loginRequest = authenticationService.handleHash();
      tick();

      // Assert
      authenticationService.hashHandled.subscribe(() => {
        expect(authenticationService.isAuthenticated()).toBe(true);
        const request = authenticationService.logout();
        tick();

        request.subscribe(() => {
          expect(authenticationService.isAuthenticated()).toBe(false);
          expect(authenticationService.credentials).toBeNull();
          expect(sessionStorage.getItem(credentialsKey)).toBeNull();
          expect(localStorage.getItem(credentialsKey)).toBeNull();
          window.location.hash = '';
        });
      });
    }));
  });
});
