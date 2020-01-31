export class UserDiskSpace {

    public free: number;
    public used: number;
    public total: number;

    public freeUnit: Unit;
    public usedUnit: Unit;
    public totalUnit: Unit;

    public freeUnitString: string;
    public usedUnitString: string;
    public totalUnitString: string;
}

export enum Unit {

    Bytes = 1,
    kB = 1000,
    MB = 1000000,
    GB = 1000000000,
    TB = 1000000000000
}
