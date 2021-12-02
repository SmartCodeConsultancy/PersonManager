import { Component, OnInit } from '@angular/core';
import { Person } from '../person';
import { PersonService } from '../person.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  persons: Person[] = [];

  constructor(public personService: PersonService) { }

  ngOnInit(): void {
    this.personService.getPersons().subscribe((data:Person[])=> {
      this.persons = data;
    });
  }
  
  deletePerson(id){
    this.personService.deletePerson(id).subscribe(res => {
      this.persons = this.persons.filter(item => item.id !== id);
    });
  }
}
