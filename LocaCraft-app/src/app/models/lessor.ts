import { Lease } from "./lease";

export interface Lessor{
    id: number;
    name: string;
    adress: string;
    city: string;
    postalCode: string;
    country: string;
    leases: Lease[];
}