import { Injectable } from '@angular/core';
import { sticker } from '../_models/sticker';
import { batchIngredient } from '../_models/batchIngredient';

@Injectable({
  providedIn: 'root'
})
export class StickersService {

  sticker :sticker;
  repeatCount:number;
  constructor() 
  {
     // initialize the sticker variable....
      this.sticker = {} as sticker;
      this.repeatCount=1;
  }

  setSticker(_batchNo:string,_barcode:string,_productName:string,_ingredients:batchIngredient[],_repeatCount:number)
  {
    this.sticker.batchNo = _batchNo;
    this.sticker.barcode= _barcode;
    this.sticker.productName = _productName;
    this.sticker.ingredients = _ingredients;
    this.repeatCount = _repeatCount;

  }
}
