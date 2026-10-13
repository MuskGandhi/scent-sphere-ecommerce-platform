import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AdminProduct } from '../../models/product-management';

@Injectable({
  providedIn: 'root',
})
export class AdminProductService {
  private readonly apiUrl = `${environment.apiUrl}/admin/products`;

  constructor(private http: HttpClient) {}

  createProduct(request: any): Observable<any> {
    return this.http.post(this.apiUrl, request);
  }

  updateProduct(id: string, request: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, request);
  }

  deleteProduct(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  getProducts() {
    return this.http.get<AdminProduct[]>(this.apiUrl);
  }
  getProductById(id: string) {
    return this.http.get<AdminProduct>(`${this.apiUrl}/${id}`);
  }
}
