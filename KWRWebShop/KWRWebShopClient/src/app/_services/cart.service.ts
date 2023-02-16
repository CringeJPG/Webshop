import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CartItem } from '../_models/cartItem';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartName = "webshopCart"

  currentCart: Observable<CartItem[]>;

  currentCartSubject: BehaviorSubject<CartItem[]>;

  constructor() {
    this.currentCartSubject = new BehaviorSubject<CartItem[]>(
      JSON.parse(localStorage.getItem(this.cartName) || "[]")
    );
    this.currentCart = this.currentCartSubject.asObservable();
  }

  get currentCartValue(): CartItem[] {
    return this.currentCartSubject.value;
  }

  saveCart(cart: CartItem[]): void {
    localStorage.setItem(this.cartName, JSON.stringify(cart));
    this.currentCartSubject.next(cart);
  }

  addToCart(item: CartItem): void {
    let productFound = false;
    let cart = this.currentCartValue;

    cart.forEach(cartItem => {
      if (cartItem.productId == item.productId) {
        cartItem.amount += item.amount;
        productFound = true;
        if (cartItem.amount <= 0) {
          this.removeItemFromCart(item.productId)
        }
      }
    });

    if (!productFound) {
      cart.push(item);
    }
    this.saveCart(cart);
  }

  removeItemFromCart(productId: number): void {
    let cart = this.currentCartValue;

    for (let i = cart.length - 1; i >= 0; i--) {
      if (cart[i].productId == productId) {
        cart.splice(i, 1);
      }
    }
    this.saveCart(cart);
  }

  clearCart(): void {
    let cart: CartItem[] = [];
    this.saveCart(cart);
  }

  getCartTotal(): number {
    let total: number = 0;
    this.currentCartSubject.value.forEach(item => {
      total += item.price * item.amount;
    });
    return total;
  }
}
