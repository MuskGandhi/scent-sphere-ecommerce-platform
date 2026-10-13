import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';

import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { Product } from '../../../models/product';
import { ProductService } from '../../../core/services/product.service';
import { AdminProductService } from '../../../core/services/admin-product.service';
import { CategoryService } from '../../../core/services/category.service';
import { BrandService } from '../../../core/services/brand.service';

import { Brand, Category } from '../../../models/product-management';

@Component({
  selector: 'app-product-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-management.component.html',
  styleUrl: './product-management.component.scss',
})
export class ProductManagementComponent implements OnInit {
  products: Product[] = [];

  isLoading = false;

  productForm!: FormGroup;

  showForm = false;

  editingProductId: string | null = null;

  brands: Brand[] = [];

  categories: Category[] = [];

  constructor(
    private productService: ProductService,
    private adminProductService: AdminProductService,
    private brandService: BrandService,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadProducts();

    this.loadBrands();

    this.loadCategories();

    this.productForm = this.fb.group({
      name: ['', Validators.required],

      description: ['', Validators.required],

      price: [0, Validators.required],

      stockQuantity: [0, Validators.required],

      isFeatured: [false],

      brandId: ['', Validators.required],

      categoryId: ['', Validators.required],
    });
  }

  loadProducts(): void {
    this.isLoading = true;

    this.productService.getProducts().subscribe({
      next: (response) => {
        this.products = [...response];

        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: () => {
        this.isLoading = false;
      },
    });
  }

  editProduct(product: Product): void {
    this.adminProductService.getProductById(product.id).subscribe({
      next: (response) => {
        this.showForm = true;

        this.cdr.detectChanges();

        this.editingProductId = response.id;

        this.productForm.patchValue({
          name: response.name,

          description: response.description,

          price: response.price,

          stockQuantity: response.stockQuantity,

          isFeatured: response.isFeatured,

          brandId: response.brandId,

          categoryId: response.categoryId,
        });

        setTimeout(() => {
          document.getElementById('productForm')?.scrollIntoView({
            behavior: 'smooth',

            block: 'start',
          });
        }, 100);
      },
    });
  }

  deleteProduct(product: Product): void {
    if (!confirm(`Delete ${product.name}?`)) {
      return;
    }

    this.adminProductService.deleteProduct(product.id).subscribe({
      next: () => {
        this.products = this.products.filter((x) => x.id !== product.id);

        this.cdr.detectChanges();

        alert('Product Deleted');
      },

      error: (error) => {
        console.error(error);
      },
    });
  }

  openCreateForm(): void {
    this.showForm = true;

    this.editingProductId = null;

    this.productForm.reset({
      price: 0,

      stockQuantity: 0,

      isFeatured: false,
    });

    setTimeout(() => {
      document.getElementById('productForm')?.scrollIntoView({
        behavior: 'smooth',
        block: 'start',
      });
    }, 100);
  }

  saveProduct(): void {
    if (this.productForm.invalid) {
      return;
    }

    const request = {
      name: this.productForm.value.name,

      description: this.productForm.value.description,

      price: this.productForm.value.price,

      stockQuantity: this.productForm.value.stockQuantity,

      isFeatured: this.productForm.value.isFeatured,

      brandId: this.productForm.value.brandId,

      categoryId: this.productForm.value.categoryId,
    };

    // UPDATE
    if (this.editingProductId) {
      this.adminProductService.updateProduct(this.editingProductId, request).subscribe({
        next: () => {
          alert('Product Updated Successfully');

          this.showForm = false;

          this.editingProductId = null;

          this.loadProducts();

          this.cdr.detectChanges();
        },
      });

      return;
    }

    // CREATE
    this.adminProductService.createProduct(request).subscribe({
      next: () => {
        alert('Product Created Successfully');

        this.showForm = false;

        this.productForm.reset();

        this.loadProducts();

        this.cdr.detectChanges();
      },
    });
  }

  loadBrands(): void {
    this.brandService.getBrands().subscribe({
      next: (response) => {
        this.brands = response;
      },
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (response) => {
        this.categories = response;
      },
    });
  }

  trackByProductId(index: number, product: Product): string {
    return product.id;
  }
}
