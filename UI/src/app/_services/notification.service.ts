import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { notification } from '../_models/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {



  baseUrl = environment.apiUrl;
  
  constructor(private http:HttpClient) { }


  getallForUser(userId:string) : Observable<notification[]>
  {
    return this.http.get<notification[]>(this.baseUrl+'Notification/'+userId);
  }

  getallunreadForUser(userId:string) :Observable<notification[]>
  {
    return this.http.get<notification[]>(this.baseUrl+'Notification/Unread/'+userId);
  }
  setAllasRead(userId:string):Observable<boolean>
  {
    return this.http.put<boolean>(this.baseUrl+'Notification/'+userId,null);
  }
}
