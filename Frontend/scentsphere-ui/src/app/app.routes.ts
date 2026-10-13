import { Routes } from '@angular/router';
import { adminGuard } from './core/guards/admin-guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },

  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component').then((m) => m.LoginComponent),
  },

  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register.component').then((m) => m.RegisterComponent),
  },

  {
    path: 'products',
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/products/product-list/product-list.component').then(
            (m) => m.ProductListComponent,
          ),
      },

      {
        path: ':id',
        loadComponent: () =>
          import('./features/products/product-details/product-details.component').then(
            (m) => m.ProductDetailsComponent,
          ),
      },
    ],
  },

  {
    path: 'cart',
    loadComponent: () => import('./features/cart/cart/cart.component').then((m) => m.CartComponent),
  },

  {
    path: 'checkout',
    loadComponent: () =>
      import('./features/checkout/checkout.component').then((m) => m.CheckoutComponent),
  },

  {
    path: 'orders',
    loadComponent: () =>
      import('./features/orders/order-history/order-history.component').then(
        (m) => m.OrderHistoryComponent,
      ),
  },

  {
    path: 'admin/products',
    canActivate: [adminGuard],
    loadComponent: () =>
      import('./features/admin/product-management/product-management.component').then(
        (m) => m.ProductManagementComponent,
      ),
  },

  {
    path: '**',
    redirectTo: 'products',
  },
];
