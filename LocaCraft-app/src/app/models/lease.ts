import { Lessor } from "./lessor";
import { RealEstateAsset } from "./real-estate-assets";
import { Tenant } from "./tenant";

export interface Lease{
    id: number;
    realEstateAssetId: number;
    realEstateAsset?: RealEstateAsset;
    lessorId: number;
    lessor?: Lessor;
    leaseName: string;
    monthlyRent: number;
    monthlyCharges: number;
    deposit: number;
    rentIndexReference: number;
    tenants?: Tenant[];
    startDate: Date;
    endDate: Date;
}
