import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ingredient } from '../_models/ingredient';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class IngredientService

{
  baseUrl = environment.apiUrl;
  paginatedResult:PaginatedResult<ingredient[]>=new PaginatedResult<ingredient[]>();


  constructor(private http:HttpClient)
  {
    
  }
   getall(): Observable<ingredient[]>
   {
      return this.http.get<ingredient[]> (this.baseUrl+'Ingredient');
   }

   getpagedResult(page?:number,itemsPerPage?:number)
   {

    let params =  new HttpParams();
    if (page!==null && itemsPerPage!==null)
    {
      params=params.append('pageNumber',page.toString());
      params=params.append('pageSize',itemsPerPage.toString());
    }
    return this.http.get<ingredient[]> (this.baseUrl+'Ingredient/pagedlist',{observe:'response',params})
    .pipe(
      map
      (
        response=>
        {
          this.paginatedResult.result=response.body;
          if (response.headers.get('Pagination')!==null)
          {
            this.paginatedResult.pagination=JSON.parse(response.headers.get('Pagination'));
          }
          return this.paginatedResult;
        }
      )
    );
   }

   add(item:ingredient) : Observable<ingredient>
   {
      return this.http.post<ingredient>(this.baseUrl+'Ingredient',item);
      
   }

   get (id:number) : Observable<ingredient>
   {
       return this.http.get<ingredient> (this.baseUrl+'Ingredient/'+id);
   }

   update (id:number,item:ingredient) : Observable<ingredient>
   {
       return this.http.put<ingredient>(this.baseUrl+'Ingredient/'+id,item);
   }
   delete (id:number)  
   {
       return  this.http.delete(this.baseUrl+'Ingredient/'+id);
   }

}

