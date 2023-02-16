import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';
import { CartItem } from './_models/cartItem';
import { Login } from './_models/login';
import { Role } from './_models/role';
import { AuthService } from './_services/auth.service';
import { CartService } from './_services/cart.service';

@Component({
  selector: 'app-root',
  template: `
    <div class="header">
      <a routerLink="/" class="logo" routerLinkActive="active" [routerLinkActiveOptions]="{exact:true}">KWR Airsoft Webshop</a>
      <div class="header-right">
        <a routerLink="categories">Categories</a>
        <a routerLink="/admin/admin.dashboard" routerLinkActive="active" *ngIf="currentUser != null && currentUser.loginResponse.type == 'Admin'">Admin Dashboard</a>
        <a routerLink="/cart">Kurv({{ cartTotal }})</a>
        <a routerLink="customer" routerLinkActive="active">Profile</a>
        <a routerLink="customer/login" routerLinkActive="active" *ngIf="currentUser == null"  >Login/Sign up</a>
        <a *ngIf="currentUser != null" class="nav-link" (click)="logout()">Logout</a>
      </div>
    </div>
    <router-outlet></router-outlet>
  `,
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'KWRWebShopClient';
  cart: CartItem[] = [];
  cartTotal: number = 0;
  currentUser: any = { }
  searchText: string = '';

  constructor(private cartService: CartService, private router: Router, private authService: AuthService) {
    this.authService.currentUser.subscribe(x => this.currentUser = x);
    console.log(this.currentUser);
  }

  ngOnInit(): void {
    this.cartService.currentCart.subscribe((x) => {
      this.cart = x;
      console.log(this.currentUser)
      // Sets sum of the total amount of products.
      this.cartTotal = this.cart.reduce<number>((accumulator, obj) => {
        return accumulator + obj.amount;
      }, 0);
    });

  }

  logout() {
    if (confirm('Er du sikker på du vil logge ud')) {
      //ask auth to log out
      this.authService.logout();

      //subscribe to currentuser and load home component
      this.authService.currentUser.subscribe(x => {
        this.currentUser = x;
        this.router.navigate(['/'])
      });
    }
  }
}
