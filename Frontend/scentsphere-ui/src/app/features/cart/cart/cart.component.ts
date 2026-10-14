import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CartItem } from '../../../models/cart';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];

  totalAmount = 0;

  constructor(
    private readonly cartService: CartService,
    private readonly cdr: ChangeDetectorRef,
    private readonly router: Router,
  ) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.cartService.getCart().subscribe({
      next: (response) => {
        this.cartItems = [...response.items];

        this.totalAmount = response.cartTotal;

        this.cdr.detectChanges();
      },

      error: (err) => {
        console.error('Cart Error:', err);
      },
    });
  }

  increaseQuantity(item: CartItem): void {
    this.cartService
      .updateItem(
        item.cartItemId,

        item.quantity + 1,
      )
      .subscribe(() => {
        this.loadCart();
      });
  }

  decreaseQuantity(item: CartItem): void {
    if (item.quantity <= 1) {
      return;
    }

    this.cartService
      .updateItem(
        item.cartItemId,

        item.quantity - 1,
      )
      .subscribe(() => {
        this.loadCart();
      });
  }

  goToCheckout(): void {
    this.router.navigate(['/checkout'], {
      state: {
        totalAmount: this.totalAmount,
      },
    });
  }
}
