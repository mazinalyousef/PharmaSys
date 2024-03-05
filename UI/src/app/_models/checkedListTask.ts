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
    durationInSeconds: number
    startDate?: Date
    endDate?: Date

    taskTypeCheckLists : taskTypeCheckList []

    batchInfo :batch
    productInfo:product


}