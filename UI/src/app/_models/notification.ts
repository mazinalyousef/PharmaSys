export interface notification
{
        id: number
        userId: string
        notificationMessage: string
        batchTaskId? : number
        batchId? : number
        assignedByUserId? : number
        isRead: boolean
        dateSent?: Date
        dateRead?: Date
}