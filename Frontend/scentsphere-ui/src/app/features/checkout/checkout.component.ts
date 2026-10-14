import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';

import { CartItem } from '../../models/cart';
import { CartService } from '../../core/services/cart.service';
import { OrderService } from '../../core/services/order.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent implements OnInit {
  totalAmount = 0;

  cartItems: CartItem[] = [];

  isPlacingOrder = false;

  constructor(
    private readonly cartService: CartService,
    private readonly orderService: OrderService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.cartService.getCart().subscribe({
      next: (response) => {
        this.cartItems = [...response.items];

        this.totalAmount = response.cartTotal;

        this.cdr.detectChanges();
      },

      error: (error) => {
        console.error(error);
      },
    });
  }

  loadCart(): void {
    this.cartService.getCart().subscribe({
      next: (response) => {
        this.cartItems = response.items || [];

        this.totalAmount = response.cartTotal || 0;
      },

      error: (error) => {
        console.error('Cart Load Error:', error);
      },
    });
  }

  placeOrder(): void {
    this.isPlacingOrder = true;

    this.orderService.checkout().subscribe({
      next: (response) => {
        console.log('Order Created:', response);
        this.router.navigate(['/orders']);
      },

      error: (error) => {
        this.isPlacingOrder = false;
        console.error('Checkout Error:', error);
      },
    });
  }
  get totalItems(): number {
    return this.cartItems.reduce(
      (total, item) => total + item.quantity,

      0,
    );
  }
}
