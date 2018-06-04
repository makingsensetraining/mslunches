import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { CoreModule } from '@app/core';
import { LunchRoutingModule } from '@app/lunch/lunch-routing.module';
import { LunchTileComponent } from '@app/lunch/lunch-tile/lunch-tile.component';
import { LunchComponent } from '@app/lunch/lunch.component';
import { LunchService } from '@app/lunch/lunch.service';
import { SharedModule } from '@app/shared';




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
