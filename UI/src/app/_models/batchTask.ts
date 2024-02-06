export interface batchTask
{
    id: number
    taskStateId: number
    taskTypeId: number
    batchId: number
    departmentId?: number
    userId?: string
    durationInSeconds: number
    startDate: Date
    endDate?: Date
}