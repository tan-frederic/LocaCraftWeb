import { Lease } from "./lease";

export interface RealEstateAsset {
    id: number;
    name: string;
    description: string;
    address: string;
    addressComplement: string;
    postalCode: string;
    city: string;
    country: string;
    leases: Lease[]
}