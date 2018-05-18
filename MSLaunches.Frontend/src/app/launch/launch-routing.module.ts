import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { Route, extract } from '@app/core';
import { LaunchComponent } from './launch.component';

const routes: Routes = [
  Route.withShell([
    { path: '', redirectTo: '/launch', pathMatch: 'full' },
    { path: 'launch', component: LaunchComponent, data: { title: extract('Launch') } }
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class LaunchRoutingModule { }
