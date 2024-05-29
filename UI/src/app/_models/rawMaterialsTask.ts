
import { batch } from "./batch"
import { batchIngredient } from "./batchIngredient"
import { product } from "./product"

export interface rawMaterialsTask
{
    id: number
    taskTypeId: number
    title: string
    batchId: number
    departmentId?: number
    userId?: string
    userName?:string
    durationInSeconds: number
    startDate?: Date
    endDate?: Date

    taskStateId :number;
    batchIngredientDTOs: batchIngredient[]

    
    batchInfo :batch
    productInfo:product
}