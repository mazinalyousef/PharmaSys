import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { barcode } from '../_models/barcode';

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

}
