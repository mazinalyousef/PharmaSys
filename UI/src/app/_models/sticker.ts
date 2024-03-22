import { batchIngredient } from "./batchIngredient"

export interface sticker
{
    batchNo:string
    barcode:string
    productName:string
    
    ingredients : batchIngredient[];
}