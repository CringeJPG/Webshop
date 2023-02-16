import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from './_models/product';
import { ProductService } from './_services/product.service';
import { RouterModule } from '@angular/router';
import { CategoryService } from './_services/category.service';
import { Category } from './_models/category';

@Component({
  selector: 'app-frontpage',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="message">
      <h1>Welcome to KWR Airsoft Webshop</h1>
    </div>
    <div class="random-product-grid">
      <div class="item" *ngFor="let product of randomProducts">
        <img
          [routerLink]="['/product', product.productId]"
          class="product-img"
          src="../assets/images/No_image_available.svg"
          alt=""
        />
        <div class="product-details">
          <h5 class="product-brand">{{ product.brand }}</h5>
          <h5 class="product-name">{{ product.name }}</h5>
          <h5 class="product-price">{{ product.price | currency : 'kr ' }}</h5>
        </div>
      </div>
    </div>
    <div class="divider">
      <hr />
    </div>

    <div class="message">
      <h1>All products</h1>
    </div>
    <div class="all-product-grid">
      <div class="item" *ngFor="let product of products">
        <img
          [routerLink]="['/product', product.productId]"
          class="product-img"
          src="../assets/images/No_image_available.svg"
          alt=""
        />
        <div class="product-details">
          <h5 class="product-brand">{{ product.brand }}</h5>
          <h5 class="product-name">{{ product.name }}</h5>
          <h5 class="product-price">{{ product.price | currency : 'kr ' }}</h5>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./frontpage.components.css'],
})
export class FrontpageComponent implements OnInit {
  randomProducts: Product[] = [];
  products: Product[] = [];

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.productService.getRandom().subscribe((x) => (this.randomProducts = x));
    this.productService.getAll().subscribe((x) => (this.products = x));
  }
}
