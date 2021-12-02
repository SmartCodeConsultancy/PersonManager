import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';

import { PersonRoutingModule } from './person-routing.module';
import { ListComponent } from './list/list.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [ListComponent],
  imports: [
    CommonModule,
    PersonRoutingModule,
    FormsModule,
    ReactiveFormsModule,
  ]
})

export class PersonModule { }
