import { ResourceType } from "../../files-drive/models/file-out";

export class ShareFilter {
    public resourceType: ResourceType;

    constructor(resourceType: ResourceType) {

        this.resourceType = resourceType;
    }
}
