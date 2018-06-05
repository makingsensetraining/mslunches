import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Route, extract } from '@app/core';
import { LunchComponent } from '@app/lunch/lunch.component';


const routes: Routes = [
  Route.withShell([
    { path: 'lunch', component: LunchComponent, data: { title: extract('Lunch') } }
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class LunchRoutingModule { }
