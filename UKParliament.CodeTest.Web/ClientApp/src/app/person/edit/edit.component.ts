import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators  } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Person } from "../person";
import { PersonService } from "../person.service";

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit {

  id: number;
  person: Person;
  editForm;

  constructor(
    public personService: PersonService,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder
  ) {
    this.editForm = this.formBuilder.group({
      id: [''],
      name: ['', Validators.required],
      address:[''],
      dateOfBirth: [''],
      gender: [''],
      email: [''],
      phone: ['']
    });
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['personId'];

    this.personService.getPerson(this.id).subscribe((data: Person) => {
      this.person = data;
      this.editForm.patchValue(data);
    });
  }

  onSubmit(formData) {
    console.log(formData.value);
    this.personService.updatePerson(this.id, formData.value).subscribe(res => {
      this.router.navigateByUrl('person/list');
    });
  }
}
