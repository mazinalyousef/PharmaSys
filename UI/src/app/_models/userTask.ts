export interface userTask
{
    id: number
    taskStateId: number
    taskTypeId: number
    taskTypeTitle: string
    batchId: number
    batchNO: string
    batchSize: number
    productName: string
    tubeWeight: number
    tubesCount: number
    userId: string
    durationInSeconds: number
    startDate?: Date
    endDate?: Date
}