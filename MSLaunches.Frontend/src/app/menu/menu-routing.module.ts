import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { Route, extract } from '@app/core';
import { MenuComponent } from './menu.component';

const routes: Routes = [
  Route.withShell([
    { path: 'menu', component: MenuComponent, data: { title: extract('Menu') } }
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class MenuRoutingModule { }
