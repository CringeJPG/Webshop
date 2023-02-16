import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Order } from './_models/order';
import { Orderline } from './_models/orderline';
import { OrderService } from './_services/order.service';
import { OrderlineService } from './_services/orderline.service';
import { Customer } from './_models/customer';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div >
      <h1>Customer Details</h1>
      <p>Customer ID: {{order.customerId}}</p>
      <p>Full Name: {{order.customer.firstName}} {{order.customer.lastName}}</p>
      <p>Address: {{order.customer.address}}</p>
    </div>
    <h1>Order Details</h1>
    <div class="orderdetails" *ngFor="let orderline of order.orderline">
      <h3>Orderline ID: {{orderline.orderlineId}}</h3>
      <p>Product ID: {{orderline.productId}}</p>
      <p>Price: {{orderline.price | currency:'kr'}}</p>
      <p>Amount: {{orderline.amount}}

    </div>
  `,
  styleUrls: ['./order-details.component.css'],
})
export class OrderDetailsComponent implements OnInit {
  customer: Customer = { customerId: 0, loginId: 0, firstName: '', lastName: '', address: '', created: new Date, login: { loginId: 0, email: '', password: '' } };
  order: Order = { orderId: 0, created: new Date, customerId: 0, orderline: [], customer: this.customer }

  constructor(private orderService: OrderService, private route: ActivatedRoute, private orderlineService: OrderlineService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.orderService.findbyId(params['orderId']).subscribe(x => this.order = x);
    })
  }
}
