import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Cart } from '../../models/cart';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private readonly apiUrl = `${environment.apiUrl}/cart`;

  constructor(private http: HttpClient) {}

  getCart(): Observable<Cart> {
    return this.http.get<Cart>(this.apiUrl);
  }

  addItem(productId: string, quantity: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/items`, {
      productId,
      quantity,
    });
  }

  updateItem(cartItemId: string, quantity: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/items/${cartItemId}`, {
      quantity,
    });
  }

  removeItem(cartItemId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/items/${cartItemId}`);
  }
}
