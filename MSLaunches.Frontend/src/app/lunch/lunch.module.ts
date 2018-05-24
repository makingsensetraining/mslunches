import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';
import { LunchRoutingModule } from './lunch-routing.module';
import { LunchComponent } from './lunch.component';
import { LunchService } from './lunch.service';
import { LunchTileComponent } from '@app/lunch/lunch-tile/lunch-tile.component';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    CoreModule,
    SharedModule,
    LunchRoutingModule
  ],
  declarations: [
    LunchComponent,
    LunchTileComponent
  ],
  exports: [
    LunchComponent,
    LunchTileComponent
  ],
  providers: [
    LunchService
  ]
})
export class LunchModule { }
