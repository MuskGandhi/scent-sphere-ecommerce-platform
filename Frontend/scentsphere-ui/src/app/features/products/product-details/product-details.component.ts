import { Component, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';

import { ActivatedRoute, Router } from '@angular/router';

import { Product } from '../../../models/product';

import { ProductService } from '../../../core/services/product.service';

import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
})
export class ProductDetailsComponent implements OnInit {
  product!: Product;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly productService: ProductService,
    private readonly cartService: CartService,
    private readonly router: Router,
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      return;
    }

    this.productService.getProductById(id).subscribe({
      next: (response) => {
        this.product = response;
      },
    });
  }

  addToCart(): void {
    this.cartService.addItem(this.product.id, 1).subscribe({
      next: () => {
        this.router.navigate(['/cart']);
      },
    });
  }
}
