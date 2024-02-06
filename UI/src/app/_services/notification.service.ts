import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { notification } from '../_models/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {


  baseUrl = environment.apiUrl;
  
  constructor(private http:HttpClient) { }


  getUnreadForUser(userId:string) : Observable<notification[]>
  {
    return this.http.get<notification[]>(this.baseUrl+'Notification/'+userId);
  }
}
