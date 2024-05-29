import { batch } from "./batch";
import { checkedListTask } from "./checkedListTask";
import { rawMaterialsTask } from "./rawMaterialsTask";

export interface batchManufacturingRecord
{
    batchForEditDTO :batch
    checkedListTaskForViewDTOs : checkedListTask[];
    rawMaterialsTaskForViewDTOs : rawMaterialsTask[];
}