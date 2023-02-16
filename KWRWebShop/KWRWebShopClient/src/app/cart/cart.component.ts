import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartItem } from '../_models/cartItem';
import { CartService } from '../_services/cart.service';
import { OrderService } from '../_services/order.service';
import { Orderline } from '../_models/orderline';
import { OrderlineService } from '../_services/orderline.service';
import { Order } from '../_models/order';
import { FormsModule } from '@angular/forms';
import { Customer } from '../_models/customer';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { Login } from '../_models/login';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  orderline: Orderline = {
    orderlineId: 0,
    orderId: 0,
    productId: 0,
    amount: 0,
    price: 0,
  };
  customer: Customer = {
    customerId: 0,
    loginId: 0,
    firstName: '',
    lastName: '',
    address: '',
    created: new Date(),
    login: { loginId: 0, email: '', password: '' },
  };
  order: Order = {
    orderId: 0,
    created: new Date(),
    customerId: 0,
    orderline: [],
    customer: this.customer,
  };
  currentUser: any = {};
  total = this.cartService.getCartTotal();

  constructor(
    public cartService: CartService,
    public orderService: OrderService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.cartService.currentCart.subscribe((x) => (this.cartItems = x));

    this.cartItems.forEach((item) => {
      item.productId, item.name, item.price, item.amount;
    });

    this.authService.currentUser.subscribe(x => {
      this.currentUser = x;
    })
  }

  createOrder(): void {
    if (this.cartItems.length <= 0) {
      return;
    }

    let orderlines: Orderline[] = [];

    for (let i = 0; i < this.cartItems.length; i++) {
      // this.orderline.productId = this.cartItems[i].productId;
      // this.orderline.amount = this.cartItems[i].amount;
      // this.orderline.price = this.cartItems[i].price;

      this.orderline = this.cartItems[i] as Orderline;

      orderlines.push(this.orderline);
      orderlines.forEach((element) => {
        console.log(element.productId);
      });
    }

    this.order = {
      orderId: 0,
      created: new Date(),
      customerId: this.currentUser.loginResponse.customer.customerId,
      orderline: orderlines,
      customer: this.currentUser.loginResponse.customer,
    };

    if (this.currentUser != null) {
      this.orderService.create(this.order).subscribe({
        next: () => {
          alert('Din ordre er nu sat');
          this.clearCart();
        },
        error: (x) => {
          console.log(x);
        },
      });
    }
    else {
      this.router.navigate(['customer/login'])
    }
  }

  amountChange(cartItem: CartItem): void {
    if (cartItem.amount > 0) {
      this.total = this.cartService.getCartTotal();
    } else if (cartItem.amount <= 0) {
      this.cartItems.find((x) => {
        if (x.productId == cartItem.productId) {
          x.amount = 1;
        }
      });
    }
  }

  removeItemFromCart(productId: number): void {
    if (confirm('Er du sikker på at du vil slette denne vare fra kurven?')) {
      this.cartService.removeItemFromCart(productId);
      this.total = this.cartService.getCartTotal();
    }
  }
  clearCartBtn(): void {
    if (this.cartItems.length > 0) {
      if (confirm('Er du sikker på at du vil slette kurven?')) {
        this.clearCart();
      }
    }
  }

  clearCart(): void {
    this.cartService.clearCart();
    this.total = this.cartService.getCartTotal();
  }
}
