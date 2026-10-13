import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';

import { Order } from '../../../models/order';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-order-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-history.component.html',
  styleUrl: './order-history.component.scss',
})
export class OrderHistoryComponent implements OnInit {
  orders: Order[] = [];

  constructor(
    private orderService: OrderService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.orderService.getOrders().subscribe({
      next: (response) => {
        this.orders = [...response];

        this.cdr.detectChanges();
      },

      error: (error) => {
        console.error('Orders Error:', error);
      },
    });
  }
}
