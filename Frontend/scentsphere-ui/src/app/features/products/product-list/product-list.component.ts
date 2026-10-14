import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { Product } from '../../../models/product';
import { ProductService } from '../../../core/services/product.service';
import { CartService } from '../../../core/services/cart.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss',
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];

  searchTerm = '';
  currentPage = 1;
  pageSize = 9;
  isLoading = true;

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private cdr: ChangeDetectorRef,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;

    this.productService.getProducts().subscribe({
      next: (data) => {
        this.products = [...data];

        this.currentPage = 1;

        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {
        console.error('Products API Error:', error);

        this.isLoading = false;
      },
    });
  }

  get filteredProducts(): Product[] {
    if (!this.searchTerm) {
      return this.products;
    }

    return this.products.filter(
      (product) =>
        product.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        product.brand.toLowerCase().includes(this.searchTerm.toLowerCase()),
    );
  }
  get pagedProducts(): Product[] {
    if (this.filteredProducts.length <= this.pageSize) {
      return this.filteredProducts;
    }

    const start = (this.currentPage - 1) * this.pageSize;

    return this.filteredProducts.slice(start, start + this.pageSize);
  }
  get totalPages(): number {
    if (this.filteredProducts.length === 0) {
      return 1;
    }

    return Math.ceil(this.filteredProducts.length / this.pageSize);
  }
  get showPagination(): boolean {
    return this.totalPages > 1;
  }
  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }
  addToCart(product: Product): void {
    this.cartService.addItem(product.id, 1).subscribe({
      next: () => {
        this.notificationService.success(`${product.name} added to cart`);
      },
      error: (error) => {
        console.log(error);

        alert('Unable to add item to cart');
      },
    });
  }
  ngOnDestroy(): void {
    this.currentPage = 1;
  }
}
