import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { message } from '../_models/message';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService
 {

  baseUrl = environment.apiUrl;
  
  constructor(private http:HttpClient) { }

  getallForUser(userId:string) : Observable<message[]>
  {
     
    return this.http.get<message[]>(this.baseUrl+'Message/'+userId);
    
  }
  getallunreadForUser(userId:string) :Observable<number>
  {
     const xx:string= this.baseUrl+'Message/UnreadCount/'+userId;
     console.log("getallunreadForUser URL:"+xx);
    return this.http.get<number>(this.baseUrl+'Message/UnreadCount/'+userId);
    
  }

  // the return value must be changed in the API Controller .....or here
  add(item:message):Observable<message>
  {
     return this.http.post<message>(this.baseUrl+'Message',item);
  }
  setAllasRead(userId:string):Observable<boolean>
  {
   
    return this.http.put<boolean>(this.baseUrl+'Message/'+userId,null);
     
  }
}
