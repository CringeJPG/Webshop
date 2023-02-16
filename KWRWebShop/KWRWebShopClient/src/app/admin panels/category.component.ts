import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Category } from '../_models/category';
import { CategoryService } from '../_services/category.service';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { catchError } from 'rxjs';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="FormHolder">
      <button routerLink="/admin/admin.dashboard" class="Dashboard">Dashboard</button>
      <h1>Category admin panel</h1>
      <form [formGroup]="categoryForm" (ngSubmit)="save()">
        <div class="formControl">
          <label class="NavnLabel">Category Navn :</label>
          <input class="NavnInput" type="text" formControlName="categoryName" />
        </div>
        <br />
        <br />
        <div class="CategoryButtonHolder">
        <button class="CategoryButtons" [disabled]="!categoryForm.valid">Gem</button>
        <button class="CategoryButtons" type="button" (click)="cancel()">Annuller</button>
        </div>
      </form>
      <br />
      <br />

      <table class="CategoryTable">
        <tr>
          <th>Handling</th>
          <th>Id</th>
          <th>Category</th>
        </tr>

        <tr *ngFor="let category of categories">
          <td>
            <button class="CategoryTableBtn"(click)="edit(category)">Ret</button>
            <button class="CategoryTableBtn"(click)="delete(category)">Slet</button>
          </td>
          <td>{{ category.categoryId }}</td>
          <td>{{ category.categoryName }}</td>
        </tr>
      </table>
    </div>
  `,
  styleUrls: ['./category.component.css'],
})
export class CategoryComponent implements OnInit {
  categories: Category[] = [];
  category: Category = this.resetCategory();

  constructor(private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.categoryService.getAll().subscribe((x) => (this.categories = x));
  }

  edit(category: Category): void {
    console.log(category.categoryId);
    Object.assign(this.category, category);
    this.categoryForm.patchValue(category);
  }

  save(): void {
    if (this.categoryForm.valid && this.categoryForm.touched) {
      if (this.category.categoryId == 0) {
        this.categoryService.create(this.categoryForm.value).subscribe({
          next: (x) => {
            this.categories.push(x);
            this.cancel();
          },
          error: (err) => {
            console.warn(Object.values(err.error.errors).join(', '));
          },
        });
      } else {
        this.categoryService
          .update(this.category.categoryId, this.categoryForm.value)
          .subscribe({
            error: (err) => {
              console.warn(Object.values(err.error.errors).join(', '));
            },
            complete: () => {
              this.categoryService
                .getAll()
                .subscribe((x) => (this.categories = x));
              this.cancel();
            },
          });
      }
    }
  }

  delete(category: Category): void {
    if (
      confirm(
        'Er du sikker på at du vil slette: ' + category.categoryName + '?'
      )
    ) {
      this.categoryService.delete(category.categoryId).subscribe(() => {
        this.categories = this.categories.filter(
          (c) => category.categoryId != c.categoryId
        );
      });
    }
  }

  resetCategory(): Category {
    console.log('reset');
    return { categoryId: 0, categoryName: '', products: [{ productId: 0, name: '', brand: '', description: '', price: 0, categoryId: 0, category: {categoryId: 0, categoryName: ''} }] };
  }

  cancel(): void {
    this.category = this.resetCategory();
    this.categoryForm.reset();
  }

  categoryForm: FormGroup = this.resetForm();

  resetForm(): FormGroup {
    return new FormGroup({
      categoryName: new FormControl(null, Validators.required),
    });
  }
}
