import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import {catchError} from 'rxjs/operators';

import { Person } from "./person";
import { identifierModuleUrl } from "@angular/compiler";

@Injectable({
    providedIn:'root'
})
export class PersonService{

    private apiURL = "http://localhost:5001/api";
    httpOptions ={
        headers: new HttpHeaders({
            'Content-Type' : 'application/json'
        })
    };

    constructor(private httpClient: HttpClient){}

    getPersons(): Observable<Person[]>{
        return this.httpClient.get<Person[]>(
            this.apiURL + '/person/persons')
            .pipe(catchError(this.errorHandler));
    }

    getPerson(id): Observable<Person>{
        return this.httpClient.get<Person>(
            this.apiURL + '/person/' + id)
            .pipe(catchError(this.errorHandler));
    }

    createPerson(person) : Observable<Person>{
        return this.httpClient.post<Person>(
            this.apiURL + '/person/', JSON.stringify(person), this.httpOptions)
            .pipe(catchError(this.errorHandler));
    }

    updatePerson(id, person): Observable<Person>{
        return this.httpClient.put<Person>(
            this.apiURL + '/person/' + id, JSON.stringify(person), this.httpOptions)
            .pipe(catchError(this.errorHandler));
    }

    deletePerson(id){
        return this.httpClient.delete<Person>(
            this.apiURL + '/person/' + id, this.httpOptions)
            .pipe(catchError(this.errorHandler));
    }

    errorHandler(error) {
        let errorMessage = '';

        if (error.error instanceof ErrorEvent) {
          errorMessage = error.error.message;
        } else {
          errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
        return throwError(errorMessage);
      }

}
