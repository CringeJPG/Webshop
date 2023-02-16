import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Customer } from '../_models/customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private readonly apiUrl = environment.apiUrl + 'customer';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Customer[]> {
    return this.http.get<Customer[]>(this.apiUrl);
  }

  findbyId(customerId: number): Observable<Customer> {
    return this.http.get<Customer>(this.apiUrl + '/' + 1);
  }
  delete(customerId: number): Observable<Customer> {
    return this.http.delete<Customer>(`${this.apiUrl}/${customerId}`);
  }
  create(user: Customer): Observable<Customer> {
    console.log(user);
    return this.http.post<Customer>(this.apiUrl, user);
  }

  update(customerId: number, user: Customer): Observable<Customer> {
    return this.http.put<Customer>(`${this.apiUrl}/${customerId}`, user);
  }
}
