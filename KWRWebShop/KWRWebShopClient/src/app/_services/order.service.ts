import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order } from '../_models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private readonly apiUrl = environment.apiUrl + 'order';


  constructor(private http: HttpClient) { }

  getAll(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }
  findbyId(orderId: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${orderId}`);
  }
  create(order: Order): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, order);
  }
  delete(orderId: number): Observable<Order> {
    return this.http.delete<Order>(`${this.apiUrl}/${orderId}`)
  }
}
