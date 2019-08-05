import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Character } from 'js.src/model/character';
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root'
})
export class RestService {
  
  constructor(private httpClient: HttpClient) { }

  public getCharacterData(): Observable<Character[]> {
    return this.httpClient.get<Character[]>(`http://localhost:5000/api/character`);
  }
}
