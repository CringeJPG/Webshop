import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Product } from '../_models/product';
@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly apiUrl = environment.apiUrl + 'product';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  getRandom(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/random`);
  }

  delete(productId: number): Observable<Product> {
    return this.http.delete<Product>(`${this.apiUrl}/${productId}`);
  }

  create(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  update(productId: number, product: Product): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/${productId}`, product);
  }

  findbyId(productId: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${productId}`);
  }
}
