import { batch } from "./batch"
import { product } from "./product"
import { taskTypeCheckList } from "./taskTypeCheckList"

export interface checkedListTask
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

    taskTypeCheckLists : taskTypeCheckList []

    batchInfo :batch
    productInfo:product


}