import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';
import { MenuRoutingModule } from './menu-routing.module';
import { MenuComponent } from './menu.component';
import { MenuService } from './menu.service';
import { MenuTileComponent } from './menu-tile/menu-tile.component';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    CoreModule,
    SharedModule,
    MenuRoutingModule
  ],
  declarations: [
    MenuComponent,
    MenuTileComponent
  ],
  exports: [
    MenuComponent,
    MenuTileComponent
  ],
  providers: [
    MenuService
  ]
})
export class MenuModule { }
