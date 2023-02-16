import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './_helpers/auth.guard';
import { Role } from './_models/role';

const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./frontpage.component').then((it) => it.FrontpageComponent),
  },
  {
    path: 'admin/product',
    loadComponent: () =>
      import('./admin panels/product.component').then(
        (it) => it.ProductComponent
      ),
  },
  {
    path: 'product/:productId',
    loadComponent: () =>
      import('./product-details.component').then(
        (it) => it.ProductDetailComponent
      ),
  },
  { path: 'admin/product', loadComponent: () => import('./admin panels/product.component').then(it => it.ProductComponent), canActivate: [AuthGuard], data: { roles: [Role.Admin] } },
  { path: 'product/:productId', loadComponent: () => import('./product-details.component').then(it => it.ProductDetailComponent) },
  { path: 'customer', loadComponent: () => import('./loginpage/userpage.component').then(it => it.UserpageComponent), canActivate: [AuthGuard], data: { roles: [Role.User] && [Role.Admin] } },
  { path: 'customer/editprofile', loadComponent: () => import('./loginpage/useredit.component').then(it => it.UsereditComponent) },
  { path: 'admin/order', loadComponent: () => import('./admin panels/orders.component').then(it => it.OrdersComponent), canActivate: [AuthGuard], data: { roles: [Role.Admin] } },
  { path: 'order/:orderId', loadComponent: () => import('./order-details.component').then(it => it.OrderDetailsComponent) },
  { path: 'admin/user', loadComponent: () => import('./admin panels/user.component').then(it => it.UserComponent), canActivate: [AuthGuard], data: { roles: [Role.Admin] } },
  { path: 'customer/login', loadComponent: () => import('./loginpage/login.component').then(it => it.LoginComponent) },
  { path: 'customer/register', loadComponent: () => import('./loginpage/register.component').then(it => it.RegisterComponent) },
  {
    path: 'admin/admin.dashboard',

    loadComponent: () =>
      import('./admin panels/admin.dashboard.component').then(
        (it) => it.AdminDashboardComponent
      ),
    canActivate: [AuthGuard], data: { roles: [Role.Admin] }
  },
  {
    path: 'admin/category',
    loadComponent: () =>
      import('./admin panels/category.component').then(
        (it) => it.CategoryComponent
      ),
    canActivate: [AuthGuard], data: { roles: [Role.Admin] }


  },
  {
    path: 'categories',
    loadComponent: () =>
      import('./categoriespage/categorypage.component').then(
        (it) => it.CategoriesComponent
      ),
  },
  {
    path: 'category/:categoryId',
    loadComponent: () => {
      return import('./productpage/prouct.component').then(
        (it) => it.ProuctComponent
      );
    },
  },
  {
    path: 'cart',
    loadComponent: () =>
      import('./cart/cart.component').then((it) => it.CartComponent),
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
