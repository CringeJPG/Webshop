import { Login } from "./login";

export interface Customer {
    customerId: number;
    loginId: number;
    firstName: string;
    lastName: string;
    address: string;
    created: Date;
    login: Login;
}
