import { taskTypeRange } from "./taskTypeRange"

export interface rangeSelectTask
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

    taskTypeRangeDTOs : taskTypeRange[]
}