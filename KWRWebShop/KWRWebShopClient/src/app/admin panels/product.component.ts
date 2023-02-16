import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
} from '@angular/forms';
import { Product } from '../_models/product';
import { ProductService } from '../_services/product.service';
import { Category } from '../_models/category';
import { CategoryService } from '../_services/category.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="FormHolder">
    <button routerLink="/admin/admin.dashboard" class="Dashboard">Dashboard</button>
    <h1>Product Administration</h1>
<form  [formGroup]="productForm" (ngSubmit)="save()">
    <div class="formControl">
        <label class="NavnLabel">Product Name: </label>
        <input class="ProductInput" type="text" formControlName="name">
        <span class="error" *ngIf="productForm.get('name')?.invalid
                && productForm.get('name')?.touched">Udfyld!</span>
    </div>
    <div class="formControl">
        <label class="NavnLabel">Product Brand: </label>
        <input class="BrandInput" type="text" formControlName="brand">
        <span class="error" *ngIf="productForm.get('name')?.invalid
                && productForm.get('name')?.touched">Udfyld!</span>
    </div>
    <div class="formControl">
        <label class="NavnLabel">Description: </label>
        <input class="DescInput" type="text" formControlName="description">
    </div>
    <div class="formControl">
        <label class="NavnLabel">Price: </label>
        <input class="PriceInput" type="number" formControlName="price">
        <span class="error" *ngIf="productForm.get('price')?.invalid
                && productForm.get('price')?.touched">Udfyld!</span>
    </div>
    <div class="formControl">
        <label class="NavnLabel">category: </label>
        <select class="CategoryInput" formControlName="categoryId">
            <option *ngFor="let category of categories" [ngValue]="category.categoryId">
                {{category.categoryName}}
            </option>
        </select>
    </div>
    <button class="ProductButtons" [disabled]="!productForm.valid">Gem</button>
    <button class="ProductButtons" type="button" (click)="Cancel()">Annuller</button>
</form>
<table class="ProductTable">
    <tr>
        <th>handling</th>
        <th>ID</th>
        <th>Product</th>
        <th>Brand</th>
        <th>Price</th>
    </tr>
    <tr *ngFor="let product of products">
        <td>
            <button class="ProductTableBtn"(click)="edit(product)">Ret</button>
            <button class="ProductTableBtn"(click)="deleteProduct(product)">Slet</button>
        </td>
        <td>{{product.productId}}</td>
        <td>{{product.name}}</td>
        <td>{{product.brand}}</td>    
        <td>{{product.price | currency:'kr'}}</td>    
    </tr>
</table>
    </div>
  `,
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  products: Product[] = [];
  categories: Category[] = [];
  product: Product = this.resetProduct();
  errors: string = '';
  productForm: FormGroup = this.resetForm();

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    this.productService.getAll().subscribe((x) => (this.products = x));
    this.categoryService.getAll().subscribe((x) => (this.categories = x));
  }

  resetForm(): FormGroup {
    return new FormGroup({
      name: new FormControl(null, Validators.required),
      brand: new FormControl(null, Validators.required),
      description: new FormControl(null, Validators.required),
      price: new FormControl(0, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(2500),
      ]),
      categoryId: new FormControl(null, Validators.required),
    });
  }
  resetProduct(): Product {
    return { productId: 0, name: '', brand: '', description: '', price: 0, categoryId: 0, category: { categoryId: 0, categoryName: '' } }
  }

  deleteProduct(product: Product): void {
    if (confirm('Er du sikker på du vil slette ' + product.name + '?')) {
      this.productService.delete(product.productId).subscribe((x) => {
        this.products = this.products.filter(
          (p) => product.productId != p.productId
        );
      });
    }
  }
  save(): void {
    if (this.productForm.valid && this.productForm.touched) {
      if (this.product.productId == 0) {
        this.productService.create(this.productForm.value).subscribe({
          next: (x) => {
            this.products.push(x);
            this.Cancel();
          },
          error: (err) => {
            console.warn(Object.values(err.error.errors).join(', '));
            this.errors = Object.values(err.error.errors).join(', ');
          },
        });
      } else {
        this.productService
          .update(this.product.productId, this.productForm.value)
          .subscribe({
            error: (err) => {
              console.warn(Object.values(err.error.errors).join(', '));
              this.errors = Object.values(err.error.errors).join(', ');
            },
            complete: () => {
              this.productService
                .getAll()
                .subscribe((x) => (this.products = x));
              this.Cancel();
            },
          });
      }
    }
  }
  Cancel(): void {
    this.product = this.resetProduct();
    this.errors = '';
    this.productForm = this.resetForm();
  }
  edit(product: Product): void {
    product.categoryId = product.category.categoryId;
    Object.assign(this.product, product); //Copies the values of the hero u click on and pastes it in the input boxes
    this.productForm.patchValue(product);
  }
}
