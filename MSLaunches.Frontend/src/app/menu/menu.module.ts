import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

import { MenuRoutingModule } from './menu-routing.module';
import { MenuTileComponent } from './menu-tile/menu-tile.component';
import { MenuComponent } from './menu.component';
import { MenuService } from './menu.service';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    CoreModule,
    SharedModule,
    MenuRoutingModule,
    FormsModule,
    NgbTypeaheadModule
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
