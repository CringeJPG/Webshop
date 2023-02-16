import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Product } from './_models/product';
import { Category } from './_models/category';
import { ProductService } from './_services/product.service';
import { CategoryService } from './_services/category.service';
import { CartItem } from './_models/cartItem';
import { CartService } from './_services/cart.service';

@Component({
  selector: 'app-hero-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="user-path">
      <h1>
        <a [routerLink]="['../../categories']">Category</a> >
        <a [routerLink]="['../../category/', product.category.categoryId]">{{
          product.category.categoryName
        }}</a>
      </h1>
    </div>

    <div class="flex-container">
      <div class="flex-child">
        <img
          class="product-img"
          src="../assets/images/No_image_available.svg"
        />
      </div>

      <div class="flex-child">
        <h2 class="product-brand pb1cm">{{ product.brand }}</h2>
        <h1 class="product-name pb1cm">{{ product.name }}</h1>
        <h3 class="product-description pb1cm">{{ product.description }}</h3>
        <h2 class="product-price pb1cm">
          {{ product.price | currency : 'kr ' }}
        </h2>

        <button class="cart-button" (click)="addToCart(product)">
          Add to cart
        </button>
      </div>
    </div>
  `,
  styleUrls: ['./product-details.component.css'],
})
export class ProductDetailComponent implements OnInit {
  product: Product = {
    productId: 0,
    name: '',
    brand: '',
    description: '',
    price: 0,
    categoryId: 0,
    category: { categoryId: 0, categoryName: '' },
  };

  cartItem: CartItem = { productId: 0, name: '', amount: 0, price: 0 };

  constructor(
    private productService: ProductService,
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.productService
        .findbyId(params['productId'])
        .subscribe((x) => (this.product = x));
    });
  }

  addToCart(product: Product): void {
    this.cartItem = {
      productId: product.productId,
      name: product.name,
      amount: 1,
      price: product.price,
    };
    this.cartService.addToCart(this.cartItem);
  }
}
