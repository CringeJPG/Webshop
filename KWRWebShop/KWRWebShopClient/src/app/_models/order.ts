import { Customer } from "./customer";
import { Orderline } from "./orderline";

export interface Order {
    orderId: number;
    created: Date;
    customerId: number;
    orderline: Orderline[];
    customer: Customer;
}
