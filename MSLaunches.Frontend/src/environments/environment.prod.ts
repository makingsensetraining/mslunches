// `.env.ts` is generated by the `npm run env` command
import env from './.env';
import { WebAuth } from 'auth0-js';

export const environment = {
  production: true,
  version: env.npm_package_version,
  serverUrl: 'https://mslunchesapi.azurewebsites.net/api',
  defaultLanguage: 'en-US',
  supportedLanguages: [
    'en-US',
    'fr-FR',
    'es-AR'
  ]
};

export const auth0Config = new WebAuth({
  clientID: 'f7FmdAiwLajMwcePFLfBQ6QWO3q6UvGp',
  domain: 'mslaunches.auth0.com',
  responseType: 'token id_token',
  audience: environment.serverUrl,
  redirectUri: 'http://mslunches.azurewebsites.net/',
  scope: 'openid profile'
});
