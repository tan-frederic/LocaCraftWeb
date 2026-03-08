import { Lease } from "./lease";

export interface Lessor{
    id: number;
    firstName: string;
    lastName: string;
    address: string;
    city: string;
    postalCode: string;
    country: string;
    phone: string;
    email: string;
    leases: Lease[];
}