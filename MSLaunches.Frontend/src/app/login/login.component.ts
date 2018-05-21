import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { ISubscription } from 'rxjs/Subscription';

import { environment } from '@env/environment';
import { Logger, I18nService, AuthenticationService } from '@app/core';
import { Credentials } from 'crypto';

const log = new Logger('Login');

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  version: string = environment.version;
  error: string;
  logSubscription: ISubscription;
  isLoading = true;

  constructor(private router: Router,
              private formBuilder: FormBuilder,
              private i18nService: I18nService,
              private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    console.log(this.authenticationService);
    this.authenticationService.hashHandled.subscribe(() => {
      if (this.authenticationService.isAuthenticated()) {
        this.router.navigate(['/home'], {replaceUrl: true});
      } else {
        this.isLoading = false;
      }
    });
  }

  login() {
    this.isLoading = true;
    this.authenticationService.login();
  }

  setLanguage(language: string) {
    this.i18nService.language = language;
  }

  get currentLanguage(): string {
    return this.i18nService.language;
  }

  get languages(): string[] {
    return this.i18nService.supportedLanguages;
  }

}
