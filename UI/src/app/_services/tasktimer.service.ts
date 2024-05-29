import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TasktimerService {

  baseUrl = environment.apiUrl;
  constructor(private http:HttpClient) { }

 
  /*
  gettimerdata(id:number):Observable<number> 
  {
     return this.http.get<number>(this.baseUrl+'TaskTimer/'+id);
  }
  */
}
