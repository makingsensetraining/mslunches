import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { LunchModule } from '@app/lunch/lunch.module';
import { LunchComponent } from '@app/lunch/lunch.component';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    CoreModule,
    SharedModule,
    HomeRoutingModule,
    LunchModule
  ],
  declarations: [
    HomeComponent,
  ],
  providers: [
  ]
})
export class HomeModule { }
