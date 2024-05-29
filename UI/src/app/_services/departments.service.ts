import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Department } from '../_models/department';

@Injectable({
  providedIn: 'root'
})
export class DepartmentsService {

  baseUrl = environment.apiUrl;


  constructor(private http:HttpClient) { }

  getDepartments():Observable<Department[]>
  {
     return this.http.get<Department[]>(this.baseUrl+'Departments');
  }

  

}
