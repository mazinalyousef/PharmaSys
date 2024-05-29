import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EmailMessage } from '../_models/EmailMessage';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  baseUrl = environment.apiUrl;
  constructor(private http:HttpClient) 
  { 

  }
  sendEmail(emailmessage:EmailMessage):Observable<boolean>
  {
    return this.http.post<boolean>(this.baseUrl+'EmailMessage',emailmessage);
  }
  getFirstManager():Observable<User>
  {
    return  this.http.get<User>(this.baseUrl+'EmailMessage/FirstManager');
  }
}
