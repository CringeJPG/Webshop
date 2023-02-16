import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Orderline } from '../_models/orderline';

@Injectable({
  providedIn: 'root'
})
export class OrderlineService {

  private readonly apiUrl = environment.apiUrl + 'orderline';

  constructor(private http: HttpClient) { }

  findbyId(orderlineId: number): Observable<Orderline> {
    return this.http.get<Orderline>(`${this.apiUrl}/${orderlineId}`);
  }
  GetAll(): Observable<Orderline[]> {
    return this.http.get<Orderline[]>(this.apiUrl);
  }
}
