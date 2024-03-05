export interface message
{
    id:number;
    userId: string;
    userName: string;
    messageText: string;
    batchTaskId?: number;
    batchId?: number;
    batchNO: string;
    destinationUserId: string;
    isRead: boolean;
    dateSent?: Date;
    dateRead?: Date;
}