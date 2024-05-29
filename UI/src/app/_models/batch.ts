import { batchIngredient } from "./batchIngredient"

export interface batch
{
    id:number
    batchNO: string
    batchSize: number
    mFgDate?: Date
    expDate?: Date
    revision: string
    revisionDate?: Date
    barcode: string
    mfno: string
    ndcno: string
    productId: number
    productName : string
    userId: string
    batchStateId: number
    startDate?: Date
    endDate?: Date
    tubePictureURL: string
    cartoonPictureURL: string
    tubeWeight: number
    tubesCount: number
    cartoonsCount?:number
    masterCasesCount?:number
    batchIngredients: batchIngredient []
}
