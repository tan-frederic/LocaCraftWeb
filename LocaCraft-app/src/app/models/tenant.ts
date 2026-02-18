import { Lease } from "./lease";

export interface Tenant{
    id: number;
    leaseId: number;
    lease: Lease;
    name: string;
    surname: string;
    phoneNumber: string;
    email: string;
    address: string;
    city: string;
    postalCode: string;
    country: string;
}