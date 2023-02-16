import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
} from '@angular/forms';
import { Order } from '../_models/order';
import { OrderService } from '../_services/order.service';
import { RouterModule } from '@angular/router';
import { CartComponent } from '../cart/cart.component';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="orderholder">
      <button routerLink="/admin/admin.dashboard" class="Dashboard">
        Dashboard
      </button>
      <table class="orderTable">
        <tr>
          <th>Handling</th>
          <th>Order Id</th>
          <th>Customer Id</th>
          <!-- <th>Total price</th> -->
          <th>Created</th>
        </tr>
        <tr *ngFor="let order of orders">
          <td>
            <!-- <button class="ProductTableBtn"(click)="edit(order)">Ret</button> -->
            <button class="orderTableBtn" (click)="deleteOrder(order)">
              Cancel
            </button>
          </td>
          <td>
            <a [routerLink]="['/order', order.orderId]">{{ order.orderId }}</a>
          </td>
          <td>{{ order.customerId }}</td>
          <td>{{ order.created }}</td>
        </tr>
      </table>
    </div>
  `,
  styleUrls: ['./orders.component.css'],
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.orderService.getAll().subscribe((x) => (this.orders = x));
  }

  deleteOrder(order: Order): void {
    if (
      confirm('Er du sikker på du vil slette order:  ' + order.orderId + '?')
    ) {
      this.orderService.delete(order.orderId).subscribe((x) => {
        this.orders = this.orders.filter((o) => order.orderId != o.orderId);
      });
    }
  }
}
