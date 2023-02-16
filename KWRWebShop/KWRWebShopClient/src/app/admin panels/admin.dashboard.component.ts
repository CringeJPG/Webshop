import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin.dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="buttonholder">
      <button class="adminbuttons" [routerLink]="['/admin/category']">
        Categories
      </button>
      <button class="adminbuttons" [routerLink]="['/admin/product']">
        Products
      </button>
      <button class="adminbuttons" [routerLink]="['/admin/order']">
        Orders
      </button>
      <button class="adminbuttons" [routerLink]="['/admin/user']">
        Customers
      </button>
    </div>
  `,
  styleUrls: ['./admin.dashboard.component.css'],
})
export class AdminDashboardComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
