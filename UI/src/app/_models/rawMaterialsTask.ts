
import { batch } from "./batch"
import { batchIngredient } from "./batchIngredient"
import { batchTaskNote } from "./batchTaskNote"
import { message } from "./message"
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

    messages : message[]
}