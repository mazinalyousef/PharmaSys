
import { batchIngredient } from "./batchIngredient"

export interface rawMaterialsTask
{
    id: number
    taskTypeId: number
    title: string
    batchId: number
    departmentId?: number
    userId?: string
    durationInSeconds: number
    startDate?: Date
    endDate?: Date

    batchIngredientDTOs: batchIngredient[]
}