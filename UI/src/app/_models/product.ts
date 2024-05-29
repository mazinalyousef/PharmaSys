import { productIngredient } from "./productIngredient"

export interface product
{
        id: number
        productName: string
        productTypeId?: number
        productIngredients: productIngredient[]
      
}