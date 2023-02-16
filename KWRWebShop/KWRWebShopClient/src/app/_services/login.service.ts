import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Login } from '../_models/login';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private readonly apiUrl = environment.apiUrl + 'login';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Login[]> {
    return this.http.get<Login[]>(this.apiUrl);
  }

  findById(loginId: number): Observable<Login> {
    return this.http.get<Login>(`${this.apiUrl}/${loginId}`);
  }

  create(login: Login): Observable<Login> {
    return this.http.post<Login>(this.apiUrl, login);
  }

  update(loginId: number, login: Login): Observable<Login> {
    return this.http.put<Login>(this.apiUrl + '/' + loginId, login);
  }

  delete(loginId: number): Observable<Login> {
    return this.http.delete<Login>(this.apiUrl + '/' + loginId);
  }
}
