import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { barcode } from '../_models/barcode';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BarcodeService {

  baseUrl = environment.apiUrl;
   constructor(private http:HttpClient)
   { 

   }

   getBarcode(barcode:string)
  {
    return this.http.get<barcode>(this.baseUrl+'Barcode/'+barcode);
  }

   getall(): Observable<barcode[]>
   {
      return this.http.get<barcode[]> (this.baseUrl+'Barcode');
   }

   add(item:barcode) : Observable<barcode>
   {
      return this.http.post<barcode>(this.baseUrl+'Barcode',item);
      
   }

    

   update (id:number,item:barcode) : Observable<barcode>
   {
       return this.http.put<barcode>(this.baseUrl+'Barcode/'+id,item);
   }
   delete (id:number)  
   {
       return  this.http.delete(this.baseUrl+'Barcode/'+id);
   }

}
