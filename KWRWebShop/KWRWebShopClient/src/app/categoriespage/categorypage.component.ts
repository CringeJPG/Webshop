import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../_services/category.service';
import { Category } from '../_models/category';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-categorypage',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="category-grid">
      <div
        [routerLink]="['/category', category.categoryId]"
        class="item"
        *ngFor="let category of categories"
      >
        <h3 class="category-header">
          <a>{{ category.categoryName }}</a>
        </h3>
      </div>
    </div>
  `,
  styleUrls: ['./categorypage.component.css'],
})
export class CategoriesComponent implements OnInit {
  categories: Category[] = [];
  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.categoryService.getAll().subscribe((x) => (this.categories = x));
  }
}
