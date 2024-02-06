import { Injectable } from '@angular/core';
import { Department } from '../_models/department';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {


  public GlobalDeparments!: Department[];

  private departments$$ = new BehaviorSubject<Department[]>([]);
  public  departments$ = this.departments$$.asObservable();



    constructor()
    {
    }

    filldepartments(data:Department[])
    {
     this.departments$$.next(data);
    }

}
