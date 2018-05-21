import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';
import { LaunchRoutingModule } from './launch-routing.module';
import { LaunchComponent } from './launch.component';
import { LaunchService } from './launch.service';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    CoreModule,
    SharedModule,
    LaunchRoutingModule
  ],
  declarations: [
    LaunchComponent
  ],
  providers: [
    LaunchService
  ]
})
export class LaunchModule { }
