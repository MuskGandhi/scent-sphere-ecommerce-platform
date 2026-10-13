import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../../models/product';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly api = 'https://localhost:7270/api/products';

  constructor(private http: HttpClient) {}

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.api);
  }

  getProductById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.api}/${id}`);
  }

  search(keyword: string): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.api}/search?keyword=${keyword}`);
  }
}
