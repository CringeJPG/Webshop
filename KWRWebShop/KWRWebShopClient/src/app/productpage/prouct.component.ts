import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CategoryService } from '../_services/category.service';
import { Category } from '../_models/category';

@Component({
  selector: 'app-prouct',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="user-path">
      <h1>
        <a [routerLink]="['../../categories']">Category</a> >
        {{ category.categoryName }}
      </h1>
    </div>

    <div
      class="product-grid"
      *ngIf="category?.products?.length != 0; else empty"
    >
      <div class="item" *ngFor="let product of category?.products">
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

    <ng-template #empty>
      <div class="empty">
        <h1>No products found.</h1>
      </div>
    </ng-template>
  `,
  styleUrls: ['./product.component.css'],
})
export class ProuctComponent implements OnInit {
  categories: Category[] = [];
  category: Category = {
    categoryId: 0,
    categoryName: '',
    products: [],
  };
  constructor(
    private categoryService: CategoryService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.categoryService
        .findbyId(params['categoryId'])
        .subscribe((category) => (this.category = category));
    });
  }
}
