import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from './list/list.component';


const routes: Routes = [
  { path: 'Person', redirectTo: 'Person/list', pathMatch: 'full' },
  { path: 'Person/list', component: ListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PersonRoutingModule { }
