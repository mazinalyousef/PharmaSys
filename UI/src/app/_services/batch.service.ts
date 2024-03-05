import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { batch } from '../_models/batch';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class BatchService {

   baseUrl = environment.apiUrl;
   constructor(private http:HttpClient)
   { 

   }
   getBatches(): Observable<batch[]>
   {
      return this.http.get<batch[]> (this.baseUrl+'Batches');
   }

   addBatch(batch:batch) : Observable<batch>
   {
      return this.http.post<batch>(this.baseUrl+'Batches',batch);
      
   }

   getBatch (id:number) : Observable<batch>
   {
       return this.http.get<batch> (this.baseUrl+'Batches/'+id);
   }

   updateBatch (id:number,batch:batch) : Observable<batch>
   {
       return this.http.put<batch>(this.baseUrl+'Batches/'+id,batch);
   }
   deleteBatch (id:number)  
   {
       return  this.http.delete(this.baseUrl+'Batches/'+id);
   }

   sendBatch(id:number) : Observable<boolean>
   {
       return this.http.post<boolean>(this.baseUrl+'Batches/send/'+id,null);
   }




}
