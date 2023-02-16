import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const currentUser = this.authService.CurrentUserValue;
    if (currentUser) {
      //send user to login page if user doesn't have permission
      if (route.data['roles'] && route.data['roles'].indexOf(currentUser.loginResponse.type) === -1) {
        this.router.navigate(['customer/login'], { queryParams: { returnUrl: state.url } });
        return false;
      }
      //current user exists, logged in, retun true
      return true;
    }

    //if not logged in, go to login page
    this.router.navigate(['customer/login'], { queryParams: { returnUrl: state.url } });
    return false;

  }

}


