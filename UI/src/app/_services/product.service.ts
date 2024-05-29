import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { product } from '../_models/product';
import { HttpClient } from '@angular/common/http';
import { InsertedEntityStatus } from '../_models/InsertedEntityStatus';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  baseUrl = environment.apiUrl;
  constructor(private http:HttpClient) 
  { }

  getall(): Observable<product[]>
  {
     return this.http.get<product[]> (this.baseUrl+'Product');
  }

  add(item:product) : Observable<InsertedEntityStatus>
  {
     return this.http.post<InsertedEntityStatus>(this.baseUrl+'Product',item);
     
  }

 

  get (id:number) : Observable<product>
  {
      return this.http.get<product> (this.baseUrl+'Product/'+id);
  }

  update (id:number,item:product) : Observable<product>
  {
      return this.http.put<product>(this.baseUrl+'Product/'+id,item);
  }
  delete (id:number)  
  {
      return  this.http.delete(this.baseUrl+'Product/'+id);
  }
  
}
