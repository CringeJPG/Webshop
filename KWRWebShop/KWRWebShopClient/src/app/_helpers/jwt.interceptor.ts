import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    //add auth header with jwt if user is logged in and request is to api url
    const currentUser = this.authService.CurrentUserValue;
    const IsLoggedIn = currentUser && currentUser.token;
    const isApiUrl = request.url.startsWith(environment.apiUrl);

    if(IsLoggedIn && isApiUrl){
      request = request.clone({
        setHeaders: {Authorization: `Bearer ${currentUser.token}` }
      });
    }

    return next.handle(request);
  }
}
