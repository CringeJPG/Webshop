import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private readonly apiUrl = environment.apiUrl + 'category';

  constructor(private http: HttpClient) { }


  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiUrl);
  }

  findbyId(categoryId: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/${categoryId}`);

  }
  create(category: Category): Observable<Category> {
    return this.http.post<Category>(this.apiUrl, category);
  }

  update(categoryId: number, category: Category): Observable<Category> {
    return this.http.put<Category>(this.apiUrl + '/' + categoryId, category);
  }

  delete(categoryId: number): Observable<Category> {
    return this.http.delete<Category>(this.apiUrl + '/' + categoryId);
  }
}
